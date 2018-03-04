using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        ImagePopDao imagePopDao;
        public static int size = 100;
        public MainWindow()
        {
            InitializeComponent();
            imagePopDao = ImagePopDao.GetInstance();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(pathSearch.Text.Length< 10)
            {
                MessageBox.Show("10 simvoldan kicikdir!!");
                return;
            }
            var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            var files = GetFilesFrom(pathSearch.Text, filters, false);
            foreach (var item in files)
            {
                imagePopDao.Create(item);
            }
            lst.ItemsSource = imagePopDao.getList();

        }
        public static String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            label1.Content = "Source:";
            label1.Content += imagePopDao.ReadImage(lst.SelectedIndex).ImagePath;
            FileInfo file = new FileInfo(imagePopDao.ReadImage(lst.SelectedIndex).ImagePath);
            label2.Content = "Size:";
            label2.Content += ((float)(file.Length/1024)/1024).ToString();
            label2.Content += "Mb";
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePopDao.ReadImage(lst.SelectedIndex).ImagePath));
            label3.Content = "Image Size:";
            label3.Content += bitmapImage.Width + "x" + bitmapImage.Height;
        }

        private void lst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ZoomImage zoom = new ZoomImage(imagePopDao.ReadImage(lst.SelectedIndex).ImagePath);
            zoom.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            lst_MouseDoubleClick(null, null);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            imagePopDao.Delete(imagePopDao.ReadImage(lst.SelectedIndex));
            lst.ItemsSource = imagePopDao.getList();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            string patha = imagePopDao.ReadImage(lst.SelectedIndex).ImagePath;
            MessageBox.Show("");
            lst.ItemsSource = "";
            File.Delete(patha);
            imagePopDao.clear();
            var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            var files = GetFilesFrom(pathSearch.Text, filters, false);
            foreach (var item in files)
            {
                imagePopDao.Create(item);
            }
            lst.ItemsSource = imagePopDao.getList();
        }
    }


    public class ImagePop
    {
        public string Title{ get; set; }
        public string ImagePath{ get; set; }

        public string ImageSize { get; set; }


        public ImagePop(string Path)
        {
            this.ImagePath = Path;
            
        }

        
        public ImagePop()
        {

        }
    }


    interface ICRUD
    {
        void Create(string Path);
        FileInfo Read(string Path);
        void Update(string Path);
        void Delete(ImagePop img);
        List<ImagePop> getList();
    }
    public class ImagePopDao : ICRUD
    {
        List<ImagePop> listimages;
        static ImagePopDao imgpop = new ImagePopDao();


        public static ImagePopDao GetInstance()
        {
            return imgpop;
        }

        ImagePopDao()
        {
            listimages = new List<ImagePop>();
        }
        public void Create(string Path)
        {
            listimages.Add(new ImagePop() { ImagePath = Path , Title = Read(Path).Name});
        }

        public FileInfo Read(string Path)
        {
            FileInfo oFileInfo = new FileInfo(Path);
            return oFileInfo;
        }
        
        public ImagePop ReadImage(int id)
        {
            return listimages[id];
        }

        public void Update(string Path)
        {
            //Lazim Deyil
        }

        public void Delete(ImagePop img)
        {
            listimages.Remove(img);
        }

        public List<ImagePop> getList()
        {
            return listimages;
        }

        public void clear()
        {
            listimages.Clear();
        }
    }
}
