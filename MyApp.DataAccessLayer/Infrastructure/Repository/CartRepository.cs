﻿using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private ApplicationDBContext _context;
        public CartRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        
    }
}