﻿namespace ESIID42025.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}