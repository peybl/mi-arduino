using UnityEngine;
using System;
using System.IO.Ports;

public class ArduinoConnector : ArduinoBase
{
    public static readonly int SOUND_LENGTH = 300;

    private SerialPort _stream = new SerialPort("COM3", 9600); //Set the port (com4) and the baud rate (9600, is standard on most devices)
    private int _soundDelay = 0;


    private void Start()
    {
        try
        {
            OpenStream();
        } catch(System.IO.IOException)
        {
            Debug.LogError("Could not open arduino-stream.");
        }
        _stream.ReadTimeout = 400;
    }

    private void Update()
    {
        if (!_stream.IsOpen)
            return;

        /* Receiving */
        string value = _stream.ReadLine(); //Read the information
        string[] values = value.Split(' ');

        if (Int32.TryParse(values[0], out _brightness))
        {
            Debug.Log("light: " + _brightness);
        }

        if (Int32.TryParse(values[1], out _distance))
        {
            Debug.Log("distance: " + _distance);
        }

        /* Sending */
        if (_soundDelay > 0)
        {
            _stream.Write("c");
            _soundDelay--;
        }
    }

    private void OpenStream()
    {
        if(_stream != null && !_stream.IsOpen)
        {
            _stream.Open(); //Open the Serial Stream.
        }
       
    }

    private void OnApplicationQuit()
    {
        if (_stream != null && _stream.IsOpen)
        {
            _stream.Close(); //Close the Serial Stream.
        }
    }

    public override void PlaySound()
    {
        _soundDelay = SOUND_LENGTH;
    }
}