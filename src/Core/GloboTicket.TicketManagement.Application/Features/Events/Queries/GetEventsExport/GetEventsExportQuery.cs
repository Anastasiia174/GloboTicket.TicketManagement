using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport
{
    public class GetEventsExportQuery : IRequest<EventsExportFileVm>
    {
    }
}
