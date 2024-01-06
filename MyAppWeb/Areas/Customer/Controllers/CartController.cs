using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models.ViewModels;
using System.Security.Claims;

namespace MyAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private IUnitofWork _unitOfWork;
        public CartVM vm { get; set; }

        public CartController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            vm = new CartVM()
            {
                ListOfCart = _unitOfWork.Cart.GetAll(x=>x.ApplicationUserId==claims.Value, includeProperties: "Product"),
                OrderHeader=new MyApp.Models.OrderHeader()
            };
             

            foreach (var item in vm.ListOfCart)
            {
                vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            return View(vm);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            vm = new CartVM()
            {
                ListOfCart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new MyApp.Models.OrderHeader()
            };
            vm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetT(x => x.Id == claims.Value);
            vm.OrderHeader.Name = vm.OrderHeader.ApplicationUser.Name;
            vm.OrderHeader.Phone = vm.OrderHeader.ApplicationUser.PhoneNumber;
            vm.OrderHeader.Address = vm.OrderHeader.ApplicationUser.Address;
            vm.OrderHeader.City = vm.OrderHeader.ApplicationUser.City;
            vm.OrderHeader.State = vm.OrderHeader.ApplicationUser.State;
            vm.OrderHeader.PostalCode = vm.OrderHeader.ApplicationUser.Pincode;

            foreach (var item in vm.ListOfCart)
            {
                vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            return View(vm);
        }

        public IActionResult plus(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);
            _unitOfWork.Cart.IncrementCartItem(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index)); 
        }

        public IActionResult minus(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);
            if (cart.Count<=1) 
            {
                _unitOfWork.Cart.Delete(cart);
            }
            else
            {
                _unitOfWork.Cart.DecrementCartItem(cart, 1);
            }
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult delete(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);
            _unitOfWork.Cart.Delete(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
