using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Order : ControllerBase
    {
        private readonly IMediator _mediator;

        public Order(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string userName)
        {
            var orders = await _mediator.Send(new GetOrdersListQuery(userName));
            return Ok(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();

        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _mediator.Send(new DeleteOrderCommand { Id = id });
            return NoContent();
        }

    }
}
