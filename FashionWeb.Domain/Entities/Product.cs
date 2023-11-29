using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int PersonBusinessId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public bool AcceptComplement { get; set; }
        public bool IsComplement { get; set; }
        public int CountAdd { get; set; }
        public int MaxComplement { get; set; }
        public bool IsDigitalShop { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }
        public List<Product> Complements { get; set; }
        public List<ProductArchive> ProductArchives { get; set; }
        public List<Tamanho> ProductTamanhos { get; set; }
        public List<Cor> ProductCores { get; set; }
        public List<ProductConfig> ProductConfigs { get; set; }
        public int ColorId { get; set; }
        public int TamanhoId { get; set; }
    }
}
