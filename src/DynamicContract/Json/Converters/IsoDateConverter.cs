using Newtonsoft.Json.Converters;

namespace DynamicContract.Json.Converters
{
    public class IsoDateConverter : IsoDateTimeConverter
    {
        public IsoDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
