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
using System.Windows.Shapes;
using System.IO;

namespace WpfApp3
{
    public partial class ZoomImage : Window
    {
        public ZoomImage(string imgpath)
        {
            InitializeComponent();
            BitmapImage bitmap= new BitmapImage(new Uri(imgpath));
            img.Source = bitmap;
        }
    }
}
