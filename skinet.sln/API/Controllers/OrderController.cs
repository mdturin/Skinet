using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrderAsync(OrderDTO orderDto)
    {
        var email = HttpContext.User.RetriveEmailFromPrincipal();
        var address = _mapper.Map<AddressDTO, Address>(orderDto.ShipToAddress);
        var order = await _orderService.CreateOrderAsync(
            email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

        if (order == null) 
            return BadRequest(new ApiResponse(400, "Problem creating order"));

        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<OrderToReturnDTO>>> GetOrdersForUser()
    {
        var email = HttpContext.User.RetriveEmailFromPrincipal();
        var orders = await _orderService.GetOrdersForUserAsync(email);
        IReadOnlyCollection<OrderToReturnDTO> ordersDTO = 
            _mapper.Map<IReadOnlyCollection<Order>, IReadOnlyCollection<OrderToReturnDTO>>(orders);
        return base.Ok(ordersDTO);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
    {
        var email = HttpContext.User.RetriveEmailFromPrincipal();
        var order = await _orderService.GetOrderByIdAsync(id, email);
        if (order == null) return NotFound(new ApiResponse(404)); 
        return Ok(_mapper.Map<Order, OrderToReturnDTO>(order));
    }

    [HttpGet("deliveryMethods")]
    public async Task<ActionResult<IReadOnlyCollection<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await _orderService.GetDeliveryMethodsAsync());
    }
}
