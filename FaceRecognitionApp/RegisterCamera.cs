﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace friendcognition
{
    public partial class RegisterCamera : Form
    {
        private string email;
        private string password;

        private bool wantsToExit = true;

        public RegisterCamera(string email, string password)
        {
            InitializeComponent();
            this.email = email;
            this.password = password;
        }
        private void RegisterCamera_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            CameraController.Instance().InitializeCamera(CameraPictureBox);
            CameraPictureBox.Width = CameraController.Instance().getVideoFormat().Width;
            CameraPictureBox.Height = CameraController.Instance().getVideoFormat().Height;
            this.Width = CameraController.Instance().getVideoFormat().Width;
            this.Height = CameraController.Instance().getVideoFormat().Height;
            this.CenterToScreen();
        }
        private void RegisterCamera_Shown(object sender, EventArgs e)
        {
            CameraController.Instance().StartStreaming(true, this.Location);
        }
        private void RegisterCamera_Closing(object sender, FormClosingEventArgs e)
        {
            CameraController.Instance().StopStreaming();
            if (wantsToExit)
            {
                Application.Exit();
            }
        }
        private DataTable getDatable()
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("Email", email));
            sqlParams.Add(new SqlParameter("Password", password));

            return DataController.Instance().ExecSP("Login", sqlParams);
        }
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            var dt = getDatable();

            if (CameraController.Instance().Register(dt))
            {
                SubmitButton.Visible = false;
                CameraController.Instance().StopStreaming();
            }
        }

        private void RegisterCameraRetakeButton_Click(object sender, EventArgs e)
        {
            SubmitButton.Visible = true;
            CameraController.Instance().StartStreaming(true, this.Location);
        }

        private void RegisterCameraRegisterButton_Click(object sender, EventArgs e)
        {
            var dt = getDatable();
            wantsToExit = false;
            this.Close();
            DataController.Instance().setId(Convert.ToInt32(dt.Rows[0]["Id"]));
            FaceRecognitionApp.OpenForm openForm = new FaceRecognitionApp.OpenForm();
            openForm.Show();
        }
    }
}
