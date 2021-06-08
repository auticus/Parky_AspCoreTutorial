using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Parky.api.Models.DTOs;

namespace Parky.api.Models
{
    public class Trail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Elevation { get; set; }

        public enum DifficultyType
        {
            Easy,
            Moderate,
            Difficult,
            Expert
        }

        public DifficultyType Difficulty { get; set; }

        [Required]
        [ForeignKey("NationalPark")]
        public int NationalParkId { get; set; }

        //[ForeignKey("Id")]
        public NationalPark NationalPark { get; set; }

        public DateTime DateCreated { get; set; }

        public TrailDTO ToDTO()
        {
            return new TrailDTO()
            {
                Id = Id,
                Name = Name,
                Distance = Distance,
                Difficulty = Difficulty,
                Elevation = Elevation,
                NationalParkId = NationalParkId,
                NationalPark = NationalPark.ToDTO()
            };
        }
    }
}
