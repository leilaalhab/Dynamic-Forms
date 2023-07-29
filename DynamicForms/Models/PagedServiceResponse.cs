using System;
namespace DynamicForms.Models
{
	public class PagedServiceResponse<T> : ServiceResponse<T>
	{
		public new List<T>? Data { get; set; }
		public int TotalRecords { get; set; }

    }
}

