using System.Windows;

namespace HIMP1
{
    public partial class LoginWindow : Window
    {
        private DatabaseHelper dbHelper;

        public LoginWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper(); 
        }

        public bool IsLoginSuccessful { get; private set; } = false;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            string hashedPassword = dbHelper.HashPassword(password);

            if (dbHelper.ValidateUser(username, hashedPassword))
            {
                MessageBox.Show("Logowanie udane!");
                IsLoginSuccessful = true; 
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Błędny login lub hasło.");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nazwa użytkownika i hasło nie mogą być puste.");
                return;
            }

           
            if (dbHelper.UserExists(username))
            {
                MessageBox.Show("Użytkownik o podanej nazwie już istnieje.");
                return;
            }

         
            dbHelper.RegisterUser(username, password);
            MessageBox.Show("Rejestracja zakończona sukcesem!");

            
        }
    }
}
