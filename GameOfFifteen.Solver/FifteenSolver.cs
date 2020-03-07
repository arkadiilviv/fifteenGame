using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Medallion.Collections;
namespace GameOfFifteen.Solver
{
    public class FifteenSolver
    {
        Fifteen initial;
        List<Fifteen> result = new List<Fifteen>();
        public class Item
        {
            public Item prevFifteen;
            public Fifteen fifteen;
            public Item(Item prevFifteen, Fifteen fifteen)
            {
                this.prevFifteen = prevFifteen;
                this.fifteen = fifteen;
            }

            public Fifteen GetFifteen()
            {
                return fifteen;
            }




        }
        class ItemComp<T> : IComparer<T> where T : Item
        {
            public int Compare(T x, T y)
            {
                return (measure(x).CompareTo(measure(y)));

            }
        }
        private static int measure(Item item)
        {
            Item item2 = item;
            int c = 0;   // g(x)
            int measure = item.GetFifteen().h;  // h(x)
            while (true)
            {
                c++;
                item2 = item2.prevFifteen;
                if (item2 == null)
                {
                    return measure + c;
                }
            }
        }
        public FifteenSolver(Fifteen initial,string path)
        {
            try
            {
                String input = File.ReadAllText(@path);

                int i = 0, j = 0;
                int[,] fileOut = new int[4, 4];
                foreach (var row in input.Split('\n'))
                {
                    j = 0;
                    foreach (var col in row.Split(','))
                    {
                        if(!String.IsNullOrEmpty(col))
                            fileOut[i, j] = int.Parse(col);
                        j++;
                    }
                    i++;
                }
                initial.fifteen = fileOut;
                this.initial = initial;
                ItemComp<Item> comparer = new ItemComp<Item>();
                PriorityQueue<Item> priorityQueue = new PriorityQueue<Item>(10, comparer);
                priorityQueue.Enqueue(new Item(null, initial));
                while (true)
                {
                    Item fifteen = priorityQueue.Dequeue();
                    if (fifteen.fifteen.isGoal())
                    {
                        itemToList(new Item(fifteen, fifteen.fifteen));
                        return;
                    }

                    foreach (var item in fifteen.fifteen.Neighbors())
                    {
                        Fifteen tempFif = item;
                        if (item != null && !containsInPath(fifteen, tempFif))
                            priorityQueue.Enqueue(new Item(fifteen, tempFif));
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {


            }

        }
        
        public List<Fifteen> solution(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (Fifteen item in result)
                {
                    int[,] fifteen = item.fifteen;
                    for (int col = 0; col < fifteen.GetLength(0); col++)
                    {
                        string rowLine = "";
                        for (int row = 0; row < fifteen.GetLength(1); row++)
                        {
                            if (row == (fifteen.GetLength(1) - 1))
                                rowLine += fifteen[col, row];
                            else
                                rowLine += fifteen[col, row] + ",";

                        }
                        sw.WriteLine(rowLine);
                    }
                    sw.WriteLine("\n");
                }
            }
            return result;
        }

        private void itemToList(Item item)
        {
            Item item2 = item;
            while (true)
            {
                item2 = item2.prevFifteen;
                if (item2 == null)
                {
                    result.Reverse();
                    return;
                }
                result.Add(item2.fifteen);
            }
        }
        private bool containsInPath(Item item, Fifteen fifteen)
        {
            Item item2 = item;
            while (true)
            {
                if (item2.fifteen.Equals(fifteen)) return true;
                item2 = item2.prevFifteen;
                if (item2 == null) return false;
            }
        }


    }
}
