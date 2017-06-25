using UnityEngine;
using System;
using System.IO.Ports;

public class ArduinoConnector : ArduinoBase
{
    public static readonly int SOUND_LENGTH = 15;
    private static readonly String SOUND = "a";
    private static readonly String GAMEOVER = "b";
    private static readonly String RESTART = "c";
    private SerialCommunication serialCommunication;

    private int _soundDelay = 0;
    


    void Start()
    {
        serialCommunication = new SerialCommunication();
        serialCommunication.SendCommand(RESTART);
    }

    private void Update()
    {
        if (serialCommunication.hasMessage())
        {
            /* Receiving */
            string value = serialCommunication.LatestLine; //Read the information

            //Debug.Log(value + "");
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
        }

            /* Sending */
            if (_soundDelay > 0)
            {
                serialCommunication.SendCommand(SOUND);
                _soundDelay--;
            }
            if (_gameover && _soundDelay == 0)
            {
                serialCommunication.SendCommand(GAMEOVER);
                this.CloseStream();
            }
        
    }

    private void CloseStream()
    {
        serialCommunication.Stop();
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

    void OnApplicationQuit()
    {
        serialCommunication.SendCommand(RESTART);
        this.CloseStream();
    }
}