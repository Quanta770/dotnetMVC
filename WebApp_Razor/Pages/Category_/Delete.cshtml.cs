using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Razor.Data;
using WebApp_Razor.Models;

namespace WebApp_Razor.Pages.Category_
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        [BindProperty]
        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _dbContext = db;
        }
        public void OnGet(int id)
        {
            if (id != null && id != 0)
            {
                Category = _dbContext.Categories.FirstOrDefault(x => x.Id == id);

            }
        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                _dbContext.Remove(Category);
                _dbContext.SaveChanges();
                TempData["success"] = "Category deleted.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
