using System.IO;
using Microsoft.Extensions.Configuration;

namespace SemanticKernel_Demo.AIRegister
{


    public static class AiSettings
    {
        public static AiOptions LoadAiProvidersFromFile()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("APIConfig.json", optional: true, reloadOnChange: true)
                .Build();

            var aiOptions = configuration.GetSection("AiOptions").Get<AiOptions>();
            return aiOptions;
        }
    }
}