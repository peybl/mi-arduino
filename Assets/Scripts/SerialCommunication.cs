using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialCommunication
{
    private const string PORT = "COM4";
    private SerialPort _port;

    public string LatestLine { get; set; }

    private bool _runThread = true;

    public SerialCommunication()
    {
        _port = new SerialPort(PORT, 9600);
        _port.Open();
        LatestLine = "";

        Thread pollingThread = new Thread(RunPollingThread) { IsBackground = true };
        pollingThread.Start();
    }

    public void Stop()
    {
        _runThread = false;
        if (_port.IsOpen)
        {
            _port.Close();
        }
    }

    private void RunPollingThread()
    {
        while (_runThread)
        {
            PollArduino();
        }
    }

    private void PollArduino()
    {

        if (!_port.IsOpen)
        {
            return;
        }

        LatestLine = _port.ReadLine();
     }

    public void SendCommand(string Command)
    {
        if (_port.IsOpen)
        {
            Debug.Log(Command);
            _port.Write(Command);
        }
    }

    public bool hasMessage()
    {
        return LatestLine.Length != 0;
    }
}