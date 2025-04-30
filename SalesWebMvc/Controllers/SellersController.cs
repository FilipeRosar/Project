using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellersService _sellersService;
        private readonly DepartmentsService _departmentsService;
        public SellersController(SellersService sellersService, DepartmentsService departmentsService)
        {
            _sellersService = sellersService;
            _departmentsService = departmentsService;

        }
        public IActionResult Index()
        {
            var list = _sellersService.FindAll();
            return View(list);
        }
        public IActionResult Create()
        {
            var departments = _departmentsService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellersService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _sellersService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _sellersService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _sellersService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _sellersService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            List<Department> departments = _departmentsService.FindAll();
            SellerFormViewModel sellerFormViewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(sellerFormViewModel);

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }
            try
            {
                _sellersService.Update(seller);
                return RedirectToAction(nameof(Index));
            } catch (NotFoundException)
            {
                return NotFound();
            } catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
