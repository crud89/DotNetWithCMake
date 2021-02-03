using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    using Example;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            IHello hello = new CSharpClass();
            MessageBox.Show(hello.SayHello());
            hello = new CppCliClass();
            MessageBox.Show(hello.SayHello());
        }
    }
}
