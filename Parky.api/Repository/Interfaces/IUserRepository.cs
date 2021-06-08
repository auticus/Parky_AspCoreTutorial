using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parky.api.Models;

namespace Parky.api.Repository.Interfaces
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string userName);
        User Authenticate(string userName, string password);
        User Register(string userName, string password);
    }
}
