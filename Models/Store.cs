using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Store
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Google Maps URL")]
    public string GoogleMaps_URL { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Registration Date")]
    public DateTime Registration_Date { get; set; }

    // Foreign Key (nullable)
    public int? ReportID { get; set; }

    // Navigation Property (optional)
    public Report? Report { get; set; }

    // Optional Collections - Initialized to prevent validation issues
    public ICollection<Price>? Prices { get; set; } = new List<Price>();

    public ICollection<StoreProd>? StoreProducts { get; set; } = new List<StoreProd>();
}