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
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public enum DifficultyType
        {
            Easy,
            Moderate,
            Difficult,
            Expert
        }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        [ForeignKey("Id")]
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
                NationalParkId = NationalParkId,
                NationalPark = NationalPark.ToDTO()
            };
        }
    }
}
