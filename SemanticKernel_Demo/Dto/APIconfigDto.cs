using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SemanticKernel_Demo.AIRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("APIConfig.json").Build();

            //configuration.Bind(this); // Updated to reflect new JSON structure

            //读取配置文件
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("APIConfig.json", optional: true, reloadOnChange: true)
                .Build();   
            
            configuration.GetSection("APIConfig").Bind(this); // 将配置绑定到当前对象



        }


    }
    public class ModelDto
    {
        public string ModelId { get; set; }
    }
}
