using KiDemo.Backend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KiDemo.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string InputText { get; set; }

        [BindProperty]
        public string OutputText { get; set; }

        public void OnGet()
        {
            // Initialize properties if needed
        }

        public void OnPost()
        {
            OutputText = BackendService.ProcessUserInput(InputText);
        }
    }
}