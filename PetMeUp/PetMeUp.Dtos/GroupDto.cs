using PetMeUp.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Dtos
{
    public class GroupDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Family is required")]
        [Range(1, int.MaxValue, ErrorMessage = "No Family with that Id")]
        public int FamilyId { get; set; }
        public PicDto? Pic { get; set; }
    }
}
