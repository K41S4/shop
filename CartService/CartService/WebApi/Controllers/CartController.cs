using CartApp.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartApp.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController(ICartService service) : ControllerBase
{

}
