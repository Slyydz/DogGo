using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        public List<Owner> GetAllOwners();

        public Owner GetOwnerById(int id);
    }
}
