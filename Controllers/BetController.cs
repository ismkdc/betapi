using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BetApi.Models;
using BetApi.Services;

namespace BetApi.Controllers
{
    public class BetController : Controller
    {
        readonly BetService _betService;
        public BetController(BetService betService)
        {
            _betService = betService;
        }
        [HttpGet]
        [Route("values")]
        public IEnumerable<Bet> Get()
        {
            return _betService.GetBets();
        }

    }
}
