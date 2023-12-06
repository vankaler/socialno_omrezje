using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace socialno_omrezje
{
    public partial class addPostWindow : Window
    {
        private AddPostViewModel viewModel;

        public addPostWindow(ObservableCollection<Data> wallPosts)
        {
            InitializeComponent();
            viewModel = new AddPostViewModel(wallPosts);
            DataContext = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the property changed is SelectedImage
            if (e.PropertyName == nameof(AddPostViewModel.SelectedImage))
            {
                // Update the image source in the UI
                UpdateImageSource();
            }
        }

        private void UpdateImageSource()
        {
            if (DataContext is AddPostViewModel vm)
            {
                imageControl.Source = vm.SelectedImage;
            }
        }
    }
}
