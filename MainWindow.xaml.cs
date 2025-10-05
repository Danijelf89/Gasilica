using Gasilica.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gasilica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainView;

        public MainWindow()
        {
            InitializeComponent();
            _mainView = new MainViewModel(this);
            DataContext = _mainView;
            StopButton.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
               
                //_mainView.Minutes = minutes;
                _mainView.Start();
                StopButton.Visibility = Visibility.Visible;
                StarButton.Visibility = Visibility.Collapsed;
            
               
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _mainView.Stop();
            OdborjavanjeTxt.Text = string.Empty;
            StarButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;
        }
    }
}