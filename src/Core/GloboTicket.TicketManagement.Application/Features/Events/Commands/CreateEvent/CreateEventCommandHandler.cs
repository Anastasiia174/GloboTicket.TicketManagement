using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateEventCommandHandler> _logger;

        public CreateEventCommandHandler(
            IMapper mapper, 
            IEventRepository eventRepository, 
            IEmailService emailService, 
            ILogger<CreateEventCommandHandler> logger)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<CreateEventCommandResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateEventCommandResponse();

            var validator = new CreateEventCommandValidator(_eventRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                response.Success = false;
                response.ValidationErrors = new List<string>();

                validationResult.Errors.ForEach(e => response.ValidationErrors.Add(e.ErrorMessage));
            }

            if (response.Success)
            {
                var @event = _mapper.Map<Event>(request);

                @event = await _eventRepository.AddAsync(@event);
                response.Event = _mapper.Map<CreateEventDto>(@event);

                var email = new Email()
                {
                    To = "gill@snoball.be",
                    Body = $"An event var created: {request}",
                    Subject = "A new event was created"
                };

                try
                {
                    await _emailService.SendEmail(email);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Mailing about event {@event.EventId} failed due to exception: {ex.Message}");
                }
            }
            

            return response;
        }
    }
}
