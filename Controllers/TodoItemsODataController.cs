using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    public class TodoItemsODataController : ODataController
    {
        private readonly TodoContext _context;

        public TodoItemsODataController(TodoContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public async Task<IEnumerable<TodoItem>> Get()
        {
            return await _context.TodoItems.ToListAsync();
        }

        [EnableQuery]
        
        public SingleResult<TodoItem> Get([FromODataUri] int key)
        {
            var result = _context.TodoItems.Where(p => p.Id == key);

            return SingleResult.Create(result);
        }
    }
}