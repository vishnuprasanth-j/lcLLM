using EnvDTE;
using lcLLM.Models;
using System.Threading.Tasks;

namespace lcLLM.Clients.Interfaces
{
    internal interface ILlmClient
    {
        void SetBaseUrl(string baseUrl);
        string GetBaseUrl();
        void SetTimeOut(TimeSpan timeOut);
        TimeSpan GetTimeOut();
        Task<string[]> GetModelListAsync();
        Task<CodeSuggestionResponse> GetCodeSuggestionsAsync(CodeType codeType, string prompt);
    }
}
