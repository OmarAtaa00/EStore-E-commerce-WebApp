using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartRepository _cartRepo;
        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;

        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetCartById(string id)
        {
            var cart = await _cartRepo.GetCartAsync(id);
            return Ok(cart ?? new Cart(id));
        }
        [HttpPost]
        public async Task<ActionResult<Cart>> UpdateOrCreateCart(Cart cart)
        {
            var updatedCart = await _cartRepo.UpdateOrCreateCart(cart);
            return Ok(updatedCart);
        }
        [HttpDelete]

        public async Task DeleteCart(string id)
        {
            await _cartRepo.DeleteCartAsync(id);
        }
    }

}

