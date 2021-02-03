using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    using Example;

    static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IHello hello = new CSharpClass();
            MessageBox.Show(hello.SayHello());
            hello = new CppCliClass();
            MessageBox.Show(hello.SayHello());
        }
    }
}