using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SST
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreParametersCorrect())
            {
                return;
            }
        }

        protected Boolean AreParametersCorrect()
        {
            bool Response;
            StringBuilder Error;
            int Port;


            Response = true;
            Error = new StringBuilder();

            if (string.IsNullOrEmpty(HostBox.Text))
            {
                Response = false;
                Error.AppendLine("Enter a host.");
            }

            if (string.IsNullOrEmpty(PortBox.Text))
            {
                Response = false;
                Error.AppendLine("Enter a port.");
            }
            else
            {
                if (!int.TryParse(PortBox.Text, out Port))
                {
                    Response = false;
                    Error.AppendLine("Enter a valid number for the port.");
                }
            }

            if (string.IsNullOrEmpty(UserNameBox.Text))
            {
                Response = false;
                Error.AppendLine("Enter a username.");
            }

            if (string.IsNullOrEmpty(PasswordInputBox.Password))
            {
                Response = false;
                Error.AppendLine("Enter a password.");
            }


            if (Response)
            {
                return true;
            }
            else
            {
                MessageBox.Show(Error.ToString(), "Can't Connect", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
