﻿using System.ComponentModel.DataAnnotations;

namespace Parky.api.Models.DTOs
{
    public class TrailUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public Trail.DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public Trail ToTrail()
        {
            return new Trail()
            {
                Id = Id,
                Name = Name,
                Distance = Distance,
                Difficulty = Difficulty,
                NationalParkId = NationalParkId
            };
        }
    }
}
