using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Threading;
using System.Windows;
using System.Threading;

namespace MissionControl
{
    class USBReader : MainWindow
    {
        SerialPort usbPort;

        public USBReader(int baudRate)
        {
            usbPort = new SerialPort();
            usbPort.BaudRate = baudRate;
            if  (SerialPort.GetPortNames().Length > 0) {
				usbPort.PortName = "COM4";//SerialPort.GetPortNames()[0];
            }
        }

        public bool openPort()
        {
            try
            {
                 usbPort.Open();
            }
            catch (System.UnauthorizedAccessException e)
            {
                return false;
            }
            catch (System.IO.IOException i)
            {
                return false;
            }
            return true;
        }

        public float[] readData()
        {
            string[] data = usbPort.ReadLine().Split(' ');
            float[] dataFloat = new float[4];

            if  (data.Length == 0)
            {
                return null;
            }
			else
			{
				usbPort.WriteLine("Test");
			}

            if (data[0] == "ACC")
            {
                dataFloat[0] = float.Parse(data[1]);
                dataFloat[1] = float.Parse(data[2]);
                dataFloat[2] = float.Parse(data[3]);
                dataFloat[3] = 1;
            }
            else if (data[0] == "GYRO")
            {
                dataFloat[0] = float.Parse(data[1]);
                dataFloat[1] = float.Parse(data[2]);
                dataFloat[2] = float.Parse(data[3]);
                dataFloat[3] = 2;
            }
            else
            {
                dataFloat[0] = 0;
                dataFloat[1] = 0;
                dataFloat[2] = 0;
                dataFloat[3] = 0;
            }

            return dataFloat;
        }
    }
}
