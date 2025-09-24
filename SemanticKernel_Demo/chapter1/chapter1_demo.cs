using Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;
using Microsoft.SemanticKernel.TextGeneration;
using SemanticKernel_Demo.AIRegister;
using SemanticKernel_Demo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace SemanticKernel_Demo.chapter1
{
    //第一章节 大模型快速调用 聊天生成
    public class chapter1_demo
    {
        public APIconfigDto apiConfig = new APIconfigDto();
        public Kernel kernel;
        public ChatHistory chatHistory = new ChatHistory();
        public IChatCompletionService chatCompletionService;
        private readonly ILogger<chapter1_demo> _logger;

        public chapter1_demo() {

            var oneApiEndpoint = new Uri(apiConfig.APIUrl);
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(
                modelId: apiConfig.Models.FirstOrDefault().ModelId,
                apiKey: apiConfig.APIKey,
                endpoint: oneApiEndpoint
                );
            kernel = builder.Build();

            //添加系统提示词
            //chatHistory.AddSystemMessage("你是一个AI助手，帮助用户解决问题。");

            //获取APIconfigDto 并打印          
            Console.WriteLine(apiConfig.ToString());
            Console.WriteLine(apiConfig.APIKey);
            Console.WriteLine(apiConfig.APIUrl);
            Console.WriteLine(apiConfig.Models.FirstOrDefault().ModelId);

            Console.WriteLine("请输入字符（输入 'Q' 退出）：");


        }

        //chapter1_demo 构造函数使用 AIProviderRegisterFactory 创建 AIProviderRegister 实例,初始化Kernel对象
        public chapter1_demo(string aiProviderCode)
        {
            // 初始化依赖注入容器
            var services = new ServiceCollection();

            // 注册全局日志服务
            services.AddNLogging();

            // 注册Kernel服务
            services.RegisterKernels();
           
            // 构建ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            kernel= serviceProvider.GetKeyedService<Kernel>(aiProviderCode);

            var aiProvider = KernelAiProviderMap.CodeToProvider[aiProviderCode];

             chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            Console.WriteLine($"Endpoint:{aiProvider.ApiEndpoint}");
            Console.WriteLine($"Name:{aiProvider.Name}");
            Console.WriteLine($"ModelId:{aiProvider.GetChatCompletionApiService()?.ModelId}");            
            
            Console.WriteLine("请输入字符（输入 'Q' 退出）：");

            var response = kernel.InvokePromptAsync("一句话简单介绍ML.NET。");

            _logger = kernel.GetRequiredService<ILogger<chapter1_demo>>();
            _logger.LogInformation("测试日志写入");
        }


        //ITextGenerationService 实现
        public async Task Run(string chatStr)
        {
            try
            {
                // get text generation service
                var textGenerationService = kernel.Services.GetRequiredService<ITextGenerationService>();

                var text = await textGenerationService.GetTextContentAsync(chatStr);

                Console.WriteLine(text);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         
        }

        //IChatCompletionService 实现
        public async Task RunChat(string chatStr)
        {
            try
            {
                // get chat completion service
               // var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();
                //添加用户消息到聊天记录
                chatHistory.AddUserMessage(chatStr);

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                chatHistory,
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.Default
                });
                _logger.LogInformation("ChatHistory: {ChatHistory}", json);

                //获取聊天记录
                var chat = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
                Console.WriteLine($"AI:{chat.Content}");

                // 记录AI返回内容
                _logger.LogInformation("AI返回内容: {Content}", chat.Content);

                //添加助手消息到聊天记录
                chatHistory.AddAssistantMessage(chat.Content);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //IChatCompletionService 实现 流式输出
        public async Task RunChatStream(string chatStr)
        {
            try
            {
                // get chat completion service
               // var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
                //添加用户消息到聊天记录
                chatHistory.AddUserMessage(chatStr);

                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                //  chatHistory,
                //  new Newtonsoft.Json.JsonSerializerSettings
                //  {
                //      StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.Default
                //  });
                //    _logger.LogInformation("ChatHistory: {ChatHistory}", json);


                //获取聊天记录
                var chatResult = chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory);
                string response = "";
            
                await foreach (var chunk in chatResult)
                {
                   // if (chunk.Role.HasValue) Console.Write(chunk.Role + ": ");
                    response += chunk;
                    await Task.Delay(100);
                    Console.Write(chunk);
                   
                }
                chatHistory.AddAssistantMessage(response);

                _logger.LogInformation("AI返回内容: {Content}", response);

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
