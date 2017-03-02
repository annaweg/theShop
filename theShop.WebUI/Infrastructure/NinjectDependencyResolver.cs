using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using Ninject;
using theShop.Domain.Abstract;
using theShop.Domain.Concrete;
using theShop.Domain.Entities;


namespace theShop.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        //Constructor
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type ServiceType)
        {
            return kernel.TryGet(ServiceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
            
        }
    }
}