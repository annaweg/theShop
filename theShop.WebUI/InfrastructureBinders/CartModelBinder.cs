using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using theShop.Domain.Entities;

namespace theShop.WebUI.InfrastructureBinders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        //ControllerContext has a httpContext property which has a session prop.
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //get the cart from the session
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }

            //create the cart if there wasn't one in the session data
            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            //return the cart
            return cart;
        }
    }
}