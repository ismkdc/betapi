using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetApi.Models
{
    public class Bet
    {
        public string Code { get; set; }
        public string Match { get; set; }
        public string Type { get; set; }
        public string Guess { get; set; }
        public string Rate { get; set; }
    }
}
