using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoWebApiApp.Models;

namespace ToDoWebApiApp.Controllers
{
	[Produces("application/json")]
	[Route("api/task")]
	public class TodoTaskController : Controller
	{
		private readonly TodoDbContext _context;

		public TodoTaskController(TodoDbContext context)
		{
			_context = context;

			//if (_context.TodoItems.Count() == 0)
			//{
			//	_context.TodoItems.Add(new TodoItem { Name = "Item1" });
			//	_context.SaveChanges();
			//}
		}

		//GET /api/todo
		[HttpGet]
		public IEnumerable<TodoTask> GetAll()
		{
			return _context.TodoTasks.ToList();
		}

		//GET /api/task/{id}
		[HttpGet("{id}", Name = "get")]
		public IActionResult GetById(int id)
		{
			var task = _context.TodoTasks.FirstOrDefault(t => t.TodoTaskId == id);
			if (task == null)
			{
				return NotFound();
			}
			return new ObjectResult(task);
		}

		[HttpPost]
		public IActionResult Create([FromBody] TodoTask task)
		{
			if (task == null)
			{
				return BadRequest();
			}

			_context.TodoTasks.Add(task);
			_context.SaveChanges();

			return CreatedAtRoute("get", new { id = task.TodoTaskId }, task);
		}

		[HttpPost]
		public IActionResult CreateTaskItem([FromBody] int taskId, [FromBody] TodoItem item)
		{
			var todo = _context.TodoTasks.FirstOrDefault(t => t.TodoTaskId == taskId);
			if (item == null || todo == null)
			{
				return BadRequest();
			}
			todo.Items.Add(item);
			_context.SaveChanges();

			return CreatedAtRoute("get", new { id = item.TotoItemId }, item);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, [FromBody] TodoTask task)
		{
			if (task == null || task.TodoTaskId != id)
			{
				return BadRequest();
			}

			var todo = _context.TodoTasks.FirstOrDefault(t => t.TodoTaskId == id);
			if (todo == null)
			{
				return NotFound();
			}

			todo.IsComplete = task.IsComplete;
			todo.Name = task.Name;

			//_context.TodoItems.Attach(todo); //.Update(todo);
			_context.SaveChanges();
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var todo = _context.TodoTasks.FirstOrDefault(t => t.TodoTaskId == id);
			if (todo == null)
			{
				return NotFound();
			}

			_context.TodoTasks.Remove(todo);
			_context.SaveChanges();
			return new NoContentResult();
		}

		/*
		// GET: api/Todo
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET: api/Todo/5
		[HttpGet("{id}", Name = "Get")]
		public string Get(int id)
		{
			return "value";
		}

		// POST: api/Todo
		[HttpPost]
		public void Post([FromBody]string value)
		{
		}

		// PUT: api/Todo/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
		*/
	}
}
