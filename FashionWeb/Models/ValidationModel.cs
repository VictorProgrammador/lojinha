using System.Collections.Generic;

namespace FashionWeb.Models
{
    public class ValidationModel
    {
        public bool success { get; set; }
        public List<string> errorList { get; private set; }
        public ValidationModel(List<string> errors)
        {
            this.errorList = errors;
        }
    }
}
