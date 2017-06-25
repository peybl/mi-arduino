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

    private bool _runThread=false;

    public SerialCommunication()
    {
        _port = new SerialPort(PORT, 9600);
        try
        {
            _port.Open();
        }
        catch (System.IO.IOException e)
        {
            Debug.LogError("<b>Arduino:</b> Connection couldn't, please try again.");
            this.Stop();
        }
        Debug.Log("<b>Arduino:</b> Connection established.");
        LatestLine = "";
        _runThread = true;

        Thread pollingThread = new Thread(RunPollingThread) { IsBackground = true };
        pollingThread.Start();
    }

    public void Stop()
    {
        //closing the communication thread
        if (_runThread) { 
            _runThread = false;
            Debug.Log("<b>Arduino:</b> Communication-Thread closed.");
        }
        //closing the port
        if (_port.IsOpen)
        {
            _port.Close();
            Debug.Log("<b>Arduino:</b> Connection closed.");
        }
        
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void RunPollingThread()
    {
        Debug.Log("<b>Arduino:</b> Communication-Thread started.");
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
            _port.Write(Command);
        }
    }

    public bool hasMessage()
    {
        return LatestLine.Length != 0;
    }
}