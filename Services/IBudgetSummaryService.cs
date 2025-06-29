using System.Threading.Tasks;
using budget_app.Data.DataTransferObjects;

namespace budget_app.Services
{
    public interface IBudgetSummaryService
    {
        Task<BudgetSummaryCompiledDetails> GetCompiledDetails(int currentUserId);
    }
}