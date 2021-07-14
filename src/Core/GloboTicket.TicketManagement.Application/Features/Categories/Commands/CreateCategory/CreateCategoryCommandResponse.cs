using System;
using System.Collections.Generic;
using System.Text;
using GloboTicket.TicketManagement.Application.Responses;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandResponse : BaseResponse
    {
        public CreateCategoryCommandResponse()
            : base()
        {
        }

        public CreateCategoryDto Category { get; set; }
    }
}
