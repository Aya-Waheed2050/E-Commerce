namespace Presentation.Controllers
{
 
    public class BasketsController(IServiceManager _serviceManager) : ApiBaseController
    {

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string key) 
        {
            var Basket = await _serviceManager.BasketService.GetBasketAsync(key);
            return Ok(Basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
        {
            var Basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }


        [HttpDelete("{key:guid}")]
        public async Task<ActionResult<bool>> DeleteBasket(string Key)
        {
            var Basket = await _serviceManager.BasketService.DeleteBasketAsync(Key);
            return Ok(Basket);
        }


    }
}
