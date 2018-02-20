using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebApiApp.Models;

namespace ToDoWebApiApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly TodoDbContext ctx;
		private List<string> queries = new List<string> { "uncompleted", "completed" };
		public HomeController(TodoDbContext context)
		{
			ctx = context;
		}

		// GET: Home
		public async Task<IActionResult> Index([FromQuery] string filter)
		{
			var q = ctx.TodoTasks
				.Include(t => t.Items)
				.AsQueryable();
			int ndx = -1;
			if (!String.IsNullOrWhiteSpace(filter) &&
				(ndx = queries.IndexOf(filter = filter.ToLower())) >= 0)
			{
				var completed = ndx == 1;
				q = q.Where(t => t.IsComplete == completed);
			}
			//return View(await ctx.TodoTasks.Include(t => t.Items).ToListAsync());
			return View(await q.ToListAsync());
		}

		async Task<TodoTask> getTask(int? id, bool loadItems = false)
		{
			return loadItems ?
				await ctx.TodoTasks
				.Include(t => t.Items)
				.SingleOrDefaultAsync(m => m.TodoTaskId == id) :
				await ctx.TodoTasks
				.SingleOrDefaultAsync(m => m.TodoTaskId == id);
		}

		// GET: Home/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			var todoTask = await getTask(id, true);
			if (todoTask == null)
			{
				return NotFound();
			}
			return View(todoTask);
		}

		public async Task<IActionResult> Complete(int? id, [FromQuery] int itemid)
		{
			var task = await getTask(id, true);
			TodoItem item = null;

			if (task != null &&
				(item = task.Items.FirstOrDefault(i => i.TotoItemId == itemid)) != null)
			{
				item.IsComplete = true;
				//check if all task items were completed
				if (task.Items.All(t => t.IsComplete))
				{
					task.IsComplete = true;
				}
				await ctx.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Details), new { id = id });
		}

		// GET: Home/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Test/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("TodoTaskId,Name,IsComplete")] TodoTask todoTask)
		{
			if (ModelState.IsValid)
			{
				ctx.Add(todoTask);
				await ctx.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(todoTask);
		}

		// GET: Home/Create
		public async Task<IActionResult> CreateItem(int? id)
		{
			var todoTask = await getTask(id);
			if (todoTask == null)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(new TodoItem() { TodoTask = todoTask });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateItem(
			[Bind("TotoItemId,Name,IsComplete")] TodoItem todoItem, int TodoTaskId)
		{
			if (ModelState.IsValid)
			{
				var task = await getTask(TodoTaskId, true);
				if (task == null)
				{
					ModelState.AddModelError("TodoTaskId", "Invalid ToDo Task");
					//return NotFound();
				}
				else
				{
					task.Items.Add(todoItem);
					//open again
					task.IsComplete = false;
					await ctx.SaveChangesAsync(true);
					return RedirectToAction(nameof(Details), new { id = TodoTaskId });
				}
			}
			return View(todoItem);
		}

		// GET: Home/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			var task = await getTask(id, true);
			if (task == null || task.IsComplete)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(task);
		}

		// POST: Home/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id,
			[Bind("TodoTaskId,Name,IsComplete")] TodoTask task)
		{
			if (id != task.TodoTaskId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					ctx.Update(task);
					await ctx.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (getTask(task.TodoTaskId) == null)
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(task);
		}


		// GET: Home/Edit/5
		public async Task<IActionResult> EditItem(int? id, [FromQuery] int taskid)
		{
			var item = await ctx.TodoItems
				.Include(t => t.TodoTask)
				.FirstOrDefaultAsync(i => i.TotoItemId == id);
			if (item == null ||
				item.TodoTask.TodoTaskId != taskid ||
				item.TodoTask.IsComplete ||
				item.IsComplete)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(item);
		}

		// POST: Home/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditItem([Bind("TotoItemId,Name,IsComplete")] TodoItem todoItem,
			int TodoTaskId)
		{
			var item = await ctx.TodoItems
				.Include(t => t.TodoTask)
				.FirstOrDefaultAsync(i => i.TotoItemId == todoItem.TotoItemId);

			if (item == null || item.TodoTask.TodoTaskId != TodoTaskId)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try
				{
					item.Name = todoItem.Name;
					ctx.Update(item);
					await ctx.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
				return RedirectToAction(nameof(Details), new { id = TodoTaskId });
			}
			return View(todoItem);
		}


		// GET: Home/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			var task = await getTask(id, true);
			if (task == null)
			{
				return NotFound();
			}
			return View(task);
		}

		// POST: Home/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var task = await getTask(id, true);

			ctx.TodoTasks.Remove(task);
			await ctx.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Home/Delete/5
		public async Task<IActionResult> DeleteItem(int? id, [FromQuery] int taskid)
		{
			var item = await ctx.TodoItems
				.Include(t => t.TodoTask)
				.FirstOrDefaultAsync(i => i.TotoItemId == id);

			if (item == null ||
				item.TodoTask.TodoTaskId != taskid ||
				item.TodoTask.IsComplete ||
				item.IsComplete)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(item);
		}

		// POST: Home/Delete/5
		[HttpPost, ActionName("DeleteItem")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmedItem(int id, int TodoTaskId)
		{
			var item = await ctx.TodoItems
				.Include(t => t.TodoTask)
				.FirstOrDefaultAsync(i => i.TotoItemId == id);

			var task = await getTask(item.TodoTask.TodoTaskId, true);
			task.Items.Remove(item);
			if (task.Items.All(t => t.IsComplete))
			{
				task.IsComplete = true;
			}
			await ctx.SaveChangesAsync();
			return RedirectToAction(nameof(Details), new { id = TodoTaskId });
		}

	}
}