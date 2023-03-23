using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basket;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository basket, DiscountGrpcService discountGrpcService)
        {
            _basket = basket;
            _discountGrpcService = discountGrpcService;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basket.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost("{userName}", Name = "UpdateBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //TODO: consume grpc and calculate the new price
            foreach (var product in basket.Items)
            {
                var coupon =await _discountGrpcService.GetDiscount(product.ProductName);
                product.Price -= coupon.Amount;
            }
            return Ok(await _basket.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _basket.DeleteBasket(userName);
            return Ok();
        }
    }
}
