using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MbnApi;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace Netwerk_kiezer
{
    public partial class Form_Netwerk_kiezer : Form
    {
        private IMbnInterfaceManager m_MbnInterfaceManager;
        private IMbnConnectionManager m_MbnConnectionManager;
        private IMbnInterface m_MbnInterface;
        private IMbnConnection m_MbnConnection;
        private IMbnDeviceServicesContext m_MbnDeviceServicesContext;
        private IMbnDeviceServicesManager m_MbnDeviceServicesManager;
        private IMbnInterface inf;
        System.Timers.Timer t = new System.Timers.Timer(1000);

        public Form_Netwerk_kiezer()
        {
            InitializeComponent();
            InitializeManagers();
            this.TopMost = true;
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Start();

            MbnInterfaceManager mbnInfMgr = new MbnInterfaceManager();
            IMbnInterfaceManager infMgr = (IMbnInterfaceManager)mbnInfMgr;
            IMbnInterface[] interfaces = (IMbnInterface[])infMgr.GetInterfaces();
            inf = interfaces[0];
            MBN_INTERFACE_CAPS infcap = inf.GetInterfaceCapability();
            IMbnRegistration registrationInterface = m_MbnInterface as IMbnRegistration;

        }

        private void aanpassen(object sender, EventArgs e)
        {
            IMbnRegistration registrationInterface = m_MbnInterfaceManager.GetInterface(inf.InterfaceID) as IMbnRegistration;
            uint requestId;
            if (radioButton1.Checked)
            {
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20610", 1 | 2 | 4 | 8 | 16 | 24, out requestId);
            }
            else if (radioButton2.Checked)
            {
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20408", 1 | 2 | 4 | 8 | 16 | 24, out requestId);
            }
            else if (radioButton3.Checked)
            {
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20416", 1 | 2 | 4 | 8 | 16 | 24, out requestId);
            }
            else if (radioButton4.Checked)
            {
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20601", 1 | 2 | 4 | 8 | 16 | 24, out requestId);
            }
            else if (radioButton5.Checked)
            {
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20620", 1 | 2 | 4 | 8 | 16 | 24, out requestId);
            }
            else if (radioButton6.Checked)
            {
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20404", 1 | 2 | 4 | 8 | 16 | 24, out requestId);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(3, e);
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(2, e);
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(1, e);
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(4, e);
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(5, e);
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(6, e);
        }

        public void InitializeManagers()
        {
            try
            {
                if (m_MbnInterfaceManager == null)
                {
                    m_MbnInterfaceManager = (IMbnInterfaceManager)new MbnInterfaceManager();
                }

                if (m_MbnConnectionManager == null)
                {
                    m_MbnConnectionManager = (IMbnConnectionManager)new MbnConnectionManager();
                }

                if (m_MbnDeviceServicesManager == null)
                {
                    m_MbnDeviceServicesManager = (IMbnDeviceServicesManager)new MbnDeviceServicesManager();
                }
            }
            catch (Exception e)
            {

            }
        }

 
        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IMbnSignal sig = inf as IMbnSignal;
            IMbnSignal signalDetails = inf as IMbnSignal;
            IMbnRegistration registrationInterface = m_MbnInterfaceManager.GetInterface(inf.InterfaceID) as IMbnRegistration;
            label3.Invoke((Action)delegate
            { label3.Text = "Actief Netwerk: " + registrationInterface.GetProviderName(); });
            if (label3.Text == "Actief Netwerk: ")
            {
                aanpassen(5, e);
            }

            Process cmd = new Process();
            cmd.StartInfo.FileName = "netsh.exe";
            cmd.StartInfo.Arguments = "mbn show signal interface=\"mobiel*\"";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.Start();
            string output = cmd.StandardOutput.ReadToEnd();
            output = output.Substring(output.IndexOf("%") - 2, 3);
            label2.Invoke((Action)delegate
            { label2.Text = "Signaalsterkte: " + output; });
            cmd.WaitForExit();
        }
    }
}