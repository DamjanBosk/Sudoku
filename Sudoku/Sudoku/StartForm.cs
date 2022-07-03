using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class StartForm : Form
    {
        public GameMode GameMode;
        public GameDifficulty GameDifficulty;
        public int TimeInSeconds;
        public StartForm()
        {
            InitializeComponent();

            // Default settings
            GameMode = GameMode.Stopwatch;
            GameDifficulty = GameDifficulty.Medium;
            TimeInSeconds = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnStartNewGame_Click(object sender, EventArgs e)
        {
            SudokuGameForm sudoku = new SudokuGameForm(GameDifficulty, GameMode, TimeInSeconds);
            sudoku.ShowDialog();

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(GameMode, GameDifficulty, TimeInSeconds);
            if (settingsForm.ShowDialog() == DialogResult.Cancel) return;

            GameDifficulty = settingsForm.GameDifficulty;
            GameMode = settingsForm.GameMode;
            TimeInSeconds = settingsForm.TimeInSeconds;
        }
    }
}
