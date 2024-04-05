using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kanske.Klasser;
using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;

namespace Kanske
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();


        Dictionary<int, Dojo> dojos = new Dictionary<int, Dojo>();
        List<Dojo> dojoList = new List<Dojo>();

        

        public MainWindow()
        {
            InitializeComponent();

            
            //dojoList.Add(new Dojo(1, "National", "123"));
            //dojoList.Add(new Dojo(2, "GJK", "abc"));

            dojos = databaseConnection.GetDojos();
            dojoList = dojos.Values.ToList();
            


            Pointlable = chartPoint => string.Format("{0}({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
        }

        public Func<ChartPoint, string> Pointlable { get; set; }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (Dojo dojo in dojoList)
            {
                if (dojo.Name == dojoTextbox.Text && dojo.Password == dojoPasswordbox.Password)
                {
                    InsideDojoWindow insideDojoWindow = new InsideDojoWindow();
                    insideDojoWindow.Show();
                    insideDojoWindow.DojoNameTextBlock.Text = dojo.Name;
                    insideDojoWindow.loggedDojo = dojo;
                    this.Close();
                    break;
                }
            }
            
        }

        private void NewDojoButton_Click(object sender, RoutedEventArgs e)
        {
            NewDojoTextblock.Visibility = Visibility.Visible;
            NewDojoTextbox.Visibility = Visibility.Visible;
            NewPasswordTextblock.Visibility = Visibility.Visible;
            NewPasswordTextbox.Visibility = Visibility.Visible;
            CreateButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewDojoTextbox.Text != "" && NewPasswordTextbox.Text != "")
            {
                string name = NewDojoTextbox.Text;
                string password = NewPasswordTextbox.Text;

                Dojo dojo = databaseConnection.AddDojo(name, password);
                dojos.Add(dojo.Id, dojo); //Den eller
                //dojoList.Add(dojo); //Denna?

                dojos.Clear();
                dojoList.Clear();

                dojos = databaseConnection.GetDojos();
                dojoList = dojos.Values.ToList();

                NewDojoTextblock.Visibility = Visibility.Hidden;
                NewDojoTextbox.Visibility = Visibility.Hidden;
                NewPasswordTextblock.Visibility = Visibility.Hidden;
                NewPasswordTextbox.Visibility = Visibility.Hidden;
                CreateButton.Visibility = Visibility.Hidden;
                CancelButton.Visibility = Visibility.Hidden;
                MessageBox.Show(name + " was built :)");
                
            }
            else
            {
                MessageBox.Show("Somthing is missing");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewDojoTextbox.Text = null;
            NewPasswordTextbox.Text = null;
            NewDojoTextblock.Visibility = Visibility.Hidden;
            NewDojoTextbox.Visibility = Visibility.Hidden;
            NewPasswordTextblock.Visibility = Visibility.Hidden;
            NewPasswordTextbox.Visibility = Visibility.Hidden;
            CreateButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
        }
    }
}
