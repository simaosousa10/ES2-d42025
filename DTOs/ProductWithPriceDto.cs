﻿namespace ESIID42025.DTOs;

public class ProductWithPriceDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string CategoryName { get; set; }
    public string ImageUrl { get; set; }
    public double Price { get; set; }
}