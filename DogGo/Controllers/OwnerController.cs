using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Repositories;
using DogGo.Models;
using DogGo.Models.ViewModels;

namespace DogGo.Controllers
{
    public class OwnerController : Controller
    {

        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighRepo;

        public OwnerController(IOwnerRepository ownerRepo, IDogRepository dogRepo, IWalkerRepository walkerRepo, INeighborhoodRepository neighRepo)
        {
            _ownerRepo = ownerRepo;
            _dogRepo = dogRepo;
            _walkerRepo = walkerRepo;
            _neighRepo = neighRepo;
        }
        // GET: OwnerController
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: OwnerController/Details/ownerId
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepo.WalkersByNeighborhood(owner.NeighborhoodId);

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };

            return View(vm);
        }

        // GET: OwnerController/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighs = _neighRepo.GetAll();

            OwnerFormViewModel vmOwner = new OwnerFormViewModel
            {
                Owner = new Owner(),
                Neighborhoods = neighs
            };

            return View(vmOwner);
        }

        // POST: OwnerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnerController/Edit/5
        public ActionResult Edit(int id)
        {
            List<Neighborhood> neighs = _neighRepo.GetAll();

            OwnerFormViewModel vmOwner = new OwnerFormViewModel
            {
                Owner = _ownerRepo.GetOwnerById(id),
                Neighborhoods = neighs
            };

            return View(vmOwner);
        }

        // POST: OwnerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnerController/Delete/5
        public ActionResult Delete(int id)
        {
           Owner owner = _ownerRepo.GetOwnerById(id);

            if(owner == null)
            {
                return StatusCode(404);
            }

            return View(owner);
        }

        // POST: OwnerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(owner);
            }
        }
    }
}
