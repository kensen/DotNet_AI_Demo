using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

using xf_spark_console.Dto;

namespace xf_spark_console
{
    public class ConfigRead
    {
        public static APIConfigDto GetApiConfigDto()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("APIConfig.json").Build();
            var apiConfig=new APIConfigDto();
            configuration.Bind(apiConfig);
            return apiConfig;
        }
    }
}
