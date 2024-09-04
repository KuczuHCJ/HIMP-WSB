using System.Windows;

namespace HIMP1
{
    public partial class App : Application
    {
        private LoginWindow loginWindow;
        private MainWindow mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
            if (Application.Current.Windows.OfType<LoginWindow>().Any())
            {
                loginWindow = Application.Current.Windows.OfType<LoginWindow>().First();
            }
            else
            {
                loginWindow = new LoginWindow();
            }

            loginWindow.Show(); 

            mainWindow = new MainWindow();
            mainWindow.IsEnabled = false; 

            loginWindow.Closed += (s, args) =>
            {
                if (loginWindow.IsLoginSuccessful)
                {
                    mainWindow.IsEnabled = true;
                    mainWindow.Show();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }
    }
}
