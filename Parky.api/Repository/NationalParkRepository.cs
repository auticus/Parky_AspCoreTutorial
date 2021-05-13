using System.Collections.Generic;
using System.Linq;
using Parky.api.Data;
using Parky.api.Models;
using Parky.api.Models.DTOs;
using Parky.api.Repository.Interfaces;

namespace Parky.api.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;

        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(park => park.Name).ToList();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(park => park.Id == nationalParkId);
        }

        public bool NationalParkExists(string name)
        {
            var value = _db.NationalParks.Any(park => park.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            var value = _db.NationalParks.Any(park => park.Id == id);
            return value;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public bool Save() => _db.SaveChanges() >= 0;
    }
}
