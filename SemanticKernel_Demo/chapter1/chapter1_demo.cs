using Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextGeneration;
using SemanticKernel_Demo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public chapter1_demo() {

            var oneApiEndpoint = new Uri(apiConfig.APIUrl);
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(
                modelId: apiConfig.Models.FirstOrDefault().ModelId,
                apiKey: apiConfig.APIKey,
                endpoint: oneApiEndpoint
                );
            kernel = builder.Build();
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
                var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();

                //添加用户消息到聊天记录
                chatHistory.AddUserMessage(chatStr);

                //获取聊天记录
                var chat = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
                Console.WriteLine($"AI:{chat.Content}");

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
                var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();
                //添加用户消息到聊天记录
                chatHistory.AddUserMessage(chatStr); 
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
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
