using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Utility;

namespace WebApp_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        //unitofwork
        private readonly IUnitOfWork _unitOfWork;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            
            //passs list of product to index view
            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList
            ProductViewModel productVM = new()
            {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Product = new Product()
            };
            if(id == null || id == 0 ){
                //create
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(x => x.Id == id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductViewModel obj, IFormFile? file)
        {
           
           if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(obj.Product.ImageURL))
                    {
                        //there is image url - we are replacing with new image
                        // need to remove old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.Product.ImageURL = @"\images\product\" + filename;
                }
                if (obj.Product.Id == 0)
                {
                    //add
                    _unitOfWork.Product.Add(obj.Product);
                    TempData["success"] = "Product created successfully.";
                }
                else
                {
                    //update
                    _unitOfWork.Product.Update(obj.Product);
                    TempData["success"] = "Product edited successfully.";
                }
               
                _unitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                obj.CategoryList = _unitOfWork.Category.GetAll()
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    });
            }
            return View(obj);
        }

        public IActionResult Edit(int id)
        {
            if(id != null && id != 0)
            {
               Product result = _unitOfWork.Product.Get(x => x.Id == id);
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        //public IActionResult Delete(int id)
        //{
        //    if (id==null || id== 0)
        //    {
        //        return NotFound();
        //    }
        //    Product result = _unitOfWork.Product.Get(x => x.Id == id);

        //    if (result is null)
        //    {
        //        return NotFound();
        //    }
        //    return View(result);
        //}
        //[HttpPost]
        //// have to set input form as readonly not disabled. disabled form will be null for httppost
        //public IActionResult Delete(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Remove(product);
        //        _unitOfWork.Save();
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}
        //[HttpPost, ActionName("DeleteNow")]
        //public IActionResult DeleteNow(int id)
        //{
        //    Product result = _unitOfWork.Product.Get(x => x.Id == id);
        //    if (result is null)
        //    {
        //        return NotFound(id);
        //    }
        //    _unitOfWork.Product.Remove(result);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Category deleted successfully.";
        //    return RedirectToAction("Index");
        //}


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product product = _unitOfWork.Product.Get(x=>x.Id==id);
            if(product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageURL.TrimStart('\\'));
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }



}
