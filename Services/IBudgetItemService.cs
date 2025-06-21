using System.Threading.Tasks;
using budget_app.Data.DataTransferObjects;

namespace budget_app.Services
{
    public interface IBudgetItemService
    {
        Task PromptIsCompletedWhenDateArrives(int currentUserId);
    }
}