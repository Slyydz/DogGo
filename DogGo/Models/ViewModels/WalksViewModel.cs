using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksViewModel
    {
        public List<Walks> Walks { get; set;}

        public List<int> CheckedWalks { get; set;}
    }
}
