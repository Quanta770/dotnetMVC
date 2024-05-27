using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Net;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        public CategoryController(ApplicationDBContext db) {
            
            _dbContext = db;
        
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _dbContext.Categories.ToList();
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
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order cannot match the name");
            }
            if (obj.Name != null && obj.Name.ToLower() == "test" )
            {
                //will not show under Name input field -> key is not specified
                ModelState.AddModelError("", "Test is an invalid name");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(obj);
                _dbContext.SaveChanges();
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
            
            if(id is null or 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _dbContext.Categories.Find(id);
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
                _dbContext.Categories.Update(obj);
                _dbContext.SaveChanges();
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
            Category? categoryFromDB = _dbContext.Categories.Find(id);
           
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
            Category? obj = _dbContext.Categories.FirstOrDefault(x=>x.Id == id);
            if (obj is null)
            {
                return NotFound(id);    
            }
            _dbContext.Categories.Remove(obj);
            _dbContext.SaveChanges();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");

        }
    }
}
