using Newtonsoft.Json;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Repository
{
    public class StockDataRepo : IStockDataRepo
    {
        private static readonly HttpClient _httpclient = new HttpClient();
        private string _baseurl = "https://www.alphavantage.co";
        private string _apikey = "2UE2F3PIEK3EAKYP";

        // url: https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=2UE2F3PIEK3EAKYP
        public async Task<Dictionary<DateTime, StockPriceAdjustDaily>> GetStockPriceAdjustDaily(DateTime from, DateTime to, string symbol)
        {
            // generate uri
            string uri = _baseurl + $"/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_apikey}";
            HttpResponseMessage response = await _httpclient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Dictionary<string, dynamic> bodyobject = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseBody);
            string timeSeriesStr = JsonConvert.SerializeObject(bodyobject["Time Series (Daily)"]);
            Dictionary<string, Dictionary<string, string>> dailyprices = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(timeSeriesStr);
            var convertedprices =
                dailyprices.Select(
                    x => new KeyValuePair<DateTime, StockPriceAdjustDaily>(
                        DateTime.Parse(x.Key),
                        new StockPriceAdjustDaily
                        {
                            Open = decimal.Parse(x.Value["1. open"]),
                            High = decimal.Parse(x.Value["2. high"]),
                            Low = decimal.Parse(x.Value["3. low"]),
                            Closed = decimal.Parse(x.Value["4. close"]),
                            Volume = int.Parse(x.Value["5. volume"]),
                        }));

            Dictionary<DateTime, StockPriceAdjustDaily> result = new Dictionary<DateTime, StockPriceAdjustDaily>();
            foreach(var kv in convertedprices)
            {
                result[kv.Key] = kv.Value;
            }

            return result;
        }
    }
}
