using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {

        private IWalksRepository _walksRepo;
        private IDogRepository _dogRepo;

        public WalksController(IWalksRepository walksRepo, IDogRepository dogRepo)
        {
            _walksRepo = walksRepo;
            _dogRepo = dogRepo;
        }
        // GET: WalksController
        public ActionResult Index()
        {
            WalksViewModel vm = new WalksViewModel
            {
                Walks = _walksRepo.GetWalks(),
                CheckedWalks = new List<int>()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteChecked(WalksViewModel vm)
        {
            _walksRepo.DeleteWalks(vm.CheckedWalks);

            return RedirectToAction("Index");
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create()
        {
            WalksFormViewModel vm = new WalksFormViewModel
            {
                Walk = new Walks(),
                DogsSelect = new MultiSelectList(_dogRepo.GetAllDogs(), "Id", "Name")
            };
            

            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalksFormViewModel vm)
        {
            try
            {
                foreach (int i in vm.SelectedDogs)
                {
                    _walksRepo.AddWalk(vm.Walk, i);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
