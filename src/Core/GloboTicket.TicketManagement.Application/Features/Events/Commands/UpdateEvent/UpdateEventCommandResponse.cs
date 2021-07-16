using System;
using System.Collections.Generic;
using System.Text;
using GloboTicket.TicketManagement.Application.Responses;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandResponse : BaseResponse
    {
        public UpdateEventCommandResponse()
            : base()
        {
        }

        public UpdateEventDto Event { get; set; }
    }
}
