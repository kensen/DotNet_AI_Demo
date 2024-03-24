using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdcb.SparkDesk;
using xf_spark_console.Dto;

namespace xf_spark_console.SparkDesk
{
    public class SparkDeskApi
    {
        private readonly string _appId;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public SparkDeskApi(APIConfigDto apiConfigDto)
        {
            _appId = apiConfigDto.APPID;
            _apiKey = apiConfigDto.APIKey;
            _apiSecret= apiConfigDto.APISecret;
        }

        public async Task<string> Chat(string questionText,string modelVersion)
        {
            SparkDeskClient client = new SparkDeskClient(_appId, _apiKey, _apiSecret);
            var msg = "";
            switch (modelVersion)
            {
                case "xf_v1.5":
                    ChatResponse response = await client.ChatAsync(ModelVersion.V1_5, new ChatMessage[]
                    {
                        ChatMessage.FromUser("系统提示：你叫张三，一名5岁男孩，你在金色摇篮幼儿园上学，你的妈妈叫李四，是一名工程师"),
                        ChatMessage.FromUser($"{questionText}"), //$"你好小朋友，我是周老师，你在哪上学？"
                    });
                    msg = response.Text;
                    break;
                case "xf_v3.0":
                    StringBuilder sb = new();
                    TokensUsage usage = await client.ChatAsStreamAsync(ModelVersion.V3, new ChatMessage[]
                    {
                        ChatMessage.FromUser("1+1=?"),
                        ChatMessage.FromAssistant("1+1=3"),
                        ChatMessage.FromUser("不对啊，请再想想？")
                    }, s => sb.Append(s), uid: "zhoujie");

                    string realResponse = sb.ToString();
                    msg=realResponse;
                    break;
                case "xf_v3.5":

                    break;
            }

        

            return msg;
        }

        
        
    }
}
