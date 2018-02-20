using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ToDoWebApiApp.Models
{
	public class TodoTask
	{
		[Key]
		public int TodoTaskId { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; }

		public bool IsComplete { get; set; }

		public virtual ICollection<TodoItem> Items { get; set; }
	}
}
