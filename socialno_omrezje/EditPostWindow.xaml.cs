using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class editPostWindow : Window
    {
        private EditPostViewModel viewModel;

        public editPostWindow(ObservableCollection<Data> wallPosts, Data selectedPost)
        {
            InitializeComponent();
            viewModel = new EditPostViewModel(wallPosts, selectedPost);
            DataContext = viewModel;
        }
    }
}