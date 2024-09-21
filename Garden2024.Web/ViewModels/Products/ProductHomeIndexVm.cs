﻿using System.ComponentModel;

namespace Garden2024.Web.ViewModels.Products
{
    public class ProductHomeIndexVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal ListPrice { get; set; }
        public decimal CashPrice { get; set; }
        public bool Suspended { get; set; }
        public string? ImageUrl { get; set; }
    }
}