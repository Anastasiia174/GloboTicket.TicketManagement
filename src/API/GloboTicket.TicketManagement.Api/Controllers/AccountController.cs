using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboTicket.TicketManagement.Application.Contracts.Identity;
using GloboTicket.TicketManagement.Application.Models.Authentication;

namespace GloboTicket.TicketManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register", Name = "Register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var result = await _authenticationService.RegisterAsync(registrationRequest);
            return Ok(result);
        }

        [HttpPost("authenticate", Name = "Authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate(
            [FromBody] AuthenticationRequest authenticationRequest)
        {
            var result = await _authenticationService.AuthenticateAsync(authenticationRequest);
            return Ok(result);
        }
    }
}
