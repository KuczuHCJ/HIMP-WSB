using System.Collections.Generic;
using System.Windows;

namespace HIMP
{
    public partial class RemoveItemWindow : Window
    {
        private DatabaseHelper dbHelper;
        private List<Przedmiot> przedmioty;

        public RemoveItemWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            
            ZaladujPrzedmioty();
        }

        private void ZaladujPrzedmioty()
        {
            
            przedmioty = dbHelper.PobierzWszystkiePrzedmioty();

            
            lstPrzedmioty.Items.Clear();
            foreach (var przedmiot in przedmioty)
            {
                lstPrzedmioty.Items.Add($"{przedmiot.Id} - {przedmiot.Nazwa} ({przedmiot.Lokalizacja})");
            }
        }

        private void UsunPrzedmiot_Click(object sender, RoutedEventArgs e)
        {
            
            List<int> idDoUsuniecia = new List<int>();

            foreach (var selectedItem in lstPrzedmioty.SelectedItems)
            {
               
                string itemText = selectedItem.ToString();
                int id = int.Parse(itemText.Split('-')[0].Trim());
                idDoUsuniecia.Add(id);
            }

            
            dbHelper.UsunPrzedmioty(idDoUsuniecia);

           
            ZaladujPrzedmioty();

            MessageBox.Show("Przedmioty zostały usunięte!");
        }
    }
}
