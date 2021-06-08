using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Parky.api.Models.DTOs;

namespace Parky.api.Models
{
    public class NationalPark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
        public byte[] Picture { get; set; }

        public NationalParkDTO ToDTO()
        {
            //developer note: yes we can use automapper for this - but i am not fond of 3rd party software reliance - especially for something like mapping which is
            //trivial. You are free to do what you wish
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
