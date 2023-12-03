using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChefsAndDishes.Models;

// 1.- Definir uso de Entity Framework
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// 2.- Librería para poder hashear el password
using Microsoft.AspNetCore.Identity;

// 3.- Paquete manejador para Session's
using Microsoft.AspNetCore.Mvc.Filters;

// 4.- PARA REFERENCIAR UNA CONSULTA LAMBDA LINQ A UN MODELO CON POSTS SE DEBE INCLUIR EL SIGUIENTE PAQUETE
using Microsoft.EntityFrameworkCore;

namespace ChefsAndDishes.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    // GET => Index Chefs
    public IActionResult Index()
    {
        // Almacena en la variable AllChefs todos los chefs y que a su vez, incluya todos los platillos y los pase a una lista.
        List<Chef> AllChefs = _context.Chefs.Include(chef => chef.AllDishes).ToList();

        // Pas a la vista la lista de chefs
        return View(AllChefs);
    }

    // GET => Dishes Page
    [HttpGet("dishes")]
    public IActionResult AllDishes()
    {
        // Almacena en variable la consulta a la base de datos 
        List<Dish> AllDishes = _context.Dishes.Include(c => c.Creator).ToList();

        // Pas a la vista la lista de Dishes
        return View(AllDishes);
    }

    // GET => ADD a CHEF
    [HttpGet("chefs/new")]
    public IActionResult NewChef()
    {
        return View();
    }

    // GET => ADD a Dish
    [HttpGet("dishes/new")]
    public IActionResult NewDish()
    {
        // Almacena en variable todos los chefs 
        List<Chef> AllChefs = _context.Chefs.ToList();

        // Crea un vieBag para almacenar la lista de chefs
        ViewBag.Chefs = AllChefs;

        return View();
    }

    // POST => ADD CHEF
    [HttpPost("chefs/create")]
    public IActionResult CreateChef(Chef newChef)
    {
        if (ModelState.IsValid)
        {
            // Busca en la base de datos por el nombre y el apellido del chef y lo almacena en variable

            Chef? chefInDb = _context.Chefs.FirstOrDefault(chef => chef.FirstName.Equals(newChef.FirstName) && chef.LastName.Equals(newChef.LastName));

            // Valida si el chef en la base de datos existe 
            if (chefInDb != null)
            {
                // Si existe, muestra errores en el modelo, especificamente en los span de los camos FirstName y LastName
                ModelState.AddModelError("FirstName", "El chef ya existe");
                ModelState.AddModelError("LastName", "El chef ya existe");

                // Retorna la vista si los errores existen
                return View("NewChef");
            }

            // Si pasa las validaciones, pasa al contexto, el nuevo chef pasado desde la vista del formulario NewChef
            _context.Add(newChef);
            _context.SaveChanges();

            // Redirecciona a la vista Index, donde muestra todos los chefs en la base de datos
            return RedirectToAction("Index");
        }

        // Si no pasa las validaciones, muestra la vista con los errores de validación
        return View("NewChef");
    }

    // POST => ADD DISH
    [HttpPost("dishes/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if (ModelState.IsValid)
        {
            // Verificar si el plato existe en la base de datos
            Dish? dishInDb = _context.Dishes.FirstOrDefault(d => d.Name.Equals(newDish.Name));


            // Evalúa si el plato existe 
            if (dishInDb != null)
            {
                // Genera los errores en los span del modelo, especificamente en el input Name del formulario.
                ModelState.AddModelError("Name", "El plato ya existe");

                // Crea un VieBag, con la lista de los chefs para que los recargue en el formulario, si el platillo existe.
                ViewBag.Chefs = _context.Chefs.ToList();

                // Retorna la vista con los datos del nuevo platillo y los errores y además al crear el view Bag, nuevamente, ya que no hay persistencia de datos, el bucle en el formulario mostrará los datos de los chefs en la base de datos.
                return View("NewDish", newDish);
            }

            // Si pasa las validaciones anteriores, almacena en la base de datos el nuevo platillo con los datos pasados desde el formulario.
            _context.Add(newDish);
            _context.SaveChanges();

            return RedirectToAction("AllDishes");
        }

        // Nuevamente se crea un ViewBag, para que los Chefs puedan ser mostrados en el formulario en el menu desplegable select.
        ViewBag.Chefs = _context.Chefs.ToList();

        // Si no pasa las validaciones anteriores, además de crear el ViewBag y se muestren los datos en el formulario, se pasa el nuevo modelo, para la persistencia de los datos
        return View("NewDish", newDish);
    }

    public IActionResult Privacy()
    {

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
