using System;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;
        public CartController(ICartRepository cartRepo, IMapper mapper)
        {
            _mapper = mapper;
            _cartRepo = cartRepo;

        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetCartById(string id)
        {
            var cart = await _cartRepo.GetCartAsync(id);
            return Ok(cart ?? new Cart(id)); // id Created by the Client-side
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> UpdateOrCreateCart(CartDto cart)
        {
            var customerCart = _mapper.Map<CartDto, Cart>(cart);
            var updatedCart = await _cartRepo.UpdateOrCreateCart(customerCart);
            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task DeleteCart(string id)
        {
            await _cartRepo.DeleteCartAsync(id);
        }
    }

}

