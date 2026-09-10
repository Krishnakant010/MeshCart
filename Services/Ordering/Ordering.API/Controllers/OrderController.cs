using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.CreateOrder;
using Ordering.Application.Orders.DeleteOrder;
using Ordering.Application.Orders.GetOrders;
using Ordering.Application.Orders.UpdateOrder;
using Ordering.Application.DTOs;
using Ordering.Application.Mapper;
using Ordering.Application.Abstractions;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController(
        ICommandHandler<CreateOrderCommand, int> createOrderHandler,
        ICommandHandler<UpdateOrderCommand> updateOrderHandler,
        ICommandHandler<DeleteOrderCommand> deleteOrderHandler,
        IQueryHandler<GetOrderListQuery, List<OrderDto>> getOrderListHandler,
        ILogger<OrderController> logger)
        : ControllerBase
    {
        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUserName(string userName, CancellationToken cancellationToken)
        {
            var query = new GetOrderListQuery(userName);

            var orders = await getOrderListHandler.Handle(query, cancellationToken);

            logger.LogInformation("Orders fetched for user {UserName}", userName);
            return Ok(orders);
        }

        // Testing purpose
        [HttpPost(Name = "CheckoutOrder")]
        public async Task<ActionResult<int>> CheckoutOrder(
            [FromBody] CreateOrderDto dto,
            CancellationToken cancellationToken)
        {
            var command = dto.ToCommand();

            var orderId = await createOrderHandler.Handle(command, cancellationToken);

            logger.LogInformation("Order created with Id {OrderId}", orderId);
            return Ok(orderId);
        }

        [HttpPut(Name = "UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(
            [FromBody] OrderDto dto,
            CancellationToken cancellationToken)
        {
            var command = dto.ToCommand();

            await updateOrderHandler.Handle(command, cancellationToken);

            logger.LogInformation("Order updated with Id {OrderId}", dto.Id);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteOrderCommand(id);

            await deleteOrderHandler.Handle(command, cancellationToken);

            logger.LogInformation("Order deleted with Id {OrderId}", id);
            return NoContent();
        }
    }
}