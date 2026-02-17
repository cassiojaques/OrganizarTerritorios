using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OrganizarTerritorios.Helpers
{
    public static class AlertHelper
    {
        public static void AlertSucesso(PageModel page, string message)
        {
            page.TempData["AlertMessage"] = message;
            page.TempData["AlertType"] = "success";
        }

        public static void AlertErro(PageModel page, string message)
        {
            page.TempData["AlertMessage"] = message;
            page.TempData["AlertType"] = "danger";
        }
    }
}
