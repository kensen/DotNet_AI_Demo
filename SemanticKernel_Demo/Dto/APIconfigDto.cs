using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SemanticKernel_Demo.Dto
{
    public class APIconfigDto
    {
        public string APIUrl { get; set; }
        public string APIKey { get; set; }

        public List<ModelDto> Models { get; set; }    
        
        //构造函数，读取 APIConfig.Json 文件
        public APIconfigDto()
        {
            //var json = System.IO.File.ReadAllText("APIConfig.json");
            //var config = Newtonsoft.Json.JsonConvert.DeserializeObject<APIconfigDto>(json);
            //APIUrl = config.APIUrl;
            //APIKey = config.APIKey;
            //Models = config.Models;

            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("APIConfig.json").Build();
           
            configuration.Bind(this);
           // return apiConfig;
           //return this
        
        }


    }
    public class ModelDto
    {
        public string ModelId { get; set; }
    }
}
