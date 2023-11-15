using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.InfraStructure.Response
{
    public class ResponseFreteCalculate
    {
        public int id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string custom_price { get; set; }
        public string discount { get; set; }
        public string currency { get; set; }
        public int delivery_time { get; set; }
        public DeliveryRange delivery_range { get; set; }
        public int custom_delivery_time { get; set; }
        public DeliveryRange custom_delivery_range { get; set; }
        public List<Package> packages { get; set; }
        public bool colorida { get; set; }
        public class DeliveryRange
        {
            public int min { get; set; }
            public int max { get; set; }
        }

        public class Package
        {
            public string price { get; set; }
            public string discount { get; set; }
            public string format { get; set; }
            public Dimension dimensions { get; set; }
            public string weight { get; set; }
            public string insurance_value { get; set; }
        }

        public class Dimension
        {
            public int height { get; set; }
            public int width { get; set; }
            public int length { get; set; }
        }
    }

}
