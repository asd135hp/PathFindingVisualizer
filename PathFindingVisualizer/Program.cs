using System;
using System.Windows.Forms;

namespace Assignment1
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length > 3)
            {
                MessageBox.Show("Too many arguments. Terminating...");
                return;
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args[0], args[1], args.Length == 3 ? args[2] : null));
        }
    }
}
