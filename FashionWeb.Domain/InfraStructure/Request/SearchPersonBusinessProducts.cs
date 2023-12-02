using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.InfraStructure.Request
{
    public class SearchPersonBusinessProducts
    {
        public SearchPersonBusinessProducts()
        {
            this.sortColumn = "CreateDate";
            this.sortAscending = true;
        }
        public int PersonBusinessId { get; set; }
        public int PersonId { get; set; }
        public int CategoryId { get; set; }
        public int ProductTypeId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string sortColumn { get; set; }
        public bool sortAscending { get; set; }
        public bool AllProducts { get; set; }
    }
}
