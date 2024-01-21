using System;
using System.ComponentModel;
using System.Reflection;

namespace BudgetApp.Extensions
{
	public static class Extensions
	{
		public static string GetDescription(this Enum e)
		{
			
			var attribute =
				e.GetType()
					.GetTypeInfo()
					.GetMember(e.ToString())
					.First(member => member.MemberType == MemberTypes.Field)
					.GetCustomAttributes(typeof(DescriptionAttribute), false)
					.SingleOrDefault()
					as DescriptionAttribute;

			return attribute?.Description ?? e.ToString();
		}
	}
}

