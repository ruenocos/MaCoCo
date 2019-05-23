using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaCoCo.Models
{
    public class Category
    {
        public int categoryId { get; set; }
        public int parentId { get; set; }
        public string name { get; set; }
    }
}
