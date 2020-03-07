using System;
using System.Collections.Generic;

namespace GameOfFifteen.Solver
{
    public class Fifteen
    {
        public int[,] fifteen;
        public int zeroRow { get; }
        public int zeroCol { get; }
        public int h { get; }
        int[,] result;
        public bool isGoal()
        {
            for (int col = 0; col < fifteen.GetLength(0); col++)
            {
                for (int row = 0; row < fifteen.GetLength(1); row++)
                {
                    if (fifteen[col, row] != result[col, row])
                        return false;
                }
            }
            return true;
        }
        public Fifteen(int[,] fifteen,int[,] result)
        {
            this.fifteen = fifteen;
            this.result = result;
            h = 0;
            for (int col = 0; col < fifteen.GetLength(0); col++)
            {
                
                for (int row = 0; row < fifteen.GetLength(1); row++)
                {
                    int temp = (col * fifteen.Rank + row + 1);
                    if (fifteen[col, row] != 0 && fifteen[col, row] != temp)
                        h += 1;
                    if (fifteen[col, row] == 0)
                    {
                        zeroCol = col;
                        zeroRow = row;
                    
                    }
                }
            }
        }
        public HashSet<Fifteen> Neighbors()
        {
            HashSet<Fifteen> fifteenList = new HashSet<Fifteen>();

            fifteenList.Add(Swap(fifteen, zeroRow, zeroCol, zeroRow, zeroCol + 1));
            fifteenList.Add(Swap(fifteen, zeroRow, zeroCol, zeroRow, zeroCol - 1));
            fifteenList.Add(Swap(fifteen, zeroRow, zeroCol, zeroRow - 1, zeroCol));
            fifteenList.Add(Swap(fifteen, zeroRow, zeroCol, zeroRow + 1, zeroCol));

            return fifteenList;

        }

        Fifteen Swap(int[,] fifteenFrom, int zeroRow1, int zeroCol1, int zeroRow2, int zeroCol2)
        {
            int[,] tempFifteen = ((int[,])fifteenFrom.Clone());
            int colSize = tempFifteen.GetLength(0);
            int rowSize = tempFifteen.GetLength(1);
            if (zeroCol2 >= 0 && zeroCol2 < colSize && zeroRow2 >= 0 && zeroRow2 < rowSize)
            {
                int temp = tempFifteen[zeroCol2, zeroRow2];
                tempFifteen[zeroCol2, zeroRow2] = fifteenFrom[zeroCol1, zeroRow1];
                tempFifteen[zeroCol1, zeroRow1] = temp;

                return new Fifteen(tempFifteen,result);
            }
            else
                return null;
        }
        public override bool Equals(object obj)
        {
            int[,] fifteenToEq = ((Fifteen)obj).fifteen;
            for (int col = 0; col < fifteen.GetLength(0); col++)
            {
                for (int row = 0; row < fifteen.GetLength(1); row++)
                {
                    if (fifteen[col, row] != fifteenToEq[col, row])
                        return false;
                }
            }
            return true;

        }
    }
}
