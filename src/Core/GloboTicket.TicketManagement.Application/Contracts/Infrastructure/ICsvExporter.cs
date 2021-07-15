using System;
using System.Collections.Generic;
using System.Text;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;

namespace GloboTicket.TicketManagement.Application.Contracts.Infrastructure
{
    public interface ICsvExporter
    {
        byte[] ExportEventsToCsv(List<EventsExportDto> eventsExportDtos);
    }
}
