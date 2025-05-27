using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;

namespace SemanticKernel_Demo.AIRegister
{
    /// <summary>
    /// AiProvider 注册器基类
    /// </summary>
    public abstract class AiProviderRegister
    {
        public abstract AiProviderType AiProviderType { get; }

        public virtual void Register(IServiceCollection services, AiProvider aiProvider)
        {
            // 为指定AiProvider 注册专用Kernel服务
            services.AddKeyedTransient<Kernel>(aiProvider.Code, (sp, key) => BuildKernel(sp, aiProvider));            
            // 建立映射
            KernelAiProviderMap.CodeToProvider[aiProvider.Code] = aiProvider;            


        }

        public virtual Kernel BuildKernel(IServiceProvider serviceProvider, AiProvider aiProvider)
        {
            // 创建Kernel构建器
            var builder = Kernel.CreateBuilder();

            RegisterChatCompletionService(builder, serviceProvider, aiProvider);
            RegisterEmbeddingService(builder, serviceProvider, aiProvider);

            // Register other services if needed

            return builder.Build();
        }

        protected abstract void RegisterChatCompletionService(IKernelBuilder builder, IServiceProvider provider, AiProvider aiProvider);

        protected abstract void RegisterEmbeddingService(IKernelBuilder builder, IServiceProvider provider, AiProvider aiProvider);

    }
}
