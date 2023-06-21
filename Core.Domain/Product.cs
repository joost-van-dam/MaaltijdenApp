using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } //Beschrijvende naam (niet leeg)
        public bool ContainsAlcohol { get; set; } //Alcoholhoudend ja/nee
        public List<Package>? Packages { get; set; }

        public string? ImageFormat { get; set; }

        public byte[]? ImageData { get; set; }

    }
}
