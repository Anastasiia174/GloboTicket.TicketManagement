using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;

namespace GloboTicket.TicketManagement.Infrastructure.FileExport
{
    public class CsvExporter : ICsvExporter
    {
        public byte[] ExportEventsToCsv(List<EventsExportDto> eventsExportDtos)
        {
            using var memoryStream = new MemoryStream();
            
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.CurrentCulture));
                csvWriter.WriteRecords(eventsExportDtos);
            }

            return memoryStream.ToArray();
        }
    }
}
