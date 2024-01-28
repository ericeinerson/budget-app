using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum Rate
	{
        [Description("Weekly")]
        Weekly = 1,
        [Description("Biweekly")]
        Biweekly,
        [Description("Monthly")]
        Monthly,
        [Description("Yearly")]
        Yearly,
		[Description("No Rate")]
		NoRate,
        [Description("Other")]
        Other
	}
}

