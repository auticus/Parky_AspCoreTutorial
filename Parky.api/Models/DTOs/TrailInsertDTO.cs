using System.ComponentModel.DataAnnotations;

namespace Parky.api.Models.DTOs
{
    public class TrailInsertDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Elevation { get; set; }

        public Trail.DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public Trail ToTrail()
        {
            return new Trail()
            {
                Name = Name,
                Distance = Distance,
                Elevation = Elevation,
                Difficulty = Difficulty,
                NationalParkId = NationalParkId
            };
        }
    }
}
