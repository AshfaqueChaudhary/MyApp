using MyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private ApplicationDBContext _context;
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public ICartRepository Cart { get; private set; }
        public IApplicationUser ApplicationUser { get; private set; }

        public UnitofWork(ApplicationDBContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            Cart = new CartRepository(context);
            ApplicationUser = new ApplicationRepository(context);
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
