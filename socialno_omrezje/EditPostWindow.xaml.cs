﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace socialno_omrezje
{
    public partial class editPostWindow : Window
    {
        private EditPostViewModel viewModel;

        public editPostWindow(ObservableCollection<PostData> wallPosts, PostData selectedPost)
        {
            InitializeComponent();

            viewModel = new EditPostViewModel(wallPosts, selectedPost);
            DataContext = viewModel;
        }
    }
}
