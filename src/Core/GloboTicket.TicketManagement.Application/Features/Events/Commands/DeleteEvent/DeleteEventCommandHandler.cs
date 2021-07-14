using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IAsyncRepository<Event> _eventsRepository;

        public DeleteEventCommandHandler(IAsyncRepository<Event> eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToDelete = await _eventsRepository.GetByIdAsync(request.EventId);

            await _eventsRepository.DeleteAsync(eventToDelete);

            return Unit.Value;
        }
    }
}
