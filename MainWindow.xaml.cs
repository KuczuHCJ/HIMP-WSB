
using HIMP;
using System.Windows;

namespace HIMP1
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper(); 
        }

        private void DodajPrzedmiot_Click(object sender, RoutedEventArgs e)
        {
            
            AddItemWindow addItemWindow = new AddItemWindow();
            addItemWindow.ShowDialog(); 
        }

        private void UsunPrzedmiot_Click(object sender, RoutedEventArgs e)
        {
            RemoveItemWindow removeItemWindow = new RemoveItemWindow();
            removeItemWindow.ShowDialog(); 
        }

        private void WyswietlPrzedmioty_Click(object sender, RoutedEventArgs e)
        {
            DisplayItemsWindow displayItemsWindow = new DisplayItemsWindow();
            displayItemsWindow.ShowDialog(); 
        }

        private void ZamknijProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }
    }
}
