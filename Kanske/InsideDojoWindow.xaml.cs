using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Kanske.Klasser;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.BC;

namespace Kanske
{
    /// <summary>
    /// Interaction logic for InsideDojoWindow.xaml
    /// </summary>
    public partial class InsideDojoWindow : Window
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();


        Dictionary<int, Judoka> judokas = new Dictionary<int, Judoka>();
        List<Judoka> judokaList = new List<Judoka>();

        Dictionary<int, Subcategory> subcategorys = new Dictionary<int, Subcategory>();
        List<Subcategory> subcategoryList = new List<Subcategory>();

        Dictionary<int, Technique> techniques = new Dictionary<int, Technique>();
        List<Technique> techniqueList = new List<Technique>();

        //Dictionary<int, JudokaTechnique> judokaTechniques = new Dictionary<int, JudokaTechnique>();
        List<JudokaTechnique> judokaTechniquesList = new List<JudokaTechnique>();

        public Dojo loggedDojo;

        //public Func<ChartPoint, string> Pointlable { get; set; }

        //private ObservableValue value1;

        
        public SeriesCollection osaekomiWazaSeriesCollection { get; set; }
        public SeriesCollection teWazaSeriesCollection { get; set; }
        public SeriesCollection shimeWazaSeriesCollection { get; set; }
        public SeriesCollection koshiWazaSeriesCollection { get; set; }
        public SeriesCollection sutemiWazaSeriesCollection { get; set; }
        public SeriesCollection kansetsuWazaSeriesCollection { get; set; }
        public SeriesCollection ashiWazaSeriesCollection { get; set; }

        //public ObservableCollection<double> NotEvaluatedValues { get; set; }
        //public ObservableCollection<double> Level1Values { get; set; }
        //public ObservableCollection<double> Level2Values { get; set; }
        //public ObservableCollection<double> Level3Values { get; set; }

        //public int osaekomiWazaNotEv {  get; set; }

        public int tewazaNotEvaluatedSum {  get; set; }
        public int tewazaLevel1Sum {  get; set; }
        public int tewazaLevel2Sum {  get; set; }
        public int tewazaLevel3Sum {  get; set; }

        //public int loggedDojo {  get; set; }
        public int j = 0;

        public InsideDojoWindow()
        {
            InitializeComponent();

            

            judokas = databaseConnection.GetJudokas();
            judokaList = judokas.Values.ToList();

            subcategorys = databaseConnection.GetSubcategorys();
            subcategoryList = subcategorys.Values.ToList();

            techniques = databaseConnection.GetTechniques();
            techniqueList = techniques.Values.ToList();

            databaseConnection.GetLinktable(judokas,techniques);
            databaseConnection.GetTechniqueLevel(judokas, techniques);

            judokaTechniquesList.Add(new JudokaTechnique(judokaList[2].Techniques[3], 3));
            Testa();
            

            //JudokasListbox.ItemsSource = judokas.Values;
            //JudokasListbox.Items.Refresh();

            //judokaList.Add(new Judoka(1, "Errka", "R", JudokaGender.Male, 60, 175, new DateTime(1997,06,14), 1));
            //judokaList.Add(new Judoka(2, "Perrka", "Boi", JudokaGender.Male, 60, 175, new DateTime(1997, 06, 14), 2));

            

            foreach (Judoka judoka in judokaList)
            {
                JudokasListbox.Items.Add(judoka.Id + ": " + judoka.FirstName + " " + judoka.LastName + ": " + judoka.Weight + ": " + judoka.Length + ": " + judoka.Birth + ": " + judoka.Dojo);
            }

            

            //Pointlable = chartPoint => string.Format("{0}({1:P})", chartPoint.Y, chartPoint.Participation);

            //if (JudokasListbox.SelectedItem != null)
            //{
            //    int judokaIndex = JudokasListbox.SelectedIndex;
            //    Judoka pudoka = judokaList[judokaIndex];
            //    foreach (Technique tech in pudoka.Techniques)
            //    {
            //        TechniqesListbox.Items.Add(tech.Name);
            //    }
            //}

            //osaekomiWazaNotEv = 15;

            foreach (Technique tech in techniqueList)
            {
                TechniqesListbox.Items.Add(tech.Name);
            }

            //tewazaNotEvaluatedSum = 5;
            //tewazaLevel1Sum = 2;
            //tewazaLevel2Sum = 5;
            //tewazaLevel3Sum = 2;

            //int judokaIndex = JudokasListbox.SelectedIndex;
            //Judoka j = judokaList[judokaIndex];

            Judoka k = judokaList[j];

            OsaeKomiWazaCirkel();
            TeWazaCirkel(GetTewazaNotEvaluatedSum(k), tewazaLevel1Sum, tewazaLevel2Sum, tewazaLevel3Sum);
            ShimeWazaCirkel();
            KoshiWazaCirkel();
            SutemiWazaCirkel();
            KansetsuWazaCirkel();
            //AshiWazaCirkel();
            DataContext = this;
        }
        
