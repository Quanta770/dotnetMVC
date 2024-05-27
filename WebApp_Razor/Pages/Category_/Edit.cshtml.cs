using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Razor.Data;
using WebApp_Razor.Models;

namespace WebApp_Razor.Pages.Category_
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        [BindProperty]
        public Category categoryfromDB {  get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _dbContext = db;
        }
        public void OnGet(int id)
        {
            if(id !=null && id != 0){
                categoryfromDB = _dbContext.Categories.FirstOrDefault(x => x.Id == id);

            }

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _dbContext.Update(categoryfromDB);
                _dbContext.SaveChanges();
                TempData["success"] = "Category updates successfully.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
