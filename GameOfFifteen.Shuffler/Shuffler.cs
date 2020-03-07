using System;
using System.Collections.Generic;
using System.IO;
namespace GameOfFifteen.Shuffler
{
    public class Shuffler
    {
        struct Position
        {
            public int col;
            public int row;
            public bool IsValide()
            {
                return col >= 0;
            }
            public Position Top()
            {
                return new Position() { col = this.col - 1, row = this.row };
            }
            public Position Right()
            {
                return new Position() { col = this.col, row = this.row + 1 };
            }
            public Position Bottom()
            {
                return new Position() { col = this.col + 1, row = this.row };
            }
            public Position Left()
            {
                return new Position() { col = this.col, row = this.row - 1 };
            }

        }
        public int[,] Shuffe(int steps, int[,] fifteenOut,string path)
        {
            int[,] fifteen = fifteenOut;
            Random random = new Random();
            for (int i = 0; i < steps; i++)
            {
                Position pos = GetPosition(0, fifteen);
                if (pos.IsValide())
                {
                    int[] moves = GetPossibleMoves(pos, fifteen);
                    int moveIn = random.Next(0, moves.Length);
                    switch (moves[moveIn])
                    {
                        case 0:
                            fifteen = Swap(pos, pos.Top(), fifteen);
                            break;
                        case 1:
                            fifteen = Swap(pos, pos.Right(), fifteen);
                            break;
                        case 2:
                            fifteen = Swap(pos, pos.Bottom(), fifteen);
                            break;
                        case 3:
                            fifteen = Swap(pos, pos.Left(), fifteen);
                            break;

                        default:
                            break;
                    }
                }
                else
                    throw new Exception("Something go wrong");
            }
            WriteToFile(path, fifteen);
            return fifteen;
        }
        private void WriteToFile(string path, int[,] fifteen)
        {

            using (StreamWriter sw = new StreamWriter(path))
            {
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
                    if(!String.IsNullOrEmpty(rowLine))
                        sw.WriteLine(rowLine);
                }
            }
        }
        private int GetFieldValue(int col, int row, int[,] fifteen)
        {
            int colSize = fifteen.GetLength(0);
            int rowSize = fifteen.GetLength(1);
            if (col >= 0 && col < colSize)
            {
                if (row >= 0 && row < rowSize)
                    return fifteen[col, row];
                else
                    return -1;
            }
            else
                return -1;
        }
        private int[,] Swap(Position current, Position target, int[,] fifteen)
        {
            int temp = fifteen[current.col, current.row];
            fifteen[current.col, current.row] = fifteen[target.col, target.row];
            fifteen[target.col, target.row] = temp;
            return fifteen;
        }
        private Position GetPosition(int targetValue, int[,] fifteen)
        {
            for (int col = 0; col < fifteen.GetLength(0); col++)
            {
                for (int row = 0; row < fifteen.GetLength(1); row++)
                {
                    if (fifteen[col, row] == targetValue)
                        return new Position() { col = col, row = row };
                }
            }
            return new Position() { col = -1, row = -1 };
        }

        private int[] GetPossibleMoves(Position pos, int[,] fifteen)
        {
            List<Int32> moves = new List<int>();
            if (GetFieldValue(pos.col - 1, pos.row, fifteen) > 0) // Top
                moves.Add(0);
            if (GetFieldValue(pos.col, pos.row + 1, fifteen) > 0) // Right
                moves.Add(1);
            if (GetFieldValue(pos.col + 1, pos.row, fifteen) > 0) // Bottom
                moves.Add(2);
            if (GetFieldValue(pos.col, pos.row - 1, fifteen) > 0) // Left
                moves.Add(3);

            return moves.ToArray();
        }
    }
}
