using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;

namespace budget_app.Shared
{
    public partial class ItemTypeForm
    {
        [Parameter]
        public ItemType? ItemType { get; set; }

        [Parameter]
        public bool IsBusy { get; set; }

        [Parameter]
        public bool IsEdit { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }
        
        [Parameter]
        public EventCallback<bool> OnSubmit { get; set; }

        private async Task HandleValidSubmit()
        {
            if(OnSubmit.HasDelegate)
            {
                await OnSubmit.InvokeAsync(true);
            }
        }

        private async Task HandleInvalidSubmit()
        {
            if(OnSubmit.HasDelegate)
            {
                await OnSubmit.InvokeAsync(false);
            }
        }

        private async Task HandleCancel()
        {
            if(OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync();
            }
        }
    }
}