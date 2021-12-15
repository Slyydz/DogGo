using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        public List<Walks> GetWalksByWalker(int id);
    }
}
