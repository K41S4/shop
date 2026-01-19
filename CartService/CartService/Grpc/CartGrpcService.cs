using CartApp.BusinessLogic.Exceptions;
using CartApp.BusinessLogic.Services;
using CartApp.Models;
using Grpc.Core;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;

namespace CartApp.Grpc;

/// <summary>
/// gRPC service for cart operations.
/// </summary>
[AllowAnonymous]
public class CartGrpcService : CartService.CartServiceBase
{
    private readonly ICartService cartService;
    private readonly ILogger<CartGrpcService> logger;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartGrpcService"/> class.
    /// </summary>
    /// <param name="cartService">The cart service.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="mapper">The mapper.</param>
    public CartGrpcService(ICartService cartService, ILogger<CartGrpcService> logger, IMapper mapper)
    {
        this.cartService = cartService;
        this.logger = logger;
        this.mapper = mapper;
    }

    /// <summary>
    /// Unary RPC: Get list of items of the cart object.
    /// </summary>
    /// <param name="request">The request with cart ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The response with cart items.</returns>
    public override async Task<GetCartItemsResponse> GetCartItems(
        GetCartItemsRequest request,
        ServerCallContext context)
    {
        this.logger.LogInformation("Unary RPC: Getting cart items for cart ID: {CartId}", request.CartId);

        try
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var items = await this.cartService.GetCartItems(request.CartId);

            var response = new GetCartItemsResponse();
            response.Items.AddRange(this.mapper.Map<IEnumerable<CartItemResponse>>(items));

            this.logger.LogInformation(
                "Unary RPC: Successfully retrieved {Count} items for cart ID: {CartId}",
                items.Count,
                request.CartId);

            return response;
        }
        catch (OperationCanceledException)
        {
            this.logger.LogWarning("Unary RPC: Operation was cancelled for cart ID: {CartId}", request.CartId);
            throw new RpcException(new Status(StatusCode.Cancelled, "Operation was cancelled"));
        }
        catch (NotFoundException ex)
        {
            this.logger.LogWarning("Unary RPC: Cart not found for cart ID: {CartId}", request.CartId);
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unary RPC: Error getting cart items for cart ID: {CartId}", request.CartId);
            throw new RpcException(new Status(StatusCode.Internal, $"Error getting cart items: {ex.Message}"));
        }
    }

    /// <summary>
    /// Server stream RPC: Get list of items of the cart object.
    /// </summary>
    /// <param name="request">The request with cart ID.</param>
    /// <param name="responseStream">The response stream.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>A task.</returns>
    public override async Task GetCartItemsStream(
        GetCartItemsRequest request,
        IServerStreamWriter<CartItemResponse> responseStream,
        ServerCallContext context)
    {
        this.logger.LogInformation("Server Stream RPC: Getting cart items stream for cart ID: {CartId}", request.CartId);

        try
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var items = await this.cartService.GetCartItems(request.CartId);

            foreach (var item in items)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                var itemResponse = this.mapper.Map<CartItemResponse>(item);
                await responseStream.WriteAsync(itemResponse);

                this.logger.LogDebug(
                    "Server Stream RPC: Sent item {ItemId} for cart ID: {CartId}",
                    item.Id,
                    request.CartId);

                // delay for streaming test
                await Task.Delay(1000, context.CancellationToken);
            }

            this.logger.LogInformation(
                "Server Stream RPC: Successfully streamed {Count} items for cart ID: {CartId}",
                items.Count,
                request.CartId);
        }
        catch (OperationCanceledException)
        {
            this.logger.LogWarning("Server Stream RPC: Operation was cancelled for cart ID: {CartId}", request.CartId);
            throw new RpcException(new Status(StatusCode.Cancelled, "Operation was cancelled"));
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Server Stream RPC: Error streaming cart items for cart ID: {CartId}", request.CartId);
            throw new RpcException(new Status(StatusCode.Internal, $"Error streaming cart items: {ex.Message}"));
        }
    }

    /// <summary>
    /// Client stream RPC: Add item to the cart and return updated cart object.
    /// </summary>
    /// <param name="requestStream">The request stream.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The response with the updated cart.</returns>
    public override async Task<CartResponse> AddItemsToCart(
        IAsyncStreamReader<AddItemRequest> requestStream,
        ServerCallContext context)
    {
        this.logger.LogInformation("Client Stream RPC: Starting to receive items to add to cart");

        string? cartId = null;
        var itemsToAdd = new List<CartItem>();

        try
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrEmpty(cartId))
                {
                    if (string.IsNullOrEmpty(request.CartId))
                    {
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "Cart ID is required."));
                    }

                    cartId = request.CartId;
                    this.logger.LogInformation("Client Stream RPC: Processing items for cart ID: {CartId}", cartId);
                }
                else if (request.CartId != cartId)
                {
                    throw new RpcException(new Status(
                        StatusCode.InvalidArgument,
                        $"All requests in the stream must target the same cart. Expected: {cartId}, received: {request.CartId}"));
                }

                var cartItem = this.mapper.Map<CartItem>(request.Item);
                itemsToAdd.Add(cartItem);

                this.logger.LogDebug(
                    "Client Stream RPC: Received item {ItemId} (Quantity: {Quantity}) for cart ID: {CartId}",
                    cartItem.Id,
                    cartItem.Quantity,
                    cartId);
            }

            if (string.IsNullOrEmpty(cartId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Cart ID is required"));
            }

            foreach (var item in itemsToAdd)
            {
                await this.cartService.AddItemToCart(cartId, item);
            }

            var cart = await this.cartService.GetCart(cartId);
            if (cart == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Cart with ID {cartId} was not found"));
            }

            var mappedItems = this.mapper.Map<ICollection<CartItemResponse>>(cart.Items);
            var response = new CartResponse
            {
                CartId = cart.Id,
                Items = { mappedItems },
            };

            this.logger.LogInformation(
                "Client Stream RPC: Successfully added {Count} items to cart ID: {CartId}",
                itemsToAdd.Count,
                cartId);

            return response;
        }
        catch (OperationCanceledException)
        {
            this.logger.LogWarning("Client Stream RPC: Operation was cancelled");
            throw new RpcException(new Status(StatusCode.Cancelled, "Operation was cancelled"));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Client Stream RPC: Error adding items to cart");
            throw new RpcException(new Status(StatusCode.Internal, $"Error adding items to cart: {ex.Message}"));
        }
    }

    /// <summary>
    /// Bi-directional RPC: Add item to the cart and return updated cart object.
    /// </summary>
    /// <param name="requestStream">The request stream.</param>
    /// <param name="responseStream">The response stream.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>A task.</returns>
    public override async Task AddItemsToCartBidirectional(
        IAsyncStreamReader<AddItemRequest> requestStream,
        IServerStreamWriter<CartResponse> responseStream,
        ServerCallContext context)
    {
        this.logger.LogInformation("Bi-directional RPC: Starting bidirectional stream for adding items to cart");

        string? cartId = null;

        try
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrEmpty(cartId))
                {
                    if (string.IsNullOrEmpty(request.CartId))
                    {
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "Cart ID is required."));
                    }

                    cartId = request.CartId;
                    this.logger.LogInformation("Bi-directional RPC: Processing items for cart ID: {CartId}", cartId);
                }
                else if (request.CartId != cartId)
                {
                    throw new RpcException(new Status(
                        StatusCode.InvalidArgument,
                        $"All requests in the stream must target the same cart. Expected: {cartId}, received: {request.CartId}"));
                }

                var cartItem = this.mapper.Map<CartItem>(request.Item);
                await this.cartService.AddItemToCart(cartId, cartItem);

                this.logger.LogInformation(
                    "Bi-directional RPC: Added item {ItemId} (Quantity: {Quantity}) to cart ID: {CartId}",
                    cartItem.Id,
                    cartItem.Quantity,
                    cartId);

                var cart = await this.cartService.GetCart(cartId);
                if (cart == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Cart with ID {cartId} was not found"));
                }

                var mappedItems = this.mapper.Map<ICollection<CartItemResponse>>(cart.Items);
                var response = new CartResponse
                {
                    CartId = cart.Id,
                    Items = { mappedItems },
                };

                await responseStream.WriteAsync(response);

                this.logger.LogDebug(
                    "Bi-directional RPC: Sent updated cart with {Count} items for cart ID: {CartId}",
                    cart.Items.Count,
                    cartId);
            }

            this.logger.LogInformation("Bi-directional RPC: Successfully completed bidirectional stream for cart ID: {CartId}", cartId);
        }
        catch (OperationCanceledException)
        {
            this.logger.LogWarning("Bi-directional RPC: Operation was cancelled");
            throw new RpcException(new Status(StatusCode.Cancelled, "Operation was cancelled"));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Bi-directional RPC: Error in bidirectional stream");
            throw new RpcException(new Status(StatusCode.Internal, $"Error in bidirectional stream: {ex.Message}"));
        }
    }

}