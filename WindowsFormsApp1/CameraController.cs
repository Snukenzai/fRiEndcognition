﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luxand;

class CameraController
{

    private static CameraController instance;
    private string cameraName;
    private PictureBox currentPictureBox;
    public bool cameraWorking = true;
    private string trackerFile = "tracker.dat";
    private string username;
    private int mouseX = 0;
    private int mouseY = 0;

    private CameraController()
    {
    }

    public void InitializeCamera(PictureBox pictureBox)
    {
        currentPictureBox = pictureBox;
        if (FSDK.FSDKE_OK != FSDK.ActivateLibrary(Constants.LICENCE_KEY))
        {
            MessageBox.Show(Constants.ERROR_ACTIVATING_FACESDK, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        FSDK.InitializeLibrary();
        FSDKCam.InitializeCapturing();

        string[] cameraList;
        int cameraCount;
        FSDKCam.GetCameraList(out cameraList, out cameraCount);

        if (cameraCount == 0)
        {
            MessageBox.Show(Constants.NO_CAMERA_ERROR, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        cameraName = cameraList[0];
        FSDKCam.VideoFormatInfo[] formatList;
        FSDKCam.GetVideoFormatList(ref cameraName, out formatList, out cameraCount);

        int videoFormat = 0;
        currentPictureBox.Width = formatList[videoFormat].Width;
        currentPictureBox.Height = formatList[videoFormat].Height;
        }

    public void StartStreaming()
    {
        int cameraHandle = 0;
        if (FSDKCam.OpenVideoCamera(ref cameraName, ref cameraHandle) != FSDK.FSDKE_OK)
        {
            MessageBox.Show(Constants.CAMERA_OPEN_ERROR, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        int tracker = 0;
        if(FSDK.LoadTrackerMemoryFromFile(ref tracker, trackerFile) != FSDK.FSDKE_OK)
        {
            FSDK.CreateTracker(ref tracker);
        }
        int err = 0;
        FSDK.SetTrackerMultipleParameters(tracker, "HandleArbitraryRotations=false; DetermineFaceRotationAngle=false; InternalResizeWidth=300; FaceDetectionThreshold=5;", ref err);

        cameraWorking = true;

        while (cameraWorking)
        {
            Application.DoEvents();
            Int32 imageHandle = 0;
            FSDKCam.GrabFrame(cameraHandle, ref imageHandle);
            FSDK.CImage image = new FSDK.CImage(imageHandle);
            Image frameImage = image.ToCLRImage();

            long [] IDs;
            long faceCount = 0;
            FSDK.FeedFrame(tracker, 0, image.ImageHandle, ref faceCount, out IDs, sizeof(long)*256);
            Array.Resize(ref IDs, (int)faceCount);

            //Drawing rectangles
            Graphics graphics = Graphics.FromImage(frameImage);

            for(int i = 0; i < IDs.Length; ++i)
            {
                FSDK.TFacePosition facePosition = new FSDK.TFacePosition();
                FSDK.GetTrackerFacePosition(tracker, 0, IDs[i], ref facePosition);

                int x = facePosition.xc - (int)(facePosition.w * 0.6);
                int y = facePosition.yc - (int)(facePosition.w * 0.6);
                int w = (int)(facePosition.w * 1.2);

                String name;
                int res = FSDK.GetAllNames(tracker, IDs[i], out name, 65536);

                if (FSDK.FSDKE_OK == res && name.Length > 0)
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;

                    graphics.DrawString(name, new System.Drawing.Font("Arial", 16),
                        new System.Drawing.SolidBrush(System.Drawing.Color.LightGreen),
                        facePosition.xc, y + w + 5, format);
                }

                mouseX = Cursor.Position.X;
                mouseY = Cursor.Position.Y;

                Pen pen = new Pen(Color.FromArgb(115, 115, 115, 115), 3);

                if (mouseX >= x && mouseX <= x + w && mouseY >= y && mouseY <= y + w)
                {
                    pen = new Pen(Color.FromArgb(115, 55, 55, 255), 3);
                    if (Control.MouseButtons == MouseButtons.Left)
                    {
                        if (FSDK.FSDKE_OK == FSDK.LockID(tracker, IDs[i]))
                        {
                            friendcognition.NameInput nameInput = new friendcognition.NameInput();
                            if (nameInput.ShowDialog() == DialogResult.OK)
                            {
                                username = nameInput.username;
                                if (username == null || username.Length <= 0)
                                {
                                    FSDK.SetName(tracker, IDs[i], "");
                                    FSDK.PurgeID(tracker, IDs[i]);
                                }
                                else
                                {
                                    FSDK.SetName(tracker, IDs[i], username);
                                }
                                FSDK.UnlockID(tracker, IDs[i]);
                            }
                        }
                    }
                }
                graphics.DrawRectangle(pen, x, y, w, w);
            }
            currentPictureBox.Image = frameImage;
            GC.Collect();
        }
        FSDK.SaveTrackerMemoryToFile(tracker, trackerFile);
        FSDK.FreeTracker(tracker);

        FSDKCam.CloseVideoCamera(cameraHandle);
        FSDKCam.FinalizeCapturing();
    }

    public void StopStreaming()
    {
        cameraWorking = false;
    }

    public void ChangePictureBox(PictureBox pictureBox)
    {
        currentPictureBox = pictureBox;
    }

    public static CameraController Instance()
    {
        if (instance == null)
        {
            instance = new CameraController();
        }
        return instance;
    }
}

