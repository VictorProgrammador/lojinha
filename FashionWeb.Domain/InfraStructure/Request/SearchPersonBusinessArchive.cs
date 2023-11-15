using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.InfraStructure.Request
{
    public class SearchPersonBusinessArchive
    {
        public SearchPersonBusinessArchive()
        {
            this.sortColumn = "CreateDate";
            this.sortAscending = true;
        }
        public int PersonBusinessId { get; set; }
        public int PersonId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string sortColumn { get; set; }
        public bool sortAscending { get; set; }
    }
}