        public int GetTewazaNotEvaluatedSum(Judoka judoka)
        {
            tewazaNotEvaluatedSum = 0;
            foreach (Technique tech in judoka.Techniques)
            {
                if (tech.Subcategory == 3)
                {
                    if (tech.Level == 0)
                    {
                        tewazaNotEvaluatedSum += 1;
                    }
                    if (tech.Level == 1)
                    {
                        tewazaLevel1Sum += 1;
                    }
                }
            }
            return tewazaNotEvaluatedSum;
        }

        public void OsaeKomiWazaCirkel()
        {
            int nummer = 11;
            int level1sum = 0;
            osaekomiWazaSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title="Not Evaluated",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(nummer)},
                    DataLabels=true,
                    
                },
                new PieSeries
                {
                    Title="Level 1",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(8)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 2",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 3",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
            };
            DataContext = this;
        }
        
        public void TeWazaCirkel(int tewazaNotEvaluatedSum, int tewazaLevel1Sum, int tewazaLevel2Sum, int tewazaLevel3sum)
        {
            teWazaSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title="Not Evaluated",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(tewazaNotEvaluatedSum) },
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 1",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(tewazaLevel1Sum) },
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 2",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(tewazaLevel2Sum) },
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 3",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(tewazaLevel3sum) },
                    DataLabels=true,
                },
            };
            DataContext = this;
        }
        public void ShimeWazaCirkel()
        {
            int nummer = 11;
            shimeWazaSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title="Not Evaluated",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(nummer)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 1",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(8)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 2",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 3",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
            };
            DataContext = this;
        }
        
        public void KoshiWazaCirkel()
        {
            int nummer = 11;
            koshiWazaSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title="Not Evaluated",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(nummer)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 1",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(8)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 2",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 3",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
            };
            DataContext = this;
        }
        public void SutemiWazaCirkel()
        {
            int nummer = 11;
            sutemiWazaSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title="Not Evaluated",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(nummer)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 1",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(8)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 2",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 3",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
            };
            DataContext = this;
        }
        public void KansetsuWazaCirkel()
        {
            int nummer = 11;
            kansetsuWazaSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title="Not Evaluated",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(nummer)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 1",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(8)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 2",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
                new PieSeries
                {
                    Title="Level 3",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
            };
            DataContext = this;
        }
        public void AshiWazaCirkel()
        {
            
            
                int ashiwazaNotEvaluatedSum = 0;
                int ashiwazaLevel1Sum = 0;
                int ashiwazaLevel2Sum = 0;
                int ashiwazaLevel3Sum = 0;

                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                foreach (Technique tech in judoka.Techniques)
                {
                    if (tech.Subcategory == 3)
                    {
                        if (tech.Level == 0)
                        {
                            ashiwazaNotEvaluatedSum += 1;
                        }
                        if (tech.Level == 1)
                        {
                            ashiwazaLevel1Sum += 1;
                        }
                        if (tech.Level == 2)
                        {
                            ashiwazaLevel2Sum += 1;
                        }
                        if (tech.Level == 3)
                        {
                            ashiwazaLevel3Sum += 1;
                        }
                    }
                }
                ashiWazaSeriesCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title="Not Evaluated",
                        Values=new ChartValues<ObservableValue> {new ObservableValue(ashiwazaNotEvaluatedSum) },
                        DataLabels=true,
                    },
                    new PieSeries
                    {
                        Title="Level 1",
                        Values=new ChartValues<ObservableValue> {new ObservableValue(ashiwazaLevel1Sum) },
                        DataLabels=true,
                    },
                    new PieSeries
                    {
                        Title="Level 2",
                        Values=new ChartValues<ObservableValue> {new ObservableValue(ashiwazaLevel2Sum) },
                        DataLabels=true,
                    },
                    new PieSeries
                    {
                        Title="Level 3",
                        Values=new ChartValues<ObservableValue> {new ObservableValue(ashiwazaLevel3Sum) },
                        DataLabels=true,
                    },
                };
                DataContext = this;
            
                
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void ShowJudokaStats(int notEvaluatedSum, int level1Sum, int level2Sum, int level3Sum)
        {
            if (JudokasListbox.SelectedItem != null)
            {
                //int notEvaluatedSum;
                //int level1Sum;
                //int level2Sum;
                //int level3Sum;

                Subcategory tewaza = subcategoryList[0];
                Subcategory koshiwaza = subcategoryList[1];
                Subcategory ashiwaza = subcategoryList[2];
                Subcategory sutemiwaza = subcategoryList[3];
                Subcategory osaekomiwaza = subcategoryList[4];
                Subcategory shimewaza = subcategoryList[5];
                Subcategory kansetsuwaza = subcategoryList[6];

                //int judokaIndex = JudokasListbox.SelectedIndex;
                //Judoka judoka = judokaList[judokaIndex];

                //foreach (Technique tech in judoka.Techniques)
                //{
                //    if (tech.Subcategory == tewaza.Id && tech.Level == 0)
                //    {
                //        notEvaluatedSum += 1;

                //    }
                //}


            }
            //return notEvaluatedSum;
        }

        private int[] GetLevelSums(Judoka judoka)
        {
            int[] levelSums = new int[4];

            return levelSums;
        }
        private void osaekomiWazaPiechart_DataClick(object sender, ChartPoint chartPoint)
        {
            Subcategory subcategory = subcategoryList[4];
            
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                RefreshTechniquesListbox2(judoka, subcategory);
            }

            //returna sum variablerna? alla i fran samma funktion eller de 4 nivaerna per kategori?

            //    if (JudokasListbox.SelectedItem != null)
            //    {
            //        Subcategory subcategory = subcategoryList[4];
            //        int tewazaNotEvaluatedSum = 0;
            //        int tewazaLevel1Sum = 0;
            //        int tewazaLevel2Sum = 0;
            //        int tewazaLevel3Sum = 0;

            //        int koshiwazaNotEvaluatedSum = 0;
            //        int koshiwazaLevel1Sum = 0;
            //        int koshiwazaLevel2Sum = 0;
            //        int koshiwazaLevel3Sum = 0;

            //        int ashiwazaNotEvaluatedSum = 0;
            //        int ashiwazaLevel1Sum = 0;
            //        int ashiwazaLevel2Sum = 0;
            //        int ashiwazaLevel3Sum = 0;

            //        int sutemiwazaNotEvaluatedSum = 0;
            //        int sutemiwazaLevel1Sum = 0;
            //        int sutemiwazaLevel2Sum = 0;
            //        int sutemiwazaLevel3Sum = 0;

            //        int osaekomiwazaNotEvaluatedSum = 0;
            //        int osaekomiwazaLevel1Sum = 0;
            //        int osaekomiwazaLevel2Sum = 0;
            //        int osaekomiwazaLevel3Sum = 0;

            //        int shimewazaNotEvaluatedSum = 0;
            //        int shimewazaLevel1Sum = 0;
            //        int shimewazaLevel2Sum = 0;
            //        int shimewazaLevel3Sum = 0;

            //        int kansetsuwazaNotEvaluatedSum = 0;
            //        int kansetsuwazaLevel1Sum = 0;
            //        int kansetsuwazaLevel2Sum = 0;
            //        int kansetsuwazaLevel3Sum = 0;


            //        int judokaIndex = JudokasListbox.SelectedIndex;
            //        Judoka judoka = judokaList[judokaIndex];
            //        foreach (Technique tech in judoka.Techniques)
            //        {
            //            if (tech.Subcategory == 1)
            //            {
            //                if(tech.Level == 0)
            //                {
            //                    tewazaNotEvaluatedSum += 1;
            //                }
            //                if (tech.Level == 1)
            //                {
            //                    tewazaLevel1Sum += 1;
            //                }
            //                if (tech.Level == 2)
            //                {
            //                    tewazaLevel2Sum += 1;
            //                }
            //                if (tech.Level == 3)
            //                {
            //                    tewazaLevel3Sum += 1;
            //                }
            //            }
            //            if (tech.Subcategory == 2)
            //            {
            //                if (tech.Level == 0)
            //                {
            //                    koshiwazaNotEvaluatedSum += 1;
            //                }
            //                if (tech.Level == 1)
            //                {
            //                    koshiwazaLevel1Sum += 1;
            //                }
            //                if (tech.Level == 2)
            //                {
            //                    koshiwazaLevel2Sum += 1;
            //                }
            //                if (tech.Level == 3)
            //                {
            //                    koshiwazaLevel3Sum += 1;
            //                }
            //            }
            //            if (tech.Subcategory == 3)
            //            {
            //                if (tech.Level == 0)
            //                {
            //                    ashiwazaNotEvaluatedSum += 1;
            //                }
            //                if (tech.Level == 1)
            //                {
            //                    ashiwazaLevel1Sum += 1;
            //                }
            //                if (tech.Level == 2)
            //                {
            //                    ashiwazaLevel2Sum += 1;
            //                }
            //                if (tech.Level == 3)
            //                {
            //                    ashiwazaLevel3Sum += 1;
            //                }
            //            }
            //            if (tech.Subcategory == 4)
            //            {
            //                if (tech.Level == 0)
            //                {
            //                    sutemiwazaNotEvaluatedSum += 1;
            //                }
            //                if (tech.Level == 1)
            //                {
            //                    sutemiwazaLevel1Sum += 1;
            //                }
            //                if (tech.Level == 2)
            //                {
            //                    sutemiwazaLevel2Sum += 1;
            //                }
            //                if (tech.Level == 3)
            //                {
            //                    sutemiwazaLevel3Sum += 1;
            //                }
            //            }
            //            if (tech.Subcategory == 5)
            //            {
            //                if (tech.Level == 0)
            //                {
            //                    osaekomiwazaNotEvaluatedSum += 1;
            //                }
            //                if (tech.Level == 1)
            //                {
            //                    osaekomiwazaLevel1Sum += 1;
            //                }
            //                if (tech.Level == 2)
            //                {
            //                    osaekomiwazaLevel2Sum += 1;
            //                }
            //                if (tech.Level == 3)
            //                {
            //                    osaekomiwazaLevel3Sum += 1;
            //                }
            //            }
            //            if (tech.Subcategory == 6)
            //            {
            //                if (tech.Level == 0)
            //                {
            //                    shimewazaNotEvaluatedSum += 1;
            //                }
            //                if (tech.Level == 1)
            //                {
            //                    shimewazaLevel1Sum += 1;
            //                }
            //                if (tech.Level == 2)
            //                {
            //                    shimewazaLevel2Sum += 1;
            //                }
            //                if (tech.Level == 3)
            //                {
            //                    shimewazaLevel3Sum += 1;
            //                }
            //            }
            //            if (tech.Subcategory == 7)
            //            {
            //                if (tech.Level == 0)
            //                {
            //                    kansetsuwazaNotEvaluatedSum += 1;
            //                }
            //                if (tech.Level == 1)
            //                {
            //                    kansetsuwazaLevel1Sum += 1;
            //                }
            //                if (tech.Level == 2)
            //                {
            //                    kansetsuwazaLevel2Sum += 1;
            //                }
            //                if (tech.Level == 3)
            //                {
            //                    kansetsuwazaLevel3Sum += 1;
            //                }
            //            }

            //        }
            //    }

        }

        private void SerchButton_Click(object sender, RoutedEventArgs e)
        {
            List<Judoka> sjudokaList = new List<Judoka>();
            if (SerchTextBox.Text.Length > 0)
            {
                JudokasListbox.Items.Clear();
                string s = SerchTextBox.Text;
                sjudokaList = databaseConnection.Likeish(s);
                foreach (Judoka j in sjudokaList)
                {
                    foreach (Judoka rj in judokaList)
                    {
                        if (j.FirstName == rj.FirstName)
                        {
                            JudokasListbox.Items.Add(rj.Id + ": " + rj.FirstName + " " + rj.LastName + ": " + rj.Weight + ": " + rj.Length + ": " + rj.Birth + ": " + rj.Dojo);
                            
                        }
                    }
                }
            }
            else
            {
                JudokasListbox.Items.Clear();
                foreach (Judoka judoka in judokaList)
                {
                    JudokasListbox.Items.Add(judoka.Id + ": " + judoka.FirstName + " " + judoka.LastName + ": " + judoka.Weight + ": " + judoka.Length + ": " + judoka.Birth + ": " + judoka.Dojo);
                }
            }
            
            
        }

        private void AddJudokaButton_Click(object sender, RoutedEventArgs e)
        {
            addJudokaStackpanel.Visibility = Visibility.Visible;
            addJudokaStackpanel2.Visibility = Visibility.Visible;

        }
        private void RefreshTechniquesListbox2(Judoka ju, Subcategory sub)
        {
            TechniqesListbox.Items.Clear();
            JudokaNameTextBlock.Text = ju.FirstName;
            foreach (JudokaTechnique jt in ju.JudokaTechniques)
            {
                if (jt.Tech.Subcategory == sub.Id)
                {
                    TechniqesListbox.Items.Add("Level " + jt.Level + ": " + jt.Tech.Name);
                }
            }
        }
        private void RefreshJudokasListbox()
        {
            JudokasListbox.Items.Clear();
            foreach (Judoka judoka in judokaList)
            {
                JudokasListbox.Items.Add(judoka.FirstName + " " + judoka.LastName + ": " + judoka.Weight + ": " + judoka.Length + ": " + judoka.Dojo);
            }


        }

        private void Testa()
        {
            foreach (Technique technique in techniqueList)
            {
                JudokaTechnique judokaTechnique = new JudokaTechnique(technique, 0);
                judokaTechniquesList.Add(judokaTechnique);
            }
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            //if(TechniqesListbox.SelectedItem != null)
            //{
            //    int judokaTechIndex = TechniqesListbox.SelectedIndex;
            //    JudokaTechnique judokaTech = TechniqesListbox[judokaTechIndex];
            //    RefreshTechniquesListbox2(judoka);
            //}
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void shimeWazaPiechart_DataClick(object sender, ChartPoint chartPoint)
        {
            Subcategory subcategory = subcategoryList[5];
            
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                RefreshTechniquesListbox2(judoka, subcategory);
            }
        }

        private void teWazaPiechart_DataClick(object sender, ChartPoint chartPoint)
        {
            Subcategory subcategory = subcategoryList[0];
            
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                RefreshTechniquesListbox2(judoka, subcategory);
            }
        }

        private void koshiWazaPiechart_DataClick(object sender, ChartPoint chartPoint)
        {
            Subcategory subcategory = subcategoryList[1];
            
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                RefreshTechniquesListbox2(judoka, subcategory);
            }
        }

        private void sutemiWazaPiechart_DataClick(object sender, ChartPoint chartPoint)
        {
            Subcategory subcategory = subcategoryList[3];
            
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                RefreshTechniquesListbox2(judoka, subcategory);
            }
        }

        private void kansetsuWazaPiechart_DataClick(object sender, ChartPoint chartPoint)
        {
            Subcategory subcategory = subcategoryList[6];
            
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                RefreshTechniquesListbox2(judoka, subcategory);
            }
        }

        private void ashiWazaPiechart_DataClick(object sender, ChartPoint chartPoint)
        {
            Subcategory subcategory = subcategoryList[2];
            
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];
                RefreshTechniquesListbox2(judoka, subcategory);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (JudokasListbox.SelectedItem != null)
            {
                editStackpanel1.Visibility = Visibility.Visible;
                editStackpanel2.Visibility = Visibility.Visible;
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];

                string vikt = judoka.Weight.ToString();
                string leng = judoka.Length.ToString();

                JfirstNameTextBox.Text = judoka.FirstName;
                JlastNameTextBox.Text = judoka.LastName;
                WeightTextBox.Text = vikt;
                LengthTextBox.Text = leng;
                
            }
        }

        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            JfirstNameTextBox.Text = "";
            JlastNameTextBox.Text = "";

            editStackpanel1.Visibility = Visibility.Hidden;
            editStackpanel2.Visibility = Visibility.Hidden;
        }

        private void WeightMinusButton_Click(object sender, RoutedEventArgs e)
        {
            int judokaIndex = JudokasListbox.SelectedIndex;
            Judoka judoka = judokaList[judokaIndex];

            judoka.Weight--;
            string vikt = judoka.Weight.ToString();
            WeightTextBox.Text = vikt;

            
            


        }

        // funkar
        private void SaveFirstNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];

                
                string newFname = JfirstNameTextBox.Text;

                databaseConnection.UpdateJudokaFName(judoka.Id, newFname);

                judokas.Clear();
                judokaList.Clear();

                judokas = databaseConnection.GetJudokas();
                judokaList = judokas.Values.ToList();

                databaseConnection.GetLinktable(judokas, techniques);
                databaseConnection.GetTechniqueLevel(judokas, techniques);

                RefreshJudokasListbox();

            }
        }

        // funkar
        private void SaveLastNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];


                string newLname = JlastNameTextBox.Text;

                databaseConnection.UpdateJudokaLName(judoka.Id, newLname);
                //judokas = databaseConnection.GetJudokas();
                //judokaList = judokas.Values.ToList();

                judokas.Clear();
                judokaList.Clear();

                judokas = databaseConnection.GetJudokas();
                judokaList = judokas.Values.ToList();

                databaseConnection.GetLinktable(judokas, techniques);
                databaseConnection.GetTechniqueLevel(judokas, techniques);

                RefreshJudokasListbox();

            }
        }

        private void cancelJudokaCreationButton_Click(object sender, RoutedEventArgs e)
        {
            addJudokaStackpanel.Visibility = Visibility.Hidden;
            addJudokaStackpanel2.Visibility = Visibility.Hidden;
        }

        private void createJudokaButton_Click(object sender, RoutedEventArgs e)
        {
            if (jNewfirstNameTextBox.Text != "" && jNewlastNameTextBox.Text != "")
            {
                string fName = jNewfirstNameTextBox.Text;
                string lName = jNewlastNameTextBox.Text;

                Judoka judoka = databaseConnection.AddJudoka(fName, lName);
                judokas.Add(judoka.Id, judoka);

                judokas.Clear();
                judokaList.Clear();

                judokas = databaseConnection.GetJudokas();
                judokaList = judokas.Values.ToList();

                databaseConnection.GetLinktable(judokas, techniques);
                databaseConnection.GetTechniqueLevel(judokas, techniques);

                RefreshJudokasListbox();
            }
            else
            {
                MessageBox.Show("Somthing is missing");
            }

        }

        // funkar
        private void DeleteJudokaButton_Click(object sender, RoutedEventArgs e)
        {
            if (JudokasListbox.SelectedItem != null)
            {
                int judokaIndex = JudokasListbox.SelectedIndex;
                Judoka judoka = judokaList[judokaIndex];

                databaseConnection.DeleteJudokaInLinktable(judoka.Id);
                databaseConnection.DeleteJudoka(judoka.Id);
                
                
                judokas.Clear();
                judokaList.Clear();

                judokas = databaseConnection.GetJudokas();
                judokaList = judokas.Values.ToList();

                databaseConnection.GetLinktable(judokas, techniques);
                databaseConnection.GetTechniqueLevel(judokas, techniques);

                RefreshJudokasListbox();

            }
        }
    }
}
