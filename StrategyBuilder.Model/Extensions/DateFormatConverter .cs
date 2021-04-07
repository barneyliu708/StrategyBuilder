using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Model.Extensions
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
