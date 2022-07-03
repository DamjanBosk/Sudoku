using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class SettingsForm : Form
    {
        public GameMode GameMode;
        public GameDifficulty GameDifficulty;
        public int TimeInSeconds;
        public SettingsForm(GameMode gameMode, GameDifficulty gameDifficulty, int timeInSeconds)
        {
            InitializeComponent();

            GameMode = gameMode;
            GameDifficulty = gameDifficulty;
            TimeInSeconds = timeInSeconds;

            if (GameMode == GameMode.Stopwatch)
            {
                rbStopwatch.Checked = true;
            }
            else
            {
                rbTimer.Checked = true;
            }

            if (GameDifficulty == GameDifficulty.Easy)
            {
                rbEasy.Checked = true;
            }
            else if (GameDifficulty == GameDifficulty.Medium)
            {
                rbMedium.Checked = true;
            }
            else
            {
                rbHard.Checked = true;
            }

            nudMinutes.Value = TimeInSeconds / 60;
            nudSeconds.Value = TimeInSeconds % 60;
        }

        private void btnApplyOptions_Click(object sender, EventArgs e)
        {
            if (rbEasy.Checked)
            {
                GameDifficulty = GameDifficulty.Easy;
            }
            else if (rbMedium.Checked)
            {
                GameDifficulty = GameDifficulty.Medium;
            }
            else
            {
                GameDifficulty = GameDifficulty.Hard;
            }

            if (rbStopwatch.Checked)
            {
                GameMode = GameMode.Stopwatch;
            }
            else // Game mode = timer
            {
                GameMode = GameMode.Timer;
                if (nudMinutes.Value == 0 && nudSeconds.Value == 0)
                {
                    MessageBox.Show("Must set time for the timer!");
                }
                TimeInSeconds = (int)(nudMinutes.Value * 60) + (int)nudSeconds.Value;
            }



            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void rbTimer_CheckedChanged(object sender, EventArgs e)
        {
            nudMinutes.Enabled = true;
            nudSeconds.Enabled = true;

            nudMinutes.Value = 5;
        }

        private void rbStopwatch_CheckedChanged(object sender, EventArgs e)
        {
            nudMinutes.Value = nudSeconds.Value = 0;
            nudMinutes.Enabled = false;
            nudSeconds.Enabled = false;
            TimeInSeconds = 0;
        }

        private void nudSeconds_ValueChanged(object sender, EventArgs e)
        {
            if (nudSeconds.Value == 60)
            {
                nudSeconds.Value = 0;
                nudMinutes.Value++;
            }
        }
    }
}
