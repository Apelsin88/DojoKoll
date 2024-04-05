using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanske
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainCategory { get; set; }
        public List<Technique> Techniques { get; set; } = new List<Technique>();

        public Subcategory(int id, string name, int mainCategory)
        {
            Id = id;
            Name = name;
            MainCategory = mainCategory;
        }
    }
}
