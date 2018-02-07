using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using UGCS3.Common;
namespace UGCS3
{
    static class Program
    {
        // GoogleMaps API
        // AIzaSyD4BkAWzWCMyt4L8tSWOP9G4q2QFAbWyxQ
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SplashScreen.ShowScreen();
            //Application.Run(new MainView());
        }
    }
}
