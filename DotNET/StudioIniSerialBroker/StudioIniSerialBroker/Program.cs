﻿using System;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.IO;
// using System.Net.Http;

namespace SerialPortExample
{
    class SerialPortProgram
    {
        // NOTE - COM port is hardcoded - find correct number in Device Manager

        // Create the serial port with basic settings
        // If line 20 is changed, Build > Clean Solution, Build > Rebuild Solution, Start
        private SerialPort port1 = new SerialPort("COM8", 9600, Parity.None, 8, StopBits.One);

        List<byte> bBuffer = new List<byte>();
        System.Text.ASCIIEncoding encoding = new ASCIIEncoding();
        string sBuffer = String.Empty;

        [STAThread]
        static void Main(string[] args)
        {
            // Instatiate this class
            new SerialPortProgram();
        }

        private SerialPortProgram()
        {

            Console.WriteLine("Incoming Data:");

            // Attach a method to be called when there
            // is data waiting in the port's buffer
            port1.DataReceived += new
              SerialDataReceivedEventHandler(port_DataReceived);

            // Begin communications
            port1.Open();


            port1.Write("Handshaking...");

            // Enter an application loop to keep this thread alive
            Application.Run();
        }

        private void port_DataReceived(object sender,
          SerialDataReceivedEventArgs e)
        {
            // Buffer and process binary data
            while (port1.BytesToRead > 0)
                bBuffer.Add((byte)port1.ReadByte());

            sBuffer = encoding.GetString(bBuffer.ToArray());

            Console.WriteLine(sBuffer);
            bBuffer.Clear();
            string strData = httpData("1");
            strData += "|" + httpData("2");
            port1.Write(strData);
        }

        private string httpData(string strId)
        {
            string html = string.Empty;
            string url = @"http://54.76.187.224/studio-ini.php?action=r&sensor=" + strId;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }
    }
}