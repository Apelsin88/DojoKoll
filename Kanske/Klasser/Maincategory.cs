using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanske
{
    public class Maincategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
}
