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

            ShowMessage(new CSharpClass());
            ShowMessage(new CppCliClass());
        }

        private static void ShowMessage(IHello hello)
        {
            MessageBox.Show($"{hello.SayHello()}\r\nThe answer is: {hello.AnswerEverything()}!");
        }
    }
}