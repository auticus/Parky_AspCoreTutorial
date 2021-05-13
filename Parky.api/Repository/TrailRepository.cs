using System.Collections.Generic;
using Parky.api.Data;
using Parky.api.Models;
using Parky.api.Repository.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Parky.api.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<Trail> GetTrails()
        {
            //this uses the Entity Framework Include method, which will look for related models (the national park) and include them as well
            return _db.Trails.Include(p => p.NationalPark).OrderBy(trail => trail.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInPark(int id)
        {
            //this uses the Entity Framework Include method, which will look for related models (the national park) and include them as well
            return _db.Trails.Include(p=>p.NationalPark).Where(trail => trail.NationalParkId == id).ToList();
        }

        public Trail GetTrail(int id)
        {
            //this uses the Entity Framework Include method, which will look for related models (the national park) and include them as well
            return _db.Trails.Include(p => p.NationalPark).FirstOrDefault(trail => trail.Id == id);
        }

        public bool TrailExists(string name)
        {
            var value = _db.Trails.Any(trail => trail.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            var value = _db.Trails.Any(trail => trail.Id == id);
            return value;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public bool Save() => _db.SaveChanges() >= 0;
    }
}
