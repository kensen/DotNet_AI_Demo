using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;

namespace SemanticKernel_Demo.AIRegister
{
    public class AzureOpenAiRegister : AiProviderRegister
    {
        public override AiProviderType AiProviderType => AiProviderType.AzureOpenAI;

        protected override void RegisterChatCompletionService(IKernelBuilder builder, IServiceProvider provider, AiProvider aiProvider)
        {
            var chatModelId = aiProvider.GetChatCompletionApiService()?.ModelId;
            if (string.IsNullOrWhiteSpace(chatModelId))
            {
                return;
            }

            // Azure OpenAI ��Ҫ endpoint �� deployment ���ƣ�ͨ���� ModelId ��Ϊ deployment ���ƣ�
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: chatModelId,
                endpoint: aiProvider.ApiEndpoint,
                apiKey: aiProvider.ApiKey
            );
        }

        [Experimental("SKEXP0010")]
        protected override void RegisterEmbeddingService(IKernelBuilder builder, IServiceProvider provider, AiProvider aiProvider)
        {
            var embeddingModelId = aiProvider.GetEmbeddingApiService()?.ModelId;
            if (string.IsNullOrWhiteSpace(embeddingModelId))
            {
                return;
            }

            builder.AddAzureOpenAIEmbeddingGenerator(
                deploymentName: embeddingModelId,
                endpoint: aiProvider.ApiEndpoint,
                apiKey: aiProvider.ApiKey
            );
        }
    }
}
