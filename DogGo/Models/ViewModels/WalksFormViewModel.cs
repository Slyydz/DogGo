using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksFormViewModel
    {
        public Walks Walk { get; set;}
        public List<int> SelectedDogs { get; set;}
        public MultiSelectList DogsSelect { get; set; }
    }
}
