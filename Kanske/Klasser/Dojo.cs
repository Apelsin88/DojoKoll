using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanske
{
    public class Dojo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Judoka> Judokas { get; set; } = new List<Judoka>();

        public Dojo(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
            //Judokas = judokas;
        }
    }
}
