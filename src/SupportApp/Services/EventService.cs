using SupportApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Services
{
    public class EventService
    {
        private IGenericRepository _repo;

        public EventService(IGenericRepository repo)
        {
            this._repo = repo;
        }



    }
}
