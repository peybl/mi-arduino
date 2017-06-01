using UnityEngine;
using System;
using System.IO.Ports;

public class ArduinoConnector : ArduinoBase
{
    public static readonly int SOUND_LENGTH = 10;
    private static readonly String SOUND = "a";
    private static readonly String GAMEOVER = "b";
    private static readonly String RESTART = "c";

    private SerialPort _stream = new SerialPort("COM4", 9600); //Set the port (com4) and the baud rate (9600, is standard on most devices)
    private int _soundDelay = 0;


    private void Start()
    {
        try
        {
            OpenStream();
            _stream.Write(RESTART);
        } catch(System.IO.IOException)
        {
            Debug.LogError("Could not open arduino-stream.");
        }
        _stream.ReadTimeout = 300;
        _stream.WriteLine(RESTART);
    }

    private void Update()
    {
        if (!_stream.IsOpen)
            return;

        /* Receiving */
        string value = _stream.ReadLine(); //Read the information
        string[] values = value.Split(' ');

        if (values.Length == 2)//check if arguments are send right
        {
            if (Int32.TryParse(values[0], out _brightness))
            {
                //Debug.Log("light: " + _brightness);
            }

            if (Int32.TryParse(values[1], out _distance))
            {
                //Debug.Log("distance: " + _distance);
            }
        }

        /* Sending */
        if (_soundDelay > 0)
        {
            _stream.Write(SOUND);
            _soundDelay--;
        }
        if (_gameover && _soundDelay == 0) {
            //Debug.Log(_digits);
            _stream.Write(GAMEOVER);
            _stream.Close();
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

    public override void GameOver()
    {
        _gameover = true;
        this.PlaySound();
    }
}