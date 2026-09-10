using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController(
    IMediator mediator,
    ILogger<BasketController> logger) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<ActionResult<ShoppingCartDto>> GetBasket(string username)
    {
        try
        {
            var query = new GetBasketByUserNameQuery(username);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting basket for username: {Username}", username);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCartDto>> CreateOrUpdateBasket(
        [FromBody] CreateShoppingCartCommand command)
    {
        try
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating or updating basket");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message });
        }
    }

    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteBasket(string username)
    {
        try
        {
            var command = new DeleteBasketByUserNameCommand(username);

            await mediator.Send(command);

            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error while deleting basket for username: {Username}",
                username);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message });
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckoutDto dto)
    {
        await mediator.Send(new BasketCheckoutCommand(dto));
        return Accepted();
    }
}
