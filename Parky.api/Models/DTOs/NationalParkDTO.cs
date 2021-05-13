using System;
using System.ComponentModel.DataAnnotations;

namespace Parky.api.Models.DTOs
{
    public class NationalParkDTO
    {
        public int Id { get; set; }
        
        [Required] //receiving DTOs from users, need the tags here for validate methods to determine if the model is valid
        public string Name { get; set; }
        
        [Required]
        public string State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }

        public NationalPark ToNationalPark()
        {
            var park = new NationalPark
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
