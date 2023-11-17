using FashionWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.InfraStructure.Request
{
    public class SearchPersonBusiness
    {
        public SearchPersonBusiness()
        {
            this.sortColumn = "IsSubscriber";
            this.sortAscending = false;
        }
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public bool? IsSubscriber { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public string sortColumn { get; set; }
        public bool sortAscending { get; set; }
        public int OrderStatusId { get; set; }
    }
}
