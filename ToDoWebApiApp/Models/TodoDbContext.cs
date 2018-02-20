using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Data.Entity;

namespace ToDoWebApiApp.Models
{
	public class TodoDbContext : DbContext
	{
		public TodoDbContext(DbContextOptions<TodoDbContext> options)
				: base(options)
		{
			//this.Database..Configuration.LazyLoadingEnabled = false;
		}

		public DbSet<TodoTask> TodoTasks { get; set; }
		public DbSet<TodoItem> TodoItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TodoItem>()
				.HasOne(g => g.TodoTask)
				.WithMany(g => g.Items)
				.IsRequired();
			//modelBuilder..Conventions.Remove<PluralizingTableNameConvention>();
			//
			modelBuilder.Entity<TodoTask>()
				.HasMany(g => g.Items)
				.WithOne(g => g.TodoTask)
				.IsRequired();    // One-to-Many
													//.WithRequired(x => x.TodoTask);  
													//ImportColumn must has a father: ImportMapping
			base.OnModelCreating(modelBuilder);
		}


	}

}
