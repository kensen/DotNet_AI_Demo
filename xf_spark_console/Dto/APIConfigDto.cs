using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xf_spark_console.Dto
{
    public class APIConfigDto
    {        
        public string APIName { get; set; }
        public string APPID { get; set; }
        public string APISecret { get; set; } 
        public string APIKey { get; set; }

        public List<ModelUrl> ModelUrls { get; set; }

    }

    public class ModelUrl
    {
        public string ModelName { get; set; }
        public string Url { get; set; }
    }
}
