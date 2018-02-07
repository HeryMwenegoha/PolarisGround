using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace UGCS3.Common
{
    public static class SplashScreen
    {
        private static OptimisedForm Splash = null;
        static System.Threading.Thread splashThread = null;
        static Label statusLabel = null;
        static ProgressBar statusBar = null;
        static Timer statusTimer = null;

        static private void showForm()
        {
            Splash = new OptimisedForm();
            statusLabel = new Label();
            statusBar = new ProgressBar();
            statusTimer = new Timer();

            Splash.Controls.Add(statusLabel);
            Splash.Controls.Add(statusBar);

            Splash.Text = "HeryBotics UGCS";

            Splash.MinimizeBox = false;
            Splash.FormBorderStyle = FormBorderStyle.None;
            Splash.StartPosition = FormStartPosition.Manual;

            Splash.BackgroundImage = Properties.Resources.bigstock_Vintage_compass_on_the_old_wor_87226526;
            Splash.BackgroundImageLayout = ImageLayout.Stretch;

            Splash.Size = new Size(500, 400);
            Splash.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Splash.Width + 100) / 2,
                          (Screen.PrimaryScreen.WorkingArea.Height - Splash.Height) / 2);

            statusLabel.Size = new Size(300, 17);
            statusLabel.Location = new Point((Splash.Width - statusLabel.Width) / 2, 0);// Splash.Height - statusLabel.Height - 1);
            statusLabel.Text = "By Hery A Mwenegoha Copyright 2017";
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;

            statusBar.Size = new Size(200, 17);
            statusBar.Location = new Point((Splash.Width - statusBar.Width) / 2, Splash.Height - statusBar.Height - 1);
            statusBar.Text = "loading...";
            
            statusBar.Minimum = 0;
            statusBar.Maximum = 100;
            statusBar.Value = 35;

            statusTimer.Interval = 300;
            statusTimer.Tick += StatusTimer_Tick;
            statusTimer.Start();

            Application.Run(Splash);
        }

        private static void StatusTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                statusBar.Value = statusBar.Value + 2;
            }
            catch(Exception ex)
            {

            }
        }

        static public void closeForm()
        {
            if (Splash != null)
            {

            }

            //Splash.Invoke(new Action(() => statusBar.Value = 100));

            if(Splash != null)
            {
                Splash.Invoke(
                    (MethodInvoker)delegate
                    {
                        statusBar.Value = 100;
                    }
                    );
            }
            System.Threading.Thread.Sleep(250);

            splashThread = null;
            statusLabel = null;
            statusBar = null;

            if (Splash != null)
            {
                Splash.Invoke((MethodInvoker)delegate
                {
                    Splash.Close();
                });
                Splash = null;
            }
        }

        static public void ShowScreen()
        {
            if (Splash != null)
            {
                return;
            }

            var handle = ConsoleHelper.GetConsoleWindow();

            Console.WriteLine("Loading Background Activities...");

            System.Threading.Thread.Sleep(300);

            // Hide
            ConsoleHelper.ShowWindow(handle, ConsoleHelper.SW_HIDE);

            splashThread = new System.Threading.Thread(new System.Threading.ThreadStart(showForm));
            splashThread.IsBackground = true;
            splashThread.SetApartmentState(System.Threading.ApartmentState.STA);
            splashThread.Start();

            while (Splash == null || Splash.IsHandleCreated == false)
            {
                System.Threading.Thread.Sleep(100);
            }

            int counter = 0;
            bool run_once = false;
            while (counter++ < 3)
            {
                System.Threading.Thread.Sleep(1000);

                if (run_once == false)
                {
                   // statusBar.Value = 70;

                    Application.Run(new MainView());

                    run_once = true;
                }
            }
        }
    }
}
