using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Model.Extensions
{
    public static class DictionaryExtension
    {
        public static DateTime GetPreviousAvailableDate<T>(this Dictionary<DateTime, T> dict, DateTime current, int maxlookup = 5)
        {
            DateTime result = current.AddDays(-1);
            while (!dict.ContainsKey(result) && maxlookup > 0)
            {
                result = result.AddDays(-1);
                maxlookup--;
            }
            return maxlookup == 0 ? new DateTime() : result;
        }

        public static DateTime GetNextAvailableDate<T>(this Dictionary<DateTime, T> dict, DateTime current, int maxlookup = 5)
        {
            DateTime result = current.AddDays(1);
            while (!dict.ContainsKey(result) && maxlookup > 0)
            {
                result = result.AddDays(1);
                maxlookup--;
            }
            return maxlookup == 0 ? new DateTime() : result;
        }
    }
}
