using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class UserTasksController : Controller
    {
        private readonly AppDbContext _context;

        public UserTasksController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.Tasks.Include(t => t.User).ToList();
            return View(tasks); // this populates the Model for the view
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}



// ispod je ostali kod koji sada ne koristim



//private readonly IBookRepository bookRepository;

//public BooksController(IBookRepository bookRepository)
//{
//    this.bookRepository = bookRepository;
//}
//public IActionResult Index()
//{
//    List<Book> books = bookRepository.GetAll();
//    ListBooksViewModel model = new();
//    model.Books = books.Select(b => new BookViewModel { Id = b.Id, Title = b.Title, Author = $"{b.Author.FirstName} {b.Author.LastName}" }).ToList();
//    return View(model);
//}
//[HttpGet]
//public IActionResult Add()
//{
//    CreateBookViewModel model = new CreateBookViewModel();
//    List<Author> authors = new List<Author>();
//    authors.Add(new Author { Id = 1, FirstName = "FirstName1", LastName = "LastName1" });
//    authors.Add(new Author { Id = 2, FirstName = "FirstName2", LastName = "LastName2" });

//    //List<SelectListItem> selectListItems = new List<SelectListItem>();
//    //foreach(Author a in authors)
//    //{
//    //    var listItem = new SelectListItem(a.FirstName + a.LastName, a.Id.ToString());
//    //    selectListItems.Add(listItem);
//    //}
//    List<SelectListItem> selectListItems =
//        authors
//        .Select(a => 
//        new SelectListItem(a.FirstName + a.LastName, a.Id.ToString())
//        ).ToList();
//    model.Authors = selectListItems;

//    return View("Create", model);
//}
//[HttpPost]
//public IActionResult Add(CreateBookViewModel b)
//{
//    if (ModelState.IsValid)
//    {
//        TempData["Message"] = "Success!";
//        return RedirectToAction("Index");
//    }

//    ModelState.AddModelError("", "Greska!"); //vezuje se za model
//    //prikazuje se samo u asp-validation-summary=All/ModelOnly
//    //ModelState.AddModelError("Title", "Greska!");
//    return Add();
//}

//[Route("table")]
//public IActionResult GetTable()
//{
//    List<Book> books = bookRepository.GetAll();
//    ListBooksViewModel model = new();
//    model.Books = books.Select(b => new BookViewModel { Id = b.Id, Title = b.Title, Author = $"{b.Author.FirstName} {b.Author.LastName}" }).ToList();
//    return PartialView("Table", model);
//}













//private readonly IBookRepository bookRepository;

//public OrdersController(IBookRepository bookRepository)
//{
//    this.bookRepository = bookRepository;
//}

//[HttpGet]
//public IActionResult Create()
//{
//    CreateOrderViewModel model = new CreateOrderViewModel();
//    model.Books = bookRepository
//        .GetAll()
//        .Select(b => 
//            new SelectListItem(b.Title, b.Id.ToString())
//        ).ToList();
//    return View(model);
//}

//[HttpPost]
//public IActionResult Create(CreateOrderViewModel model)
//{
//    if (ModelState.IsValid)
//    {
//        TempData["Message"] = "Porudzbina je kreirana!";
//        return RedirectToAction("Index", "Books");
//    }
//    return Create();
//}
