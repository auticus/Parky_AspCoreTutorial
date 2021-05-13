﻿using System;
using System.ComponentModel.DataAnnotations;
using Parky.api.Models.DTOs;

namespace Parky.api.Models
{
    public class NationalPark
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public DateTime Created { get; set; }
        public DateTime Established { get; set; }

        public NationalParkDTO ToDTO()
        {
            var park = new NationalParkDTO()
            {
                Id = Id,
                Name = Name,
                State = State,
                Created = Created,
                Established = Established
            };

            return park;
        }
    }
}
