using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class SudokuGameForm : Form
    {
        PuzzleGenerator puzzleGenerator = new PuzzleGenerator();
        SudokuCell[,] cells = new SudokuCell[9, 9];


        public GameDifficulty GameDifficulty = GameDifficulty.Medium; // Default game difficulty
        public GameMode GameMode = GameMode.Stopwatch;
        public int TimeInSeconds = 0;
        public SudokuGameForm(GameDifficulty gameDifficulty, GameMode gameMode, int timeInSeconds)
        {
            InitializeComponent();

            CreateCells();
            GameDifficulty = gameDifficulty;
            GameMode = gameMode;
            TimeInSeconds = timeInSeconds;

            timer1.Interval = 1000;
            timer1.Start();
            InitGame(GameDifficulty);
        }

        private void CreateCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Create 81 cells for with styles and locations based on the index
                    cells[i, j] = new SudokuCell();
                    cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 18);
                    cells[i, j].Size = new Size(50, 50);
                    cells[i, j].ForeColor = SystemColors.ControlDarkDark;
                    cells[i, j].Location = new Point(i * 50, j * 50);
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                    cells[i, j].FlatStyle = FlatStyle.Flat;
                    cells[i, j].FlatAppearance.BorderColor = Color.Black;
                    cells[i, j].X = i;
                    cells[i, j].Y = j;

                    // Assign key press event for each cells
                    cells[i, j].KeyPress += cell_KeyPressed;
                    cells[i, j].MouseClick += cell_MouseClick;

                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }

        private void InitGame(GameDifficulty gameDifficulty)
        {
            puzzleGenerator = new PuzzleGenerator();
            puzzleGenerator.generateGrids((int)gameDifficulty);
            FillCells();
        }

        private void cell_KeyPressed(object sender, KeyPressEventArgs e)
        {
            var cell = sender as SudokuCell;

            if (cell.IsLocked)
                return;

            int value;

            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                if (value == 0)
                {
                    cell.Clear();
                }
                else
                {
                    cell.Text = value.ToString();
                    cell.Value = value;
                }
                cell.ForeColor = Color.OrangeRed;
            }
        }

       private void cell_MouseClick(object sender, MouseEventArgs e)
       {
            var cell = sender as SudokuCell;

            if (cell.IsLocked)
                return;

            HighlightCells(cell);
        }

        private void HighlightCells(SudokuCell cell)
        {
            ResetCellColor();
            for (int i = 0; i < 9; i++)
            {
                cells[cell.X, i].BackColor = Color.Aquamarine;
                cells[i, cell.Y].BackColor = Color.Aquamarine;
            }

            // This finds X & Y coordinates of the top left cell of the 3x3 block.
            int topLeftCellRow = cell.X - cell.X % 3;
            int topLeftCellCol = cell.Y - cell.Y % 3;

            for (int i = topLeftCellRow; i < topLeftCellRow + 3; i++)
            {
                for (int j = topLeftCellCol; j < topLeftCellCol + 3; j++)
                {
                    cells[i, j].BackColor = Color.Aquamarine;
                }
            }
        }
        private void ResetCellColor()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                }
            }
        }

        private void FillCells()
        {
            ResetCellColor();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (puzzleGenerator.MaskedGrid[i, j] == 0)
                    {
                        cells[i, j].Text = "";
                        cells[i, j].IsLocked = false;
                    }
                    else
                    {
                        cells[i, j].Text = puzzleGenerator.MaskedGrid[i, j].ToString();
                        cells[i, j].Value = puzzleGenerator.MaskedGrid[i, j];
                        cells[i, j].IsLocked = true;
                    }
                    cells[i, j].ForeColor = SystemColors.ControlDarkDark;
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (btnPauseResumeTimer.Text == "Start")
            {
                timer1.Start();
                btnPauseResumeTimer.Text = "Pause";
                foreach (SudokuCell cell in cells)
                {
                    cell.Enabled = true;
                }
            }
            TimeInSeconds = 0;
            timer1 = new Timer();
            timer1.Start(); 
            InitGame(GameDifficulty);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetCellColor();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!cells[i, j].IsLocked)
                    {
                        cells[i, j].Text = "";
                    }
                }
            }
        }

        private void btnCheckSolution_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (isNumberInRow(cells[i, j]) || isNumberInCol(cells[i, j]) || isNumberInBlock(cells[i, j]))
                    {
                        MessageBox.Show("The solution is wrong!\n");
                        return;
                    }
                }
            }
            if (GameMode == GameMode.Stopwatch)
            {
                MessageBox.Show(string.Format(string.Format("Congrats!!!\nYou solved the sudoku in {0:00}mm:{1:00}ss!", (int)TimeInSeconds / 60, (int)TimeInSeconds % 60)));
            }
            else
            {
                MessageBox.Show(string.Format(string.Format("Congrats!!!\nYou solved the sudoku with {0:00}:{1:00} time left!", (int)TimeInSeconds / 60, (int)TimeInSeconds % 60)));

            }
        }

        private bool isNumberInRow(SudokuCell cell)
        {
            for (int i = 0; i < 9; i++)
            {
                if (i == cell.Y) continue;
                if (cells[cell.X, i].Value == cell.Value)
                {
                    return true;
                }
            }
            return false;
        }
        private bool isNumberInCol(SudokuCell cell)
        {
            for (int i = 0; i < 9; i++)
            {
                if (i == cell.X) continue;
                if (cells[i, cell.Y].Value == cell.Value)
                {
                    return true;
                }
            }
            return false;
        }
        private bool isNumberInBlock(SudokuCell cell)
        {
            // This finds X & Y coordinates of the top left cell of the 3x3 block.
            int topLeftCellRow = cell.X - cell.X % 3;
            int topLeftCellCol = cell.Y - cell.Y % 3;

            for (int i = topLeftCellRow; i < topLeftCellRow + 3; i++)
            {
                for (int j = topLeftCellCol; j < topLeftCellCol + 3; j++)
                {
                    if (i == cell.X && j == cell.Y) continue;
                    if (cells[i, j].Value == cell.Value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GameMode == GameMode.Stopwatch)
            {
                ++TimeInSeconds;
                lblTime.Text = string.Format("Time: {0:00}:{1:00}", (int)TimeInSeconds/60, (int)TimeInSeconds%60);
                return;
            }
            else // Game mode = Timer
            {
                if (TimeInSeconds == 0)
                {
                    timer1.Stop();
                    MessageBox.Show("Time is over!!! :(");
                    DialogResult = DialogResult.Cancel;
                }
                --TimeInSeconds;
                lblTime.Text = string.Format("Time: {0:00}:{1:00}", (int)TimeInSeconds / 60, (int)TimeInSeconds % 60);
            }
        }

        private void btnPauseResumeTimer_Click(object sender, EventArgs e)
        {
            if (btnPauseResumeTimer.Text == "Pause")
            {
                timer1.Stop();
                btnPauseResumeTimer.Text = "Start";
                foreach (SudokuCell cell in cells)
                {
                    cell.Enabled = false;
                    cell.Text = "X";
                    cell.BackColor = Color.Coral;
                }
                btnClear.Enabled = false;
                btnCheckSolution.Enabled = false;
            }
            else
            {
                timer1.Start();
                btnPauseResumeTimer.Text = "Pause";
                foreach (SudokuCell cell in cells)
                {
                    cell.Enabled = true;
                    if (cell.Value == 0) cell.Text = "";
                    else cell.Text = cell.Value.ToString();
                }
                btnClear.Enabled = true;
                btnCheckSolution.Enabled = true;
                ResetCellColor();
            }
        }
    }
}
