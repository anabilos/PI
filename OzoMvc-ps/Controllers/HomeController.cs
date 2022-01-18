using Microsoft.AspNetCore.Mvc;

namespace OzoMvc.Controllers{
public class HomeController : Controller{
    public IActionResult Index(){
        return View();
    }
}
}