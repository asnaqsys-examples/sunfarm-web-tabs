using ASNA.QSys.Expo.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SunFarmSite.Pages.Asna
{
    public class DiagnoseModel : PageModel
    {
        public string AbEndMessage { get; set; }
        public string AbEndStack { get; set; }
        public void OnGet()
        {
            AbEndMessage = Request.Query.ContainsKey("AbEndMessage") ? Request.Query["AbEndMessage"] : string.Empty;
            AbEndMessage = AbEndMessage.Replace("\\n", "\n");
            AbEndMessage = AbEndMessage.Replace("\\r", "\r");
 
            AbEndStack = Request.Query.ContainsKey("AbEndStack") ? Request.Query["AbEndStack"] : string.Empty;
            AbEndStack = AbEndStack.Replace("\\n", "\n");
            AbEndStack = AbEndStack.Replace("\\r", "\r");        
        }
    }
}