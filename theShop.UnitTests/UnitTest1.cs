using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using theShop.Domain.Abstract;
using theShop.Domain.Entities;
using theShop.WebUI.Controllers;
using theShop.WebUI.Models;
using theShop.WebUI.HtmlHelpers;

namespace theShop.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
           
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"},
            });
            //create controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
           
            //Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model; 

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange - define an HTML helper - in order to apply the extension method
            HtmlHelper myHelper = null;

            //Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            //Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" 
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" 
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>", 
                            result.ToString());
        }


        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"},
            });

            //Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        
        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange
            //creeate the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"},
            });
            //Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        //Unit tests for category list page 210

        //Unit tests for selected category page 213

        //Unit test for category-specific product count 216

        [TestClass]
        public class CartTests
        {
            [TestMethod]
            public void Can_Add_New_Lines()
            {
                //Arrange - create some test products
                Product p1 = new Product { ProductID = 1, Name = "P1" };
                Product p2 = new Product { ProductID = 2, Name = "P2" };

                //Arrange - create new cart
                Cart target = new Cart();

                //Act
                target.AddItem(p1, 1);
                target.AddItem(p2, 1);
                CartLine[] result = target.Lines.ToArray();

                //Assert
                Assert.AreEqual(result.Length, 2);
                Assert.AreEqual(result[0].Product, p1);
                Assert.AreEqual(result[1].Product, p2);
            }


            [TestMethod]
            public void Can_Add_Quantity_For_Existing_Lines()
            {
                //Arrange - create some test products
                Product p1 = new Product { ProductID = 1, Name = "P1" };
                Product p2 = new Product { ProductID = 2, Name = "P2" };

                //Arrange - create new cart
                Cart target = new Cart();

                //Act
                target.AddItem(p1, 1);
                target.AddItem(p2, 1);
                target.AddItem(p1, 10);
                CartLine[] result = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

                //Assert
                Assert.AreEqual(result.Length, 2);
                Assert.AreEqual(result[0].Product, p1);
                Assert.AreEqual(result[1].Product, p2);
            }

            //more Cart tests on page 220

            [TestMethod]
            public void Can_Add_To_Cart()
            {
                //Arrange - create the mock repository
                Mock<IProductRepository> mock = new Mock<IProductRepository>();
                mock.Setup(m => m.Products).Returns(new Product[]
                {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                }.AsQueryable());

                //Arrange - create cart
                Cart cart = new Cart();

                //Arrange - create the controller
                CartController target = new CartController(mock.Object);

                //Act - add a product tot the cart
                target.AddToCart(cart, 1, null);

                //Assert
                Assert.AreEqual(cart.Lines.Count(), 1);
                Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
            }

            //more Cart tests on page 232
        }

        //deleting product test page 302
    }
}

