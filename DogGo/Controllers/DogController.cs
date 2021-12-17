using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogGo.Controllers
{
    public class DogController : Controller
    {

        private IDogRepository _dogRepo;

        public DogController(IDogRepository dogrepo)
        {
            _dogRepo = dogrepo;
        }
        // GET: DogController

        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            

            return View(dog);
        }

        // GET: DogController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Edit/id
        [Authorize]
        public ActionResult Edit(int id)
        {
            int ownerId = GetCurrentUserId();

            Dog dog = _dogRepo.GetDogById(id);

            if(ownerId != dog.OwnerId)
            {
                return StatusCode(404);
            }
            else
            {
                return View(dog);
            }
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Dog dog)
        {
            try
            {
                int ownerId = GetCurrentUserId();

                if(ownerId == dog.OwnerId)
                {
                    _dogRepo.UpdateDog(dog);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();

            if(dog.OwnerId != ownerId)
            {
                return StatusCode(404);
            }
            else
            {
                return View(dog);
            }
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(Dog dog)
        {
            try
            {
                int ownerId = GetCurrentUserId();
                if (dog.OwnerId == ownerId)
                {
                    _dogRepo.DeleteDog(dog.Id);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
