using Snake_Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_The_Game
{
    public partial class HighScoreScreen : Form
    {
        bool checkName;
        public HighScoreScreen()
        {
            InitializeComponent();
            checkName = true;
            WriteMessage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckValidName();
        }
        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            tbName.ForeColor = Color.Black;
            if (checkName)
            {
                tbName.Text = "";
                checkName = false;
            }
            if (e.KeyCode == Keys.Enter)
                CheckValidName();
        }
        private void WriteMessage()
        {
            tbName.ForeColor = Color.Gray;
            tbName.Text = "Max 10 Chars";

        }
        private void CheckValidName()
        {
            if (tbName.Text == "Max 10 Chars" || tbName.Text.Length > 10 || tbName.Text == "")
            {
                checkName = true;
                WriteMessage();

            }

            else if (tbName.Text != "" && tbName.Text.Length < 11 && tbName.Text != "Max 10 Chars")
            {
                Settings.Name = tbName.Text;
                Score score = new Score(Settings.Name, Settings.score);
                this.Close();
            }
        }
    }


}

