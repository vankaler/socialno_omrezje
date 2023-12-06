using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace socialno_omrezje
{
    public partial class MainWindow : Window
        
    {
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ViewModel();

            DataContext = viewModel;
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void postDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show( viewModel.IzbranaObjava.Content, "Content");
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files |*.jpg;*.jpeg;*.png|All Files |*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                viewModel.IzbranaObjava.Slika = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

    }
}
