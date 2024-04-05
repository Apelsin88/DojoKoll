using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace Kanske.Klasser
{
    public class DatabaseConnection
    {
        string server = "127.0.0.1";
        string database = "dojokollDB";
        string username = "root";
        string password = "Balle321";

        string connectionString = "";


        public DatabaseConnection()
        {
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";
        }
        //funkar
        public Dictionary<int, Judoka> GetJudokas()
        {
            Dictionary<int, Judoka> judokas = new Dictionary<int, Judoka>();

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM judokas";

            

            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                Judoka judoka = new Judoka((int)reader["judoka_id"], (string)reader["judoka_fname"], (string)reader["judoka_lname"], (int)reader["judoka_weightclass"], (int)reader["judoka_length"], (DateTime)reader["judoka_birth"], (int)reader["dojo_id"]);
                judokas.Add(judoka.Id, judoka);
            }

            connection.Close();
            return judokas;
        }
        //funkar
        public Dictionary<int, Dojo> GetDojos()
        {
            Dictionary<int, Dojo> dojos = new Dictionary<int, Dojo>();

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM dojos";

            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Dojo dojo = new Dojo((int)reader["dojo_id"], (string)reader["dojo_name"], (string)reader["dojo_password"]);
                dojos.Add(dojo.Id, dojo);
            }
            connection.Close();
            return dojos;
        }

        // AddDojo2 ar som AddNewStudent funktionen i dadabaskopplling17jan
        public Dojo AddDojo(string dName, string dPassword)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "CALL create_new_dojo(\"" + dName + "\", \"" + dPassword + "\");";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Dojo dojo = new Dojo((int)reader["dojo_id"], (string)reader["dojo_name"], (string)reader["dojo_password"]);
            connection.Close();
            return dojo;
        }

        //funkar
        public Judoka AddJudoka(string jfName, string jlName)
        {
           
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "CALL create_new_judoka(\"" + jfName + "\", \"" + jlName + "\");";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Judoka judoka = new Judoka((int)reader["judoka_id"], (string)reader["judoka_fname"], (string)reader["judoka_lname"], (int)reader["judoka_weightclass"], (int)reader["judoka_length"], (DateTime)reader["judoka_birth"], (int)reader["dojo_id"]);
            connection.Close();
            return judoka;
        }

        // funkar
        public bool DeleteJudoka(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "DELETE FROM judokas " +
                           "WHERE judoka_id = " + id + ";";

            MySqlCommand command = new MySqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            return rowsAffected > 0;
        }

        // funkar
        public bool DeleteJudokaInLinktable(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "DELETE FROM judokas_techniques_grade_linktable " +
                           "WHERE judoka_id = " + id + ";";

            MySqlCommand command = new MySqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            return rowsAffected > 0;
        }

        //funkar
        public void GetLinktable(Dictionary<int, Judoka> judokas, Dictionary<int, Technique> techniques)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            string query = "SELECT * FROM judokas_techniques_grade_linktable";

            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int judokaId = (int)reader["judoka_id"];
                int techniqueId = (int)reader["tech_id"];

                Judoka judoka = judokas[judokaId];
                Technique technique = techniques[techniqueId];

                judoka.Techniques.Add(technique);
                technique.Judokas.Add(judoka);
                
            }
            mySqlConnection.Close();

        }
        //funkar
        public void GetTechniqueLevel(Dictionary<int, Judoka> judokas, Dictionary<int, Technique> techniques)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            string query = "SELECT * FROM judokas_techniques_grade_linktable";

            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int judokaId = (int)reader["judoka_id"];
                int techniqueId = (int)reader["tech_id"];
                int level = (int)reader["grade"];

                Judoka judoka = judokas[judokaId];
                Technique technique = techniques[techniqueId];

                JudokaTechnique judokaTechnique = new JudokaTechnique(technique, level);

                judoka.JudokaTechniques.Add(judokaTechnique);

            }
            mySqlConnection.Close();

        }

        public Dictionary<int, Subcategory> GetSubcategorys()
        {
            Dictionary<int, Subcategory> subcategorys = new Dictionary<int, Subcategory>();

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM subcategorys";

            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Subcategory subcategory = new Subcategory((int)reader["subcategory_id"], (string)reader["subcategory_name"], (int)reader["maincategory_id"]);
                subcategorys.Add(subcategory.Id, subcategory);
            }
            connection.Close();
            return subcategorys;

        }
        //funkar
        public Dictionary<int, Technique> GetTechniques()
        {
            Dictionary<int, Technique> techniques = new Dictionary<int, Technique>();

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM techniques";

            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Technique technique = new Technique((int)reader["tech_id"], (string)reader["tech_name"], (int)reader["tech_level"], (int)reader["subcategory_id"]);
                techniques.Add(technique.Id, technique);
            }
            connection.Close();
            return techniques;
        }
        //funkar
        public bool UpdateJudokaFName(int jId, string fn)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "UPDATE judokas " +
                           "SET judoka_fname = \"" + fn + "\" " +
                           "WHERE judoka_id = " + jId + ";";

            MySqlCommand command = new MySqlCommand(query, connection);
            
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        //funkar
        public bool UpdateJudokaLName(int jId, string ln)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "UPDATE judokas " +
                           "SET judoka_lname = \"" + ln + "\" " +
                           "WHERE judoka_id = " + jId + ";";

            MySqlCommand command = new MySqlCommand(query, connection);

            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        
        //inte testad
        public void AddJudokaTechnique(int judokaKey, int techKey)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "INSERT INTO judokas_techniques_grade_linktable VALUES (" + judokaKey + ", " + techKey + ", DEFAULT)";

            MySqlCommand command = new MySqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();
        }
        // funkar
        public List<Judoka> Likeish(string s)
        {
            List<Judoka> list = new List<Judoka>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string t = "%" + s + "%";

            string query = "SELECT * FROM judokas WHERE judoka_fname LIKE \"" + t + "\"";

            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                Judoka judoka = new Judoka((int)reader["judoka_id"], (string)reader["judoka_fname"], (string)reader["judoka_lname"], (int)reader["judoka_weightclass"], (int)reader["judoka_length"], (DateTime)reader["judoka_birth"], (int)reader["dojo_id"]);
                list.Add(judoka);
            }

            connection.Close();
            return list;
        }

    }
}
