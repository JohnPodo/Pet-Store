using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Models.Models
{
    public class Pet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Vat { get; set; }

        [NotMapped]
        public decimal TotalPrice { get => Price*Vat /100; }

        public int Quantity { get; set; }

        public PetSpecie? Specie { get; set; }

        public virtual ICollection<Pic> Pics { get; set; }
    }
}
