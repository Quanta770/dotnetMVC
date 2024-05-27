using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Razor.Data;
using WebApp_Razor.Models;

namespace WebApp_Razor.Pages.Category_
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public List<Category> CategoryList { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _dbContext = db;    
        }
        public void OnGet()
        {
            CategoryList = _dbContext.Categories.ToList();
        }
        
        public IActionResult OnPostDelete(int id)
        {
            if(id!=null && id!=0)
            {
                _dbContext.Categories.Remove(_dbContext.Categories.FirstOrDefault(x => x.Id == id));
                _dbContext.SaveChanges();
                return RedirectToPage("Index");
                //return Page();
            }
            return Page();
        }
    }
}
