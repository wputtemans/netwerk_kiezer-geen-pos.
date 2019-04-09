using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp_SMS
{
    public partial class Form_SMS_Sender : Form
    {
        private SerialPort _serialPort;
        public Form_SMS_Sender()
        {
            InitializeComponent();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());
                foreach (string s in ports)
                {
                    bool b = s.Contains("AT Port");
                    if (b)
                    {
                        string poort = s.Split('(', ')')[1];
                        string poortnr = new String(poort.Where(Char.IsDigit).ToArray());
                        Int32.Parse(poortnr);
                        label1.Text = s;
                        textBoxNumber.Text = poortnr;
                    }           
                }
            }
            textBoxNumber.Text = "10";
        }

        private void aanpassen(object sender,EventArgs e)
        {
            //MessageBox.Show("COM" + poortnr);
            _serialPort = new SerialPort("COM3", 9600);
            Thread.Sleep(1000);

            _serialPort.Open();

            Thread.Sleep(1000);

            if (radioButton1.Checked)
            {
                _serialPort.Write("AT+COPS=1,0,\"B Mobistar\"\r");
            }
            else if (radioButton2.Checked)
            {
                _serialPort.Write("AT+COPS=1,0,\"NL KPN  \"\r");
            }
            else if (radioButton3.Checked)
            {
                _serialPort.Write("AT+COPS=1,0,\"T-Mobile NL\"\r");
            }
            Thread.Sleep(1000);

            _serialPort.Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(3,e);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(2, e);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(1, e);
        }
    }
}
