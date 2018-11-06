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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //SplashScreen.ShowScreen(); // undo this if you want to run splash screen then cover application run
            Application.Run(new MainView());
        }
    }
}
