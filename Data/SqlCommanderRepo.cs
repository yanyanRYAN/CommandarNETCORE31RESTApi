using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;

namespace Commander.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext _context;

        // make use of dbcontext to return command items from database
        public SqlCommanderRepo(CommanderContext context)
        {
            //get implementation of dbcontext which is CommanderContext 
            _context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Commands.Add(cmd); // add the command object to the commands DBSET so itll be tracked before it is saved via SaveChanges method on the context
            
        }

        public void DeleteCommand(Command cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Commands.Remove(cmd);

        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges() >= 0 );  // if you are making changes to data via datacontext, will need to call this method in order to save changes on the DB
        }

        public void UpdateCommand(Command command)
        {
            // do nothing  in this instance its taken care of by the dbcontext
            
        }
    }
}