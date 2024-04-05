using Kanske.Klasser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Kanske
{
    public enum JudokaGender { Male, Female };
    //public enum Weightclass { -48kg = 1, -52kg = 2, -57kg = 3, -60kg = 4, -63kg = 5, -66kg = 6, -70kg = 7, -73kg = 8, -78kg = 9, +78kg = 10, -81kg = 11, -90kg = 12, -100kg = 13, +100kg = 14 };
    public class Judoka
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public JudokaGender Gender { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public DateTime Birth { get; set; }
        public int Dojo { get; set; }

        public List<Technique> Techniques { get; set; } = new List<Technique>();
        public List<JudokaTechnique> JudokaTechniques { get; set; } = new List<JudokaTechnique>();


        //Hela constructorn
        public Judoka(int id, string firstName, string lastName, JudokaGender gender, int weight, int length, DateTime birth, int dojo)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Weight = weight;
            Length = length;
            Birth = birth;
            Dojo = dojo;

        }
        //Allt utom JudokaGender
        public Judoka(int id, string firstName, string lastName, int weight, int length, DateTime birth, int dojo)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Weight = weight;
            Length = length;
            Birth = birth;
            Dojo = dojo;

        }
        //Test med JudokaGender
        public Judoka(int id, string firstName, string lastName, JudokaGender gender)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            

        }
        //Test med DateTime
        public Judoka(int id, string firstName, string lastName, DateTime birth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Birth = birth;


        }
    }
}
