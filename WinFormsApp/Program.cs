using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Windows.Forms.Show("Test");
        }
    }
}