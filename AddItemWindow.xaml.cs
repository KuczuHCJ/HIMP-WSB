using System.Windows;

namespace HIMP
{
    public partial class AddItemWindow : Window
    {
        private DatabaseHelper dbHelper;

        public AddItemWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void DodajPrzedmiot_Click(object sender, RoutedEventArgs e)
        {
            //Na podstawie klasy pobiera dane przedmiotu od usera
            string nazwa = txtNazwa.Text;
            string lokalizacja = txtLokalizacja.Text;
            string opis = txtOpis.Text;

            if (!string.IsNullOrEmpty(nazwa) && !string.IsNullOrEmpty(lokalizacja) && !string.IsNullOrEmpty(opis))
            {
                // Jeśli się powiedzie tworzy przedmiot i dodaje do DB
                Przedmiot nowyPrzedmiot = new Przedmiot(0, nazwa, lokalizacja, opis);
                dbHelper.DodajPrzedmiot(nowyPrzedmiot);

                MessageBox.Show("Przedmiot został dodany!");
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Proszę wypełnić wszystkie pola.");
            }
        }
    }
}
