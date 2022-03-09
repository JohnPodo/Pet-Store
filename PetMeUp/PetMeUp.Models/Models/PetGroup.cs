using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Models.Models
{
    public class PetGroup
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PetFamily Family { get; set; }
        public Pic? Pic { get; set; }

        public virtual ICollection<PetSpecie> Species { get; set; }
    }
}
