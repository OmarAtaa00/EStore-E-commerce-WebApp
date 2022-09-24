using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Interfaces;
using Core.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderService = orderService;

        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            //here we can't use the clean version of getting the email from UserManager we use the http one
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            // instead of doing this again and again make an extension to use it ^^ directly 
            var email = User.GetEmailFromPrincipal();

            // map to orderAggregate address
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            // create order
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodID, orderDto.CartId, address);

            _unitOfWork.Repository<Order>().Add(order);

            // check of we got an order from our service 
            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
            // return order 

            return Ok(order);

        }


        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var email = User.GetEmailFromPrincipal();


            var orders = await _orderService.GetOrdersAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrdersById(int id)
        {
            var email = User.GetEmailFromPrincipal();
            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Order, OrderToReturnDto>(order);

        }

        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            return Ok(await _orderService.GetDeliveryMethodAsync());
        }

    }
}