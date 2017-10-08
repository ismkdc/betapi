using BetApi.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BetApi.Services
{
    public class BetService
    {
        private IMemoryCache _cache;
        public BetService(IMemoryCache cache)
        {
            _cache = cache; ;

        }
        public IEnumerable<Bet> GetBets()
        {
            IEnumerable<Bet> bets = null;
            if (!_cache.TryGetValue("BetsCache", out bets))
            {
                if (!File.Exists("bets"))
                {
                    var file = File.Create("bets");
                    file.Close();
                    GetAndWriteBets();
                }
                else
                {
                    Task.Factory.StartNew(() => GetAndWriteBets());
                }
                var _bets = JsonConvert.DeserializeObject<IEnumerable<Bet>>(File.ReadAllText("bets"));
                bets = _bets;

                

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(3));

                _cache.Set("BetsCache", _bets, cacheEntryOptions);
            }
            return bets;
        }

        string EscapeString(string text)
        {
            return text.Replace("&nbsp;", "").Replace("\n", "").Replace("\r", "");
        }

        void GetAndWriteBets()
        {
            var url = "https://www.misli.com/iddaa-hazir-kuponlar";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var item = doc.DocumentNode.SelectNodes("//table[@class='com-sub-header-table tblPCDetail']/tr");
            var _bets = item.Where(x => !String.IsNullOrWhiteSpace(EscapeString(x.SelectNodes(".//td")[0].InnerText.Trim()))).Select(x => new Bet
            {
                Code = EscapeString(x.SelectNodes(".//td")[0].InnerText.Trim()),
                Match = EscapeString(x.SelectNodes(".//td")[1].InnerText.Trim()),
                Type = EscapeString(x.SelectNodes(".//td")[2].InnerText.Trim()),
                Guess = EscapeString(x.SelectNodes(".//td")[3].InnerText.Trim()),
                Rate = EscapeString(x.SelectNodes(".//td")[4].InnerText.Trim()),
            });

            File.WriteAllText("bets", JsonConvert.SerializeObject(_bets));

        }
    }
}
