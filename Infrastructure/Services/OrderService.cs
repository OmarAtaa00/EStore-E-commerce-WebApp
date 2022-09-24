using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.OrderAggregate;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork, ICartRepository cartRepo)
        {
            _unitOfWork = unitOfWork;
            _cartRepo = cartRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId,
         string cartId, Address shippingAddress)
        {
            //check for the price if the items in the cart with our price in the database


            // get cart from repo
            var cart = _cartRepo.GetCartAsync(cartId);

            // get items from the product repo 
            var items = new List<OrderItem>();

            foreach (var item in items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,
                productItem.PictureUrl);
                //here we are getting the price from the database if the client said
                //the price is zero does not matter we don't get the price from the client
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);

            }

            // get delivery method 
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // clc SubTotal 
            var subTotal = items.Sum(item => item.Price * item.Quantity);

            //create order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, items, subTotal);
            //save to db

            var result = await _unitOfWork.Complete();
            // unit of work owns the repos so all the changes will apply or none 
            // so we don't have partial update
            if (result <= 0) return null;

            // delete cart 
            await _cartRepo.DeleteCartAsync(cartId);

            //return order
            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new ItemsAndOrderingSpec(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(string buyerEmail)
        {
            var spec = new ItemsAndOrderingSpec(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
