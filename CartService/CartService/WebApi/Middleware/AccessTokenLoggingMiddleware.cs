using System.IdentityModel.Tokens.Jwt;

namespace CartApp.WebApi.Middleware;

/// <summary>
/// Middleware to log access token details.
/// </summary>
public class AccessTokenLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<AccessTokenLoggingMiddleware> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTokenLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next.</param>
    /// <param name="logger">The logger.</param>
    public AccessTokenLoggingMiddleware(RequestDelegate next, ILogger<AccessTokenLoggingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to log access token details.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Task.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var token = ExtractTokenFromRequest(context);
            if (!string.IsNullOrEmpty(token))
            {
                this.LogTokenDetails(token);
            }
        }

        await this.next(context);
    }

    /// <summary>
    /// Extracts the access token from the Authorization header.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>The access token if found, otherwise null.</returns>
    private static string? ExtractTokenFromRequest(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authHeader.Substring("Bearer ".Length).Trim();
    }

    /// <summary>
    /// Logs detailed information about the access token.
    /// </summary>
    /// <param name="token">The access token string.</param>
    private void LogTokenDetails(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                this.logger.LogInformation(
                    "Access Token Details - " +
                    "Issuer: {Issuer}, " +
                    "Subject: {Subject}, " +
                    "Valid From: {ValidFrom}, " +
                    "Valid To: {ValidTo} ",
                    jwtToken.Issuer,
                    jwtToken.Subject,
                    jwtToken.ValidFrom,
                    jwtToken.ValidTo);

                if (!jwtToken.Claims.Any())
                {
                    return;
                }

                var claimsDetails = string.Join(", ", jwtToken.Claims.Select(c => $"{c.Type}: {c.Value}"));

                this.logger.LogInformation("Token Claims: {Claims}", claimsDetails);
            }
            else
            {
                this.logger.LogWarning("Unable to read token - invalid or malformed.");
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occurred while logging token details.");
        }
    }
}
