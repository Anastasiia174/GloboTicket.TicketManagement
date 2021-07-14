﻿using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest
    {
        public Guid EventId { get; set; }
    }
}
