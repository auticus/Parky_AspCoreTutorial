using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Parky.api.Models.DTOs
{
    public class TrailDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public Trail.DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalParkDTO NationalPark { get; set; }

        public Trail ToTrail()
        {
            return new Trail()
            {
                Id = Id,
                Name = Name,
                Distance = Distance,
                Difficulty = Difficulty,
                NationalParkId = NationalParkId,
                NationalPark = NationalPark.ToNationalPark()
            };
        }
    }
}
