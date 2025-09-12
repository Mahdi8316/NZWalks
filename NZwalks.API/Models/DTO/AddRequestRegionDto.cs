using System.ComponentModel.DataAnnotations;

namespace NZwalks.API.Models.DTO
{
    public class AddRequestRegionDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(3)]
        [MinLength(3,ErrorMessage ="has to be min 3.")]
        public string Code { get; set; }

        public string? RegionImageUrl { get; set; }

    }
}
