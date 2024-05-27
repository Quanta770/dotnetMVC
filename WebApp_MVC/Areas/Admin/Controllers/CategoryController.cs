using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Net;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {   //dependency injection to provide ICategoryRepository
        //need to register service in dependency container in Program.cs
        //private readonly ICategoryRepository _categoryRepo;

        //using unit of work pattern
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        //get
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order cannot match the name");
            }
            if (obj.Name != null && obj.Name.ToLower() == "test")
            {
                //will not show under Name input field -> key is not specified
                ModelState.AddModelError("", "Test is an invalid name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index", "Category");
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                Trace.WriteLine(message);
            }
            return View();

        }
        //get action
        //passing id to controller via helper tag asp-route...
        public IActionResult Edit(int? id) //int? => nullable int
        {

            if (id is null or 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _unitOfWork.Category.Get(x => x.Id == id);
            //other methods to get item from db
            //Category? categoryFromDB2 = _dbContext.Categories.FirstOrDefault(u=>u.Name.Contains...);
            //Category categoryFromDB = _dbContext.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDB is null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        //post action
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category changed successfully.";
                return RedirectToAction("Index", "Category");
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                Trace.WriteLine(message);
            }
            return View();

        }

        //get action

        public IActionResult Delete(int? id) //int? => nullable int
        {

            if (id is null or 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _unitOfWork.Category.Get(x => x.Id == id);

            if (categoryFromDB is null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        //post  -> do this if arg same as get method
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(x => x.Id == id);
            if (obj is null)
            {
                return NotFound(id);
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");

        }
    }
}
