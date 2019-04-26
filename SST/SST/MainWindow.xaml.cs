using System;
using System.Collections.Generic;
using System.IO.Ports;
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

using Renci.SshNet;


namespace SST
{
    public partial class MainWindow : Window
    {
        protected SerialPort m_Serial;
        protected SshClient m_SSHClient;
        protected ShellStream m_SSHStream;


        public MainWindow()
        {
            InitializeComponent();
            m_Serial = new SerialPort();
            LoadSerialPortBox();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            SshClient c;


            if (!AreParametersCorrect())
            {
                return;
            }

            if (!m_Serial.IsOpen)
            {
                m_Serial.PortName = SerialPortBox.SelectedValue.ToString();
                m_Serial.BaudRate = int.Parse(BaudRateBox.SelectedValue.ToString());
                m_Serial.Parity = (ParityBox.SelectedValue.ToString() == "None") ? Parity.None : Parity.None;
                m_Serial.DataBits = int.Parse(BaudRateBox.SelectedValue.ToString());
                m_Serial.StopBits = (StopBitsBox.SelectedValue.ToString() == "1")? StopBits.One : StopBits.None;
                m_Serial.Handshake = (HandshakeBox.SelectedValue.ToString() == "None") ? Handshake.None : Handshake.XOnXOff;
                m_Serial.Open();
            }

            WriteStatusLine("Connected to " + HostBox.Text);

            m_SSHClient = new SshClient(HostBox.Text, int.Parse(PortBox.Text), UserNameBox.Text, PasswordInputBox.Password);
            m_SSHClient.Connect();
            m_SSHStream = m_SSHClient.CreateShellStream(TerminalTypeBox.SelectedValue.ToString(), uint.Parse(TerminalWidthBox.SelectedValue.ToString()), uint.Parse(TerminalHeightBox.SelectedValue.ToString()), 100, 100, 1024);
            m_SSHStream.DataReceived += SSHDataReceived;
            m_Serial.DataReceived += SerialDataReceived;
        }

        private void SSHDataReceived(object sender, Renci.SshNet.Common.ShellDataEventArgs e)
        {
            string Buffer;


            Buffer = m_SSHStream.Read();
            m_Serial.Write(Buffer);
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string Buffer;


            Buffer = m_Serial.ReadExisting();
            m_SSHStream.Write(Buffer);
        }

        protected bool AreParametersCorrect()
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

        protected void WriteStatusLine(string MessageToWrite)
        {
            StatusArea.Text = StatusArea.Text + MessageToWrite + Environment.NewLine;
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            m_Serial.Close();
            m_SSHStream.Close();
            WriteStatusLine("Disconnected.");
        }

        protected void LoadSerialPortBox()
        {
            string[] Ports;
            ComboBoxItem cbi;
            bool First;


            Ports = SerialPort.GetPortNames();
            Array.Sort(Ports);
            First = true;

            foreach (string Port in Ports)
            {
                cbi = new ComboBoxItem();
                cbi.Content = Port;

                if (First)
                {
                    First = false;
                    cbi.IsSelected = true;
                }

                SerialPortBox.Items.Add(cbi);
            }
        }
    }
}
