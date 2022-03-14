using PetMeUp.DAL;
using PetMeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Repos
{
    public abstract class SuperRepo : IDisposable
    {
        protected readonly PetContext _db;

        public SuperRepo(string conString,DatabaseType dbtype)
        {
            _db = new PetContext(conString, dbtype);
        }
         

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
