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
    public partial class RegisterCamera : Form
    {
        private string name;

        private bool wantsToExit = true;

        public RegisterCamera(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        private void RegisterCamera_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            CameraController.Instance().InitializeCamera(CameraPictureBox);
            CameraPictureBox.Width = CameraController.Instance().getVideoFormat().Width;
            CameraPictureBox.Height = CameraController.Instance().getVideoFormat().Height;
            this.Width = CameraController.Instance().getVideoFormat().Width;
            this.Height = CameraController.Instance().getVideoFormat().Height;
        }

        private void RegisterCamera_Shown(object sender, EventArgs e)
        {
            CameraController.Instance().StartStreaming(true, this.Location);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void RegisterCamera_Closing(object sender, FormClosingEventArgs e)
        {

            CameraController.Instance().StopStreaming();
            if (wantsToExit)
            {
                Application.Exit();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SubmitButton.Tag.Equals(Constants.PHOTO_BUTTON))
            {
                CameraController.Instance().SetName(name);
                SubmitButton.Text = "Register";
                SubmitButton.Tag = Constants.SUBMIT_BUTTON;
                CameraController.Instance().StopStreaming();
            }
            else
            {
                WindowsFormsApp1.OpenForm openForm = new WindowsFormsApp1.OpenForm();
                openForm.Show();
                wantsToExit = false;
                this.Close();
            }
        }
    }
}
