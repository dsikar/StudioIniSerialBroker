using Microsoft.Kinect;
using System;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication8
{

    class SerialPortProgram
    {
        // Create the serial port with basic settings
        private SerialPort port1 = new SerialPort("COM3",
          9600, Parity.None, 8, StopBits.One);

        List<byte> bBuffer = new List<byte>();
        System.Text.ASCIIEncoding encoding = new ASCIIEncoding();
        string sBuffer = String.Empty;

        // todo - emulate 2nd (Kinect) and 3rd (xbee) serial ports i.e.
        // private SerialPort port2 = new SerialPort("COM7",
        // 9600, Parity.None, 8, StopBits.One);
        /*
        [STAThread]
        static void Main(string[] args)
        {
            // Instatiate this class
            new SerialPortProgram();
        }
        */
        public SerialPortProgram()
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

        public void port_DataReceived(object sender,
          SerialDataReceivedEventArgs e)
        {

            // Show all the incoming data in the port's buffer
            // Console.WriteLine(port1.ReadExisting());

            // Buffer and process binary data
            while (port1.BytesToRead > 0)
                bBuffer.Add((byte)port1.ReadByte());

            // Buffer string data
            // sBuffer += port1.ReadExisting();
            sBuffer = encoding.GetString(bBuffer.ToArray());

            Console.WriteLine(sBuffer.Length);
            Console.WriteLine(sBuffer);
            bBuffer.Clear();
            // ProcessBuffer(bBuffer);

            port1.Write("**Sending data to Arduino...**");
        }
    }

    class Program
    {

    [STAThread]
    static void Main(string[] args)
    {

      new SerialPortProgram();
      Console.SetWindowSize(Constants.ConsoleWidth, Constants.ConsoleHeight);
      Console.Title = "Kinect Skeleton Console";

      KinectControl c = new KinectControl(() => new ConsoleBodyDrawer());

      c.GetSensor();
      c.OpenReader();

      while (true)
      {
        var k = Console.ReadKey(true).Key;

        if (k == ConsoleKey.X)
        {
          c.CloseReader();
          c.ReleaseSensor();
          break;
        }
      }
    }
  }
}