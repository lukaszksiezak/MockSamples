﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6_DbContextMocking
{
    public class OnlineStoreService
    {
        private OnlineStoreContext storeContext;

        public OnlineStoreService(OnlineStoreContext _context)
        {
            this.storeContext = _context;
        }

        public List<Product> GetProductsHistoryForCustomer (int customerId)
        {
            var listOfShopping = (this.storeContext.Persons.Where(c => c.Id.Equals(customerId)).First().HistoryOfShopping);
            return listOfShopping;
        }
        public List<Product> AllShopProducts (int shopId)
        {
            var listOfProducts = (this.storeContext.OnlineStore.Where(s => s.Id.Equals(shopId)).First().Products);
            return listOfProducts;
        }

        public List<Person> CustomersWhoBoughtProduct(int productId)
        {
            //Fun with queries: using Any
            var listOfClientsWhoBougthProduct = this.storeContext.Persons.Where(c => c.HistoryOfShopping.Any(p => p.Id.Equals(productId))).ToList();
            //Fun with queries: without using Any/Contains
            //var anotherWayToAcheiveSame = (this.storeContext.Persons.Where(c=> (c.HistoryOfShopping.Where(s=>s.Id.Equals(productId)).ToList().Count!=0)).ToList());
            
            return listOfClientsWhoBougthProduct;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await this.storeContext.Products.ToListAsync();
        }
    }
}
