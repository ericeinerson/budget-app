using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget_app.Data.Models;

namespace budget_app.Pages
{
 
public partial class AddItemType
    {
        public ItemType? ItemType { get; set; }
        private bool IsBusy { get; set; }
        private string? SuccessMessage { get; set; }
        private string? ErrorMessage { get; set; }
        protected override void OnInitialized()
        {
            using var context = ContextFactory.CreateDbContext();

            ItemType = new()
            {
                Name = string.Empty
            };
        }

        private async Task HandleSubmit(bool isValid)
        {
            if(isValid)
            {
                await HandleValidSubmit();
            }
            else
            {
                HandleInvalidSubmit();
            }
        }

        private async Task HandleValidSubmit()
        {
            if(IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                if(ItemType is not null)
                {
                    using var context = ContextFactory.CreateDbContext();

                    context.ItemTypes.Add(ItemType);

                    await context.SaveChangesAsync();

                    SuccessMessage = $"Item Type {ItemType.Name} was added successfully";
                    ErrorMessage = null;

                    ItemType = new()
                    {
                        Name = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                SuccessMessage = null;
                ErrorMessage = $"Error while adding Item Type: {ex.Message}.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void HandleInvalidSubmit()
        {
            SuccessMessage = null;
            ErrorMessage = null;
        }
    }
}
