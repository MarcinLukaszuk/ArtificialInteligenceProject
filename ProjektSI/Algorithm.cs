using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProjektSI
{
    class Algorithm : INotifyPropertyChanged
    {

        private bool busyMenu;
        private int delay;
        private int koszt;
        private int dlugoscSciezki;
        private int iloscWierszyX;
        private int iloscKolumnY;
        bool FIND = false;

        Pole[,] tablica;
        List<List<Pole>> ListPol;
        BackgroundWorker backgroundWorkerAlg = new BackgroundWorker();


        public bool BusyMenu
        {
            get
            {
                return busyMenu;
            }
            set
            {
                busyMenu = value;
            }
        }


        public Algorithm()
        {

            iloscWierszyX = 50;
            iloscKolumnY = 50;
            delay = 10;
            BusyMenu = false;
        }
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                delay = value;

            }
        }
        [System.ComponentModel.Bindable(true)]
        public int Koszt
        {
            get
            {
                return koszt;
            }
            set
            {
                koszt = value;
                OnPropertyChanged("Koszt");
            }
        }
        public int DlugoscSciezki
        {
            get
            {
                return dlugoscSciezki;
            }
            set
            {
                dlugoscSciezki = value;
            }
        }

        public void DFSExecute(List<List<Pole>> List)
        {
            ListPol = List;

            backgroundWorkerAlg = new BackgroundWorker();
            backgroundWorkerAlg.WorkerSupportsCancellation = true;

            backgroundWorkerAlg.DoWork += (sender, args) =>
            {
                var thisWorker = sender as BackgroundWorker;
                var _child = new Thread(() =>
                {
                    DFSAlgorithm(ListPol);
                });
                _child.Start();
                while (_child.IsAlive)
                {
                    if (thisWorker.CancellationPending)
                    {
                        _child.Abort();
                        args.Cancel = true;
                    }
                    Thread.SpinWait(1);
                }
            };
            backgroundWorkerAlg.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    MessageBox.Show(args.Error.ToString());
                BusyMenu = false;
                CommandManager.InvalidateRequerySuggested();
            };
            backgroundWorkerAlg.RunWorkerAsync();




        }
        public void BFSExecute(List<List<Pole>> List)
        {
            ListPol = List;

            backgroundWorkerAlg = new BackgroundWorker();
            backgroundWorkerAlg.WorkerSupportsCancellation = true;

            backgroundWorkerAlg.DoWork += (sender, args) =>
            {
                var thisWorker = sender as BackgroundWorker;
                var _child = new Thread(() =>
                {
                    BFSAlgoritm();
                });
                _child.Start();
                while (_child.IsAlive)
                {
                    if (thisWorker.CancellationPending)
                    {
                        _child.Abort();
                        args.Cancel = true;
                    }
                    Thread.SpinWait(1);
                }
            };
            backgroundWorkerAlg.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    MessageBox.Show(args.Error.ToString());
                BusyMenu = false;
                CommandManager.InvalidateRequerySuggested();
            };
            backgroundWorkerAlg.RunWorkerAsync();
        }
        public void BestFSExecute(List<List<Pole>> List)
        {
            ListPol = List;
            backgroundWorkerAlg = new BackgroundWorker();
            backgroundWorkerAlg.WorkerSupportsCancellation = true;

            backgroundWorkerAlg.DoWork += (sender, args) =>
            {
                var thisWorker = sender as BackgroundWorker;
                var _child = new Thread(() =>
                {
                    BestFSAlgoritm();
                });
                _child.Start();
                while (_child.IsAlive)
                {
                    if (thisWorker.CancellationPending)
                    {
                        _child.Abort();
                        args.Cancel = true;
                    }
                    Thread.SpinWait(1);
                }
            };
            backgroundWorkerAlg.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    MessageBox.Show(args.Error.ToString());
                BusyMenu = false;
                CommandManager.InvalidateRequerySuggested();

            };
            backgroundWorkerAlg.RunWorkerAsync();
        }
        public void stopAlgorithm()
        {
            backgroundWorkerAlg.CancelAsync();
            BusyMenu = false;
        }


        public void DFSExecuteNoThread(List<List<Pole>> List)
        {
            ListPol = List;
            DFSAlgorithm(ListPol);
        }
        public void BFSExecuteNoThread(List<List<Pole>> List)
        {
            ListPol = List;
            BFSAlgoritm();
        }
        public void BestFSExecuteNoThread(List<List<Pole>> List)
        {
            ListPol = List;
            BestFSAlgoritm();
        }


      

        private Pole GetStartNode(Pole[,] tab)
        {
            for (int i = 0; i < iloscWierszyX; i++)
            {
                for (int j = 0; j < iloscKolumnY; j++)
                {
                    if (tab[i, j].TypSciany == Block.Start)
                    {
                        return tab[i, j];
                    }
                }
            }

            return null;
        }
        private Pole GetEndNode(Pole[,] tab)
        {
            for (int i = 0; i < iloscWierszyX; i++)
            {
                for (int j = 0; j < iloscKolumnY; j++)
                {
                    if (tab[i, j].TypSciany == Block.End)
                    {
                        return tab[i, j];
                    }
                }
            }

            return null;
        }
        private Pole[,] ConvertListsToArray(List<List<Pole>> listaPole)
        {
            Pole[,] tablica = new Pole[iloscWierszyX, iloscKolumnY];
            int x = 0, y = 0;
            foreach (List<Pole> wiersz in listaPole)
            {
                foreach (Pole komorka in wiersz)
                {
                    tablica[x, y] = komorka;
                    y++;
                }
                x++;
                y = 0;
            }
            return tablica;
        }
        public void DFSAlgorithm(List<List<Pole>> List)
        {
            ListPol = List;
            FIND = false;
            DlugoscSciezki = 0;
            Koszt = 0;


            tablica = ConvertListsToArray(ListPol);
            Pole start;
            Pole end;
            Pole firstStackNode = new Pole();
            Pole tmp = new Pole();
            if ((start = GetStartNode(tablica)) == null)
            {
                MessageBox.Show("The START node doesn't exist");
                return;

            }
            else if ((end = GetEndNode(tablica)) == null)
            {
                MessageBox.Show("The End node doesn't exist");
                return;
            }
            DFSRecirsive(start);

        }
        public int DFSRecirsive(Pole obecny)
        {
            Pole tmp;
            int wynik = 0;
            if (FIND == true)
            {
                return wynik;
            }


            if (obecny.TypSciany != Block.Start)
            {
                obecny.TypSciany = Block.Special;
                Thread.Sleep(Delay);
                obecny.TypSciany = Block.Visited;
                Koszt++;

            }


            if (obecny.YPosition != 0)
            {
                tmp = tablica[obecny.YPosition - 1, obecny.XPosition];
                if (tmp.TypSciany == Block.Hall)
                    wynik += DFSRecirsive(tmp);

                if (tmp.TypSciany == Block.End)
                {
                    DlugoscSciezki++;
                    obecny.TypSciany = Block.Special;
                    FIND = true;
                    return 1;
                }


            }


            if (obecny.XPosition != 0)
            {
                tmp = tablica[obecny.YPosition, obecny.XPosition - 1];
                if (tmp.TypSciany == Block.Hall)
                    wynik += DFSRecirsive(tmp);
                if (tmp.TypSciany == Block.End)
                {
                    DlugoscSciezki++;
                    obecny.TypSciany = Block.Special;
                    FIND = true;
                    return 1;
                }
            }



            if (obecny.YPosition != iloscKolumnY - 1)
            {
                tmp = tablica[obecny.YPosition + 1, obecny.XPosition];
                if (tmp.TypSciany == Block.Hall)
                    wynik += DFSRecirsive(tmp);
                if (tmp.TypSciany == Block.End)
                {
                    DlugoscSciezki++;
                    obecny.TypSciany = Block.Special;
                    FIND = true;
                    return 1;
                }
            }
            if (obecny.XPosition != iloscWierszyX - 1)
            {
                tmp = tablica[obecny.YPosition, obecny.XPosition + 1];
                if (tmp.TypSciany == Block.Hall)
                    wynik += DFSRecirsive(tmp);
                if (tmp.TypSciany == Block.End)
                {
                    DlugoscSciezki++;
                    obecny.TypSciany = Block.Special;
                    FIND = true;
                    return 1;
                }
            }

            if (wynik != 0 && obecny.TypSciany != Block.Start)
            {
                DlugoscSciezki++;
                obecny.TypSciany = Block.Special;
            }


            return wynik;
        }
        private int BFSAlgoritm()
        {

            tablica = ConvertListsToArray(ListPol);
            LinkedList<Pole> queue = new LinkedList<Pole>();
            Pole start;
            Pole end;
            Pole firstStackNode = new Pole();
            Pole tmp = new Pole();
            bool[,] visitedTable = new bool[50, 50];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    visitedTable[i, j] = false;
                }
            }
            Koszt = 0;
            if ((start = GetStartNode(tablica)) == null)
            {
                MessageBox.Show("The START node doesn't exist");
                return -1;
            }
            else if ((end = GetEndNode(tablica)) == null)
            {
                MessageBox.Show("The End node doesn't exist");
                return -1;
            }

            queue.AddFirst(start);
            NodesTree drzewo = new NodesTree(start);
            while (queue.Count != 0)
            {

                firstStackNode = queue.First();
                queue.RemoveFirst();


                if (firstStackNode.TypSciany != Block.Start)
                {
                    Koszt++;
                    firstStackNode.TypSciany = Block.Special;
                    Thread.Sleep(Delay);
                    firstStackNode.TypSciany = Block.Visited;
                }





                if (firstStackNode.YPosition != 0)
                {
                    tmp = tablica[firstStackNode.YPosition - 1, firstStackNode.XPosition];

                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tablica[firstStackNode.YPosition - 1, firstStackNode.XPosition].TypSciany == Block.Hall)
                    {
                        queue.AddLast(tablica[firstStackNode.YPosition - 1, firstStackNode.XPosition]);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tablica[firstStackNode.YPosition - 1, firstStackNode.XPosition].TypSciany == Block.End)
                    {

                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }
                if (firstStackNode.XPosition != 0)
                {
                    tmp = tablica[firstStackNode.YPosition, firstStackNode.XPosition - 1];
                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tmp.TypSciany == Block.Hall)
                    {
                        queue.AddLast(tmp);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tmp.TypSciany == Block.End)
                    {
                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }
                if (firstStackNode.YPosition != iloscKolumnY - 1)
                {
                    tmp = tablica[firstStackNode.YPosition + 1, firstStackNode.XPosition];
                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tmp.TypSciany == Block.Hall)
                    {
                        queue.AddLast(tmp);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tmp.TypSciany == Block.End)
                    {

                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }
                if (firstStackNode.XPosition != iloscWierszyX - 1)
                {
                    tmp = tablica[firstStackNode.YPosition, firstStackNode.XPosition + 1];
                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tmp.TypSciany == Block.Hall)
                    {
                        queue.AddLast(tmp);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tmp.TypSciany == Block.End)
                    {
                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }






            }
            return -1;
        }
        private int BestFSAlgoritm()
        {

            tablica = ConvertListsToArray(ListPol);
            TupleList<int, Pole> priorityQueue = new TupleList<int, Pole>();


            bool[,] visitedTable = new bool[50, 50];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    visitedTable[i, j] = false;
                }
            }

            Pole start;
            Pole end;
            Pole firstStackNode = new Pole();
            Pole tmp = new Pole();
            Koszt = 0;
            if ((start = GetStartNode(tablica)) == null)
            {
                MessageBox.Show("The START node doesn't exist");
                return -1;
            }
            else if ((end = GetEndNode(tablica)) == null)
            {
                MessageBox.Show("The End node doesn't exist");
                return -1;
            }



            NodesTree drzewo = new NodesTree(start);



            priorityQueue.Add(BestFirstFunction(start, end), start);
            visitedTable[start.XPosition, start.YPosition] = true;

            while (priorityQueue.Count != 0)
            {
                priorityQueue.Sort();
                firstStackNode = priorityQueue.First().Item2;
                priorityQueue.RemoveAt(0);
                if (firstStackNode.TypSciany != Block.Start)
                {
                    Koszt++;
                    firstStackNode.TypSciany = Block.Special;
                    Thread.Sleep(Delay);
                    firstStackNode.TypSciany = Block.Visited;
                }


                if (firstStackNode.YPosition != 0)
                {
                    tmp = tablica[firstStackNode.YPosition - 1, firstStackNode.XPosition];

                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tmp.TypSciany == Block.Hall)
                    {
                        priorityQueue.Add(BestFirstFunction(tmp, end), tmp);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tmp.TypSciany == Block.End)
                    {
                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }
                if (firstStackNode.XPosition != 0)
                {
                    tmp = tablica[firstStackNode.YPosition, firstStackNode.XPosition - 1];
                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tmp.TypSciany == Block.Hall)
                    {
                        priorityQueue.Add(BestFirstFunction(tmp, end), tmp);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tmp.TypSciany == Block.End)
                    {
                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }
                if (firstStackNode.YPosition != iloscKolumnY - 1)
                {
                    tmp = tablica[firstStackNode.YPosition + 1, firstStackNode.XPosition];
                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tmp.TypSciany == Block.Hall)
                    {
                        priorityQueue.Add(BestFirstFunction(tmp, end), tmp);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tmp.TypSciany == Block.End)
                    {
                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }
                if (firstStackNode.XPosition != iloscWierszyX - 1)
                {
                    tmp = tablica[firstStackNode.YPosition, firstStackNode.XPosition + 1];
                    if (visitedTable[tmp.XPosition, tmp.YPosition] == true) { }
                    else if (tmp.TypSciany == Block.Hall)
                    {
                        priorityQueue.Add(BestFirstFunction(tmp, end), tmp);
                        visitedTable[tmp.XPosition, tmp.YPosition] = true;
                        drzewo.addChild(firstStackNode, tmp);
                    }
                    else if (tmp.TypSciany == Block.End)
                    {
                        DlugoscSciezki = drzewo.ReturnWay(firstStackNode).Count;
                        drzewo.ReturnWay(firstStackNode).ForEach(x => x.TypSciany = Block.Special);
                        return Koszt;
                    }
                }

            }
            return -1;
        }
        private int BestFirstFunction(Pole actial, Pole end)
        {
            int szerokosc = Math.Abs(actial.XPosition - end.XPosition);
            int wysokosc = Math.Abs(actial.YPosition - end.YPosition);
            return (int)(Math.Pow(wysokosc, 2) + Math.Pow(szerokosc, 2));
        }



        public event PropertyChangedEventHandler PropertyChanged;
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
