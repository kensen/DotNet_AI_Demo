using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;


namespace SemanticKernel_Demo.AIRegister
{
    public static class AiProviderRegisterFactory
    {
        public static AiProviderRegister Create(AiProviderType aiProviderType)
        {
            return aiProviderType switch
            {
                AiProviderType.OpenAI=> new OpenAiRegister(),
                AiProviderType.OpenAI_Compatible => new OpenAiCompatibleAiRegister(),
                AiProviderType.AzureOpenAI => new AzureOpenAiRegister(),
                _ => throw new NotSupportedException($"The AI provider type '{aiProviderType}' is not supported.")
            };
        }

        public static IServiceCollection RegisterKernels(this IServiceCollection services)
        {
            // 从配置文件中加载AI配置
            var aiOptions = AiSettings.LoadAiProvidersFromFile();
            // 注册其他AI服务提供商
            foreach (var aiProvider in aiOptions.Providers)
            {
                var providerRegister = AiProviderRegisterFactory.Create(aiProvider!.AiType);

                providerRegister.Register(services, aiProvider);
            }
            return services;
        }
    }


    public static class KernelAiProviderMap
    {
        public static readonly Dictionary<string, AiProvider> CodeToProvider = new();
    }

}
