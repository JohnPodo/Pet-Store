using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetMeUp.Models.Models
{
    public class PetFamily
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Pic? Pic { get; set; }
          
    }
}
