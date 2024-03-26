using AppCommon.Enums;
using SqlyAI.Integrations.Dtos.OpenAI;

namespace SqlyAI.Integrations.Interfaces
{
    public interface IOpenAIService
    {
        Task<CompletionOutput> TextToSql(DbType dbType, string text, IEnumerable<TableInfo> tableInfoes);
    }
}