using System.Collections.Generic;

namespace TarsOffice.DayFeatures.Abstractions
{
    public class Tag
    {
        public string Type { get; set; }
        public string InnerText { get; set; }
        public IDictionary<string, object> Properties { get; set; }

        public Tag()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}
