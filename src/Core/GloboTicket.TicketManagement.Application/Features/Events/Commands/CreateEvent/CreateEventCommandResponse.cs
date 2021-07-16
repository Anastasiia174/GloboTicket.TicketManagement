using System;
using System.Collections.Generic;
using System.Text;
using GloboTicket.TicketManagement.Application.Responses;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandResponse : BaseResponse
    {
        public CreateEventCommandResponse()
            : base()
        {
        }

        public CreateEventDto Event { get; set; }
    }
}
