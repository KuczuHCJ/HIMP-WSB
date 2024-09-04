using System.Collections.Generic;
using System.Windows;

namespace HIMP
{
    public partial class DisplayItemsWindow : Window
    {
        private DatabaseHelper dbHelper;
        private List<Przedmiot> przedmioty;

        public DisplayItemsWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            
            ZaladujPrzedmioty();
        }

        private void ZaladujPrzedmioty()
        {
            
            przedmioty = dbHelper.PobierzWszystkiePrzedmioty();

            
            dataGridPrzedmioty.ItemsSource = przedmioty;
        }
    }
}
