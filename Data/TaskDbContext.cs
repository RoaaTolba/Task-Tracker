using Microsoft.EntityFrameworkCore;
using TaskTrackerMiniAPI.Models;
namespace TaskTrackerMiniAPI.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base ( options )
        {
        }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
    }
}
