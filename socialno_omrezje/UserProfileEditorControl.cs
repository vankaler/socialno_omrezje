using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace socialno_omrezje
{
    public partial class UserProfileEditorControl : UserControl
    {
        public ViewModel ViewModel { get; set; }

        public UserProfileEditorControl()
        {
            InitializeComponent();
        }

        public UserProfileEditorControl(ViewModel viewModel) : this()
        {
            ViewModel = viewModel;
            btnSaveChanges.Click += BtnSaveChanges_Click;
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // Null check for ViewModel and its properties
            if (ViewModel != null && ViewModel.MeData != null && ViewModel.TempMeData != null)
            {
                ViewModel.MeData.Update(ViewModel.TempMeData);
                MessageBox.Show("User data updated!");

                // Access TempData here if needed
                var tempMeData = ViewModel.TempMeData;
            }
        }

    }

    public class IsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
