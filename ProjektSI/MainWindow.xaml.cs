
//-----------------------------------------------------------------------
// <copyright>
//     Created by Marcin Łukaszu and Ewelina Ruchlewicz
// </copyright>
//-----------------------------------------------------------------------
namespace ProjektSI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Shapes;
    using System.Windows.Controls;
    using Microsoft.Win32;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization;
    public enum Block
    {
        Wall = 1, Hall, Start, End, Visited, Special
    }

    [Serializable]
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        List<List<Pole>> ListPol;
        private int iloscWierszyX;
        private int iloscKolumnY;
        private double widthRectangle;
        private double heightRectangle;
        private int globalCounter;
        BackgroundWorker backgroundWorkerAlg = new BackgroundWorker();
        Algorithm algorytm = new Algorithm();

        public event PropertyChangedEventHandler PropertyChanged;
        
        #region Properies
        public double HeightRectangle
        {
            get
            {
                return heightRectangle;
            }
            set
            {
                heightRectangle = value;
                OnPropertyChanged("HeightRectangle");
            }
        }
        public double WidthRectangle
        {
            get
            {
                return widthRectangle;
            }
            set
            {
                widthRectangle = value;
                OnPropertyChanged("WidthRectangle");
            }
        }
        public int Delay
        {
            get
            {
                return algorytm.Delay;
            }
            set
            {
                algorytm.Delay = value;
                OnPropertyChanged("Delay");
            }
        }
        public int GlobalCounter
        {
            get
            {
                return globalCounter;
            }
            set
            {
                globalCounter = value;
                OnPropertyChanged("GlobalCounter");
            }
        }
        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            DataContext = this;
            iloscWierszyX = 50;
            iloscKolumnY = 50;
            Delay = 10;
            MakeEmptyLabirynth();
            GlobalCounter = 0;

            algorytm = new Algorithm();
        }
        
        #region FunkcjeZmieniającePlansze
        private void CleanLab()
        {
            foreach (List<Pole> wiersz in ListPol)
            {
                foreach (Pole komorka in wiersz)
                {
                    if (komorka.TypSciany == Block.Visited || komorka.TypSciany == Block.Special)
                    {
                        komorka.TypSciany = Block.Hall;
                    }
                }

            }
            GlobalCounter = 0;
        }
        private void InvertLab()
        {
            foreach (List<Pole> wiersz in ListPol)
            {
                foreach (Pole komorka in wiersz)
                {
                    if (komorka.TypSciany == Block.Hall)
                    {
                        komorka.TypSciany = Block.Wall;
                    }
                    else if (komorka.TypSciany == Block.Wall)
                    {
                        komorka.TypSciany = Block.Hall;
                    }
                }

            }
        }
        private void ResetLab()
        {
            foreach (List<Pole> wiersz in ListPol)
            {
                foreach (Pole komorka in wiersz)
                {
                    komorka.TypSciany = Block.Wall;

                }

            }
        }
        private void MakeEmptyLabirynth()
        {
            ListPol = new List<List<Pole>>();
            for (int i = 0; i < iloscWierszyX; i++)
            {
                ListPol.Add(new List<Pole>());

                for (int j = 0; j < iloscKolumnY; j++)
                {
                    ListPol[i].Add(new Pole(Block.Wall, j, i));
                }
            }
            lst.ItemsSource = ListPol;
            Focus();
        }
        private void WczytajMapeFunc()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Dat Format|*.dat";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    ListPol = (List<List<Pole>>)formatter.Deserialize(fs);
                    lst.ItemsSource = ListPol;
                    Focus();
                }
                catch (SerializationException e)
                {
                    MessageBox.Show("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }

        }
        private void ZapiszMapeFunc()
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Dat Format|*.dat";
            saveFileDialog1.Title = "Save Lab";
            saveFileDialog1.ShowDialog();


            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, ListPol);
                }
                catch (SerializationException e)
                {
                    MessageBox.Show("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }







        }
        #endregion

        #region FunkcjeAlgorytmów





        public void StartAlghoritmAsync()
        {
            CleanLab();
            var tmp = spRadioButton
                .Children
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.IsChecked == true)
                .Single()
                .Tag
                .ToString();
            algorytm.BusyMenu = true;
            if (tmp.Equals("DFS"))
            {
                algorytm.DFSExecute(ListPol);
            }
            else if (tmp.Equals("BEST"))
            {
                algorytm.BestFSExecute(ListPol);
            }
            else
            {
                algorytm.BFSExecute(ListPol);
            }
          
        }
        public void StopAlghoritmAsync()
        {
            algorytm.stopAlgorithm();
        }
        public void SUMMARY()
        {


            int DFSKoszt = -1;
            int BFSKoszt = -1;
            int BESTKoszt = -1;



            int DFSSciezka = -1;
            int BFSSciezka = -1;
            int BESTSciezka = -1;

            int tmpDelay = Delay;

            Delay = 0;


            CleanLab();
            algorytm.DFSExecuteNoThread(ListPol);
            DFSKoszt = algorytm.Koszt;
            DFSSciezka = algorytm.DlugoscSciezki;
            CleanLab();
            if (DFSSciezka == 0)
            {
                Delay = tmpDelay;
                MessageBox.Show("Brak znalezionej ściezki");
                return;

            }

            algorytm.BestFSExecuteNoThread(ListPol);
            BESTKoszt = algorytm.Koszt;
            BESTSciezka = algorytm.DlugoscSciezki;
            CleanLab();

            algorytm.BFSExecuteNoThread(ListPol);
            BFSKoszt = algorytm.Koszt;
            BFSSciezka = algorytm.DlugoscSciezki;
            CleanLab();

            SubmitWindow submit = new SubmitWindow();
            submit.tbBFSKoszt.Text = BFSKoszt.ToString();
            submit.tbDFSKoszt.Text = DFSKoszt.ToString();
            submit.tbBestKoszt.Text = BESTKoszt.ToString();

            submit.tbBFSPath.Text = BFSSciezka.ToString();
            submit.tbDFSPath.Text = DFSSciezka.ToString();
            submit.tbBestPath.Text = BESTSciezka.ToString();

            submit.ShowDialog();
            Delay = tmpDelay;


        }
        #endregion

        #region CommandsStuff

        private void SaveLabyrinthExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ZapiszMapeFunc();
        }
        private void SummaryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SUMMARY();
        }
        private void LoadLabyrinthExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WczytajMapeFunc();
        }
        private void StopAlgorithmExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StopAlghoritmAsync();
        }
        private void StartAlgorithmExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StartAlghoritmAsync();
        }
        private void CleanLabyrinthExecute(object sender, ExecutedRoutedEventArgs e)
        {
            CleanLab();
        }
        private void ResetLabyrinthExecute(object sender, ExecutedRoutedEventArgs e)
        {
            ResetLab();
        }
        public void SetStartNodeExecute(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var ListPol2 in ListPol)
            {
                foreach (var cell in ListPol2)
                {
                    if (cell.TypSciany == Block.Start)
                    {
                        cell.TypSciany = Block.Wall;
                    }
                }
            }
            Pole pl = (Pole)e.Parameter;
            pl.TypSciany = Block.Start;
        }
        public void SetEndNodeExecute(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var ListPol2 in ListPol)
            {
                foreach (var cell in ListPol2)
                {
                    if (cell.TypSciany == Block.End)
                    {
                        cell.TypSciany = Block.Wall;
                    }


                }
            }
            Pole pl = (Pole)e.Parameter;
            pl.TypSciany = Block.End;
        }
        private void InvertLabExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            InvertLab();

        }
        public void CanExecuteSet(object sender, CanExecuteRoutedEventArgs e)
        {
            if (algorytm.BusyMenu == false)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

        }
        public void CanExecuteStop(object sender, CanExecuteRoutedEventArgs e)
        {
            if (algorytm.BusyMenu != false)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

        }
        #endregion

        #region Events

        private void ZmianaRozdzialki(object sender, SizeChangedEventArgs e)
        {
            WidthRectangle = lst.ActualWidth / iloscKolumnY;
            HeightRectangle = lst.ActualHeight / iloscWierszyX;
        }
        private void MousePress(object sender, MouseEventArgs e)
        {
            Pole pl = (Pole)((Rectangle)sender).Tag;
            if (Mouse.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                pl.TypSciany = Block.Wall;
            }
            else if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                pl.TypSciany = Block.Hall;
            }
        }
        private void ClosingEvent(object sender, CancelEventArgs e)
        {
            if (backgroundWorkerAlg.IsBusy)
            {
                backgroundWorkerAlg.CancelAsync();
            }

        }

        #endregion
        
        protected void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
