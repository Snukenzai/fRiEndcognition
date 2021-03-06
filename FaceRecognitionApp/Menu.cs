﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace friendcognition
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        int menu = 0;
        private void MenuButtonNext_Click(object sender, EventArgs e)
        {
            IdentClicks.Text = "prev";
            menu -= 1;
            timerPrev.Start();

        }

        private void MenuButtonPrev_Click(object sender, EventArgs e)
        {
            IdentClicks.Text = "next";
            menu += 1;
            timerNext.Start();

        }

        private void timerPrev_Tick(object sender, EventArgs e)
        {
            if (IdentClicks.Text == "prev")
            {
                if (SpreadPanel.Left >= -470)
                {
                    SpreadPanel.Left -= 10;
                    if (SpreadPanel.Left <= 5 && menu == 1)
                    {
                        timerPrev.Stop();
                    }
                    if (SpreadPanel.Left <= -156 && menu == 0)
                    {
                        timerPrev.Stop();
                    }
                    else if (SpreadPanel.Left <= 168 && menu == 2)
                    {
                        timerPrev.Stop();
                    }

                    else if (SpreadPanel.Left <= -318 && menu == -1)
                    {
                        timerPrev.Stop();
                    }
                    else if (SpreadPanel.Left <= -480 && menu == -2)
                    {
                        timerPrev.Stop();
                    }


                }
            }
        }

        private void timerNext_Tick(object sender, EventArgs e)
        {
            if (IdentClicks.Text == "next")
            {
                if (SpreadPanel.Left <= 169)
                {
                    SpreadPanel.Left += 10;

                    if (SpreadPanel.Left >= -156 && menu == 0)
                    {
                        timerNext.Stop();
                    }

                    else if (SpreadPanel.Left >= 5 && menu == 1)
                    {
                        timerNext.Stop();
                    }
                    else if (SpreadPanel.Left >= 168 && menu == 2)
                    {
                        timerNext.Stop();
                    }

                    else if (SpreadPanel.Left >= -318 && menu == -1)
                    {
                        timerNext.Stop();
                    }

                    else if (SpreadPanel.Left >= -480 && menu == -2)
                    {
                        timerNext.Stop();
                    }

                }
            }

        }

        private void MenuReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.OpenForms["OpenForm"].Close();
            LOGIN login = new LOGIN();
            login.Show();
        }

        private void MenuProfile_Click(object sender, EventArgs e)
        {
            this.Close();
            Profile profile = new Profile();
            profile.Show();
        }

    }
}
