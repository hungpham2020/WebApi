using Microsoft.EntityFrameworkCore;

namespace WebApi.Model
{
    public class TodoDbContext: DbContext
    {
       public TodoDbContext(DbContextOptions<TodoDbContext> options): base(options)
        {

        }
        
    }
}
