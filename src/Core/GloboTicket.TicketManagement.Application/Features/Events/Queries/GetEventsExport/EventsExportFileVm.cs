using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport
{
    public class EventsExportFileVm
    {
        public string ContentType { get; set; }
        public string EventExportFileName { get; set; }
        public byte[] Data { get; set; }
    }
}
