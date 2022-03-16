using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Models.Models
{
    public class PetSpecie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string? Origin { get; set; }
        public string? Life_Span { get; set; }
        
        public double? MinimumMaleWeight { get; set; }
        public double? MaximumMaleWeight { get; set; }
        public double? MinimumFemaleHeight { get; set; }
        public double? MaximumFemaleHeight { get; set; }
        public PetGroup? Group { get; set; } 
        public Pic? Pic { get; set; }
        public ICollection<Pet> Pets { get; set; }
    }
}
