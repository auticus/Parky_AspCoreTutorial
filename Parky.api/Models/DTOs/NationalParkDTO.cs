using System;

namespace Parky.api.Models.DTOs
{
    public class NationalParkDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
