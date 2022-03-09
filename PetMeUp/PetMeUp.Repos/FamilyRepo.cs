using Microsoft.EntityFrameworkCore;
using PetMeUp.Models;
using PetMeUp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Repos
{
    public class FamilyRepo : SuperRepo
    {
        public FamilyRepo(string conString, DatabaseType dbtype) : base(conString, dbtype)
        {
        }  

        public async Task<PetFamily> GetFamily(int id) => await _db.Families.Where(_x => _x.Id == id).Include(s => s.Pic).FirstOrDefaultAsync();
        public async Task<List<PetFamily>> GetFamilies() => await _db.Families.Include(s=>s.Pic).ToListAsync();
    }
}
