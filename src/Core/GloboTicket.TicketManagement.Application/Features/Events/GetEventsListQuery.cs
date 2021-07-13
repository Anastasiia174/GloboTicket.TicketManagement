using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events
{
    public class GetEventsListQuery : IRequest<List<EventListVm>>
    {
    }
}
