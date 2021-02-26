using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class CommanderContext : DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> opt) : base(opt)
        {
            // base keyword just calls the constructor of the base level of commanderContext which is DbContext
        }

        // create representation of command model in our database
        // use DB set in EF
        public DbSet<Command> Commands {get; set;}  //this is a property
    }
}