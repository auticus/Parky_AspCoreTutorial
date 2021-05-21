using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Parky.web.Models;

namespace Parky.web.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            
        }
    }
}
