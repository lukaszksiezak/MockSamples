﻿using System.Collections.Generic;

namespace EF6_DbContextMocking
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public virtual List<Product> HistoryOfShopping { get; set; }
    }
}