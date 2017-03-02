using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using theShop.Domain.Abstract;
using theShop.Domain.Entities;

namespace theShop.WebUI.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        //constructor
        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                //tempData message deleted at end of httpRequest
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                //return to list of products if something is wrong with the data values
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productID)
        {
            Product deletedProduct = repository.DeleteProduct(productID);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}