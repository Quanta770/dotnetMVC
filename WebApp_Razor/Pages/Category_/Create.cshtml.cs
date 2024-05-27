using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Razor.Data;
using WebApp_Razor.Models;

namespace WebApp_Razor.Pages.Category_
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        [BindProperty]
        public Category Category { get; set; }  

        public CreateModel(ApplicationDbContext db)
        {
            _dbContext = db;
        }
        public void OnGet()
        {
        }

        //return to redirect to other pages
        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                _dbContext.Categories.Add(Category);
                _dbContext.SaveChanges();
                TempData["success"] = "Category created successfully.";
                return RedirectToPage("Index");
            }
            return Page();  
        }
    }
}
