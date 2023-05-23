using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation("Order with order id {0} has been successfully created", newOrder.Id);
            await SendMail(newOrder);
            return newOrder.Id;
        }

        private async Task SendMail(Order newOrder)
        {
            var email = new Email { To = newOrder.EmailAddress, Body = $"Your order has been created successfully", Subject = "Your order is on the way" };
            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Order: {0} has failed due to an error with the mail service {1}", newOrder.Id, ex.Message);
            }
        }
    }
}
