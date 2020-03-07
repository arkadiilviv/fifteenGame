using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameOfFifteen.Shuffler;
using GameOfFifteen.Solver;

namespace GameOfFifteen
{
    public partial class Form1 : Form
    {
        int[,] fifteen = new int[,] { {1, 2, 3, 4, },
                                      {5, 6, 7, 8 },
                                      {9, 10, 11, 12 },
                                      {13, 14, 15, 0 }};
        int[,] solution = new int[,] { {1, 2, 3, 4, },
                                      {5, 6, 7, 8 },
                                      {9, 10, 11, 12 },
                                      {13, 14, 15, 0 }};
        //int[,] fifteen = new int[,] { { 1, 2, 3}, { 4, 5, 6 },{ 7, 8, 0 } };
        //int[,] fifteen = new int[,] { { 1, 2, },{ 3, 0 } };

        Shuffler.Shuffler shuffler;
        List<Fifteen> result;
        SaveFileDialog save;
        OpenFileDialog open;
        decimal speed = 1;
        NumericUpDown speedNumeric;
        public Form1()
        {
            InitializeComponent();
            save = new SaveFileDialog();
            open = new OpenFileDialog();
            open.Filter = "txt files (*.txt)|*.txt";
            save.DefaultExt = "txt";
            save.Filter = "txt files (*.txt)|*.txt";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawField();
            this.Width = 225;
            this.Height = 300;

        }

        private void DrawField()
        {
            this.Controls.Clear();
            for (int col = 0; col < fifteen.GetLength(0); col++)
            {
                for (int row = 0; row < fifteen.GetLength(1); row++)
                {
                    Button button = new Button();
                    if (fifteen[col, row] == 0)
                    {

                        button.Visible = false;

                    }
                    button.FlatStyle = FlatStyle.System;
                    button.Text = fifteen[col, row].ToString();
                    button.Width = 50;
                    button.Height = 50;
                    button.Left = 50 * row;
                    button.Top = 50 * col;
                    button.Click += ButtonClick;
                    this.Controls.Add(button);
                }
            }
            Button buttonShuffle = new Button();
            buttonShuffle.Text = "Shuffle";
            buttonShuffle.Top = 50 * (fifteen.GetLength(0));
            buttonShuffle.Width = 100;
            buttonShuffle.Click += ButtonSuffle;

            Button buttonSolve = new Button();
            buttonSolve.Text = "Solve";
            buttonSolve.Top = 50 * (fifteen.GetLength(0));
            buttonSolve.Left = 100;
            buttonSolve.Width = 100;
            buttonSolve.Click += ButtonSolve;

            this.Controls.Add(buttonShuffle);
            this.Controls.Add(buttonSolve);

            speedNumeric = new NumericUpDown();
            speedNumeric.Top = 50 * (fifteen.GetLength(0)) + 25;
            speedNumeric.Width = 100;
            speedNumeric.Value = speed;
            speedNumeric.Maximum = 5;
            speedNumeric.Minimum = 0.25M;
            this.Controls.Add(speedNumeric);

            Button buttonPlay = new Button();
            buttonPlay.Top = 50 * (fifteen.GetLength(0)) + 25;
            buttonPlay.Left = 100;
            buttonPlay.Width = 100;
            buttonPlay.Click += ButtonPlay;
            buttonPlay.Text = "Play";
            this.Controls.Add(buttonPlay);
        }

        private void ButtonClick(object sender, EventArgs e)
        {

            int val = Convert.ToInt32(((Button)sender).Text);
            if (val != 0)
            {
                for (int col = 0; col < fifteen.GetLength(0); col++)
                {
                    for (int row = 0; row < fifteen.GetLength(1); row++)
                    {
                        if (fifteen[col, row] == val) // Our position
                        {
                            if (GetFieldValue(col - 1, row) == 0) // Top
                                Swap(col, row, col - 1, row);
                            else if (GetFieldValue(col, row + 1) == 0) // Right
                                Swap(col, row, col, row + 1);
                            else if (GetFieldValue(col + 1, row) == 0) // Bottom
                                Swap(col, row, col + 1, row);
                            else if (GetFieldValue(col, row - 1) == 0) // Left
                                Swap(col, row, col, row - 1);

                            return;
                        }
                    }
                }
            }
        }
        private async void ButtonSolve(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
                control.Enabled = false;
            open.ShowDialog();
            save.ShowDialog();
            if (save.FileName != null && open.FileName != null)
            {
             
                await Task.Run(() => Solve());

                
            }
            foreach (Control control in this.Controls)
                control.Enabled = true;
        }
        private async Task Solve()
        {
            FifteenSolver fifteenSolver = new FifteenSolver(new Fifteen(fifteen, solution), open.FileName);
            result = fifteenSolver.solution(save.FileName);
            result.ToArray();
            
        }
        private void ButtonSuffle(object sender, EventArgs e)
        {
         
            save.ShowDialog();
            if (save.FileName != null)
            {
                shuffler = new Shuffler.Shuffler();
                fifteen = shuffler.Shuffe(15, fifteen, save.FileName);
                DrawField();
            }
        }
        private async void ButtonPlay(object sender, EventArgs e)
        {
            speed = speedNumeric.Value;
            foreach (var item in result)
            {
                fifteen = item.fifteen;
                await Task.Delay((Int32)(speed * 1000));
                DrawField();
            }
        }

        private int GetFieldValue(int col, int row)
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
        private void Swap(int currentCol, int currentRow, int targerCol, int targetRow)
        {
            int temp = fifteen[currentCol, currentRow];
            fifteen[currentCol, currentRow] = fifteen[targerCol, targetRow];
            fifteen[targerCol, targetRow] = temp;
            DrawField();
        }
    }
}
