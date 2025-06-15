using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget_app.Data.Models;

namespace budget_app.Pages
{
 
public partial class AddCategory
    {
        public Category? Category { get; set; }
        private bool IsBusy { get; set; }
        private string? SuccessMessage { get; set; }
        private string? ErrorMessage { get; set; }
        protected override void OnInitialized()
        {
            using var context = ContextFactory.CreateDbContext();

            Category = new()
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
                if(Category is not null)
                {
                    using var context = ContextFactory.CreateDbContext();

                    Category.CreatedDate = DateTime.Now;
                    
                    context.Categories.Add(Category);

                    await context.SaveChangesAsync();

                    SuccessMessage = $"Category {Category.Name} was added successfully";
                    ErrorMessage = null;

                    Category = new()
                    {
                        Name = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                SuccessMessage = null;
                ErrorMessage = $"Error while adding Category: {ex.Message}.";
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
