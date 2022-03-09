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
    public class PicRepo : SuperRepo
    {
        public PicRepo(string conString, DatabaseType dbtype) : base(conString, dbtype)
        {
        }

        public async Task<bool> AddPic(Pic model)
        {
            await _db.Pics.AddAsync(model); 
            return true;
        }

        public async Task<bool> DeletePic(int id)
        {
            var picToDel = await _db.Pics.Where(x=>x.Id == id).FirstOrDefaultAsync();
            if(picToDel != null)
            {
                _db.Pics.Remove(picToDel); 
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePic(int id, Pic model)
        {
            var picToUpdate = await _db.Pics.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (picToUpdate != null)
            {
                picToUpdate.Name = model.Name;
                picToUpdate.Content = model.Content; 
                return true;
            }
            return false;
        }

        public async Task<Pic> GetPic(int id) => await _db.Pics.Where(_x => _x.Id == id).FirstOrDefaultAsync();
        public async Task<List<Pic>> GetPics() => await _db.Pics.ToListAsync();
    }
}
