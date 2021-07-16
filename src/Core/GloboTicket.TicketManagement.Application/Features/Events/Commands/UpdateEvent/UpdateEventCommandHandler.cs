using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, UpdateEventCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Event> _eventRepository;

        public UpdateEventCommandHandler(IMapper mapper, IAsyncRepository<Event> eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }
        public async Task<UpdateEventCommandResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateEventCommandResponse();
            var eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId);

            if (eventToUpdate == null)
            {
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            var validator = new UpdateEventCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                response.Success = false;
                response.ValidationErrors = new List<string>();

                validationResult.Errors.ForEach(e => response.ValidationErrors.Add(e.ErrorMessage));
            }

            if (response.Success)
            {
                _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));

                await _eventRepository.UpdateAsync(eventToUpdate);

                response.Event = _mapper.Map<UpdateEventDto>(eventToUpdate);
            }

            return response;
        }
    }
}
