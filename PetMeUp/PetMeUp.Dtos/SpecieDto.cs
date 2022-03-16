using PetMeUp.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Dtos
{
    public class SpecieDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Group is required")]
        [Range(1, int.MaxValue, ErrorMessage = "No Family with that Id")]
        public int GroupId { get; set; }
        public PicDto? Pic { get; set; }
        public double? MaximumFemaleHeight { get; set; }
        public double? MaximumMaleWeight { get; set; }
        public double? MinimumFemaleHeight { get; set; }
        public double? MinimumMaleWeight { get; set; }

        public string? LifeSpan { get; set; }
        public string? Origin { get; set; }

    }
}
