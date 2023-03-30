using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly DataContext _context;

        public TodosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo()
        {
          if (_context.Todo == null)
          {
              return NotFound();
          }
            return await _context.Todo.ToListAsync();
        }

        // GET: api/Todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(long id)
        {
          if (_context.Todo == null)
          {
              return NotFound();
          }
            var todo = await _context.Todo.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // PUT: api/Todos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(long id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Todos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
          if (_context.Todo == null)
          {
              return Problem("Entity set 'DataContext.Todo'  is null.");
          }
            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(long id)
        {
            if (_context.Todo == null)
            {
                return NotFound();
            }
            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(long id)
        {
            return (_context.Todo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
