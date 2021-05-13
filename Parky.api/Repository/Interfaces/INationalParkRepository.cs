using System.Collections.Generic;
using Parky.api.Models;
using Parky.api.Models.DTOs;

namespace Parky.api.Repository.Interfaces
{
    public interface INationalParkRepository
    {
        ICollection<NationalParkDTO> GetNationalParks();
        NationalParkDTO GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNationalPark(NationalParkDTO nationalPark);
        bool UpdateNationalPark(NationalParkDTO nationalPark);
        bool DeleteNationalPark(NationalParkDTO nationalPark);
        bool Save();
    }
}
