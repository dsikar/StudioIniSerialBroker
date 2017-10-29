using Microsoft.Kinect;
using System;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication8
{
    class Program
    {

    [STAThread]
    static void Main(string[] args)
    {

            // new SerialPortProgram();
            //Console.SetWindowSize(Constants.ConsoleWidth, Constants.ConsoleHeight);
            Console.SetWindowSize(Constants.ConsoleWidth, Constants.ConsoleHeight);
            Console.Title = "CBT - PULL REQUESTS";

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