using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.IO.Ports;

public class ArduinoConnector : MonoBehaviour
{

    SerialPort stream = new SerialPort("COM3", 9600); //Set the port (com4) and the baud rate (9600, is standard on most devices)

    //properties from arduino
    private int light = 0;
    public int Light { get { return light; } }
    private int distance = 0;
    public int Distance { get { return distance; } }

    private int life = 0;
    public int Life { set { life = value; } }

    private int soundlength = 300;
    private int sounddelay = 0;


    void Start()
    {
        this.OpenStream();
        stream.ReadTimeout = 400;
    }

    void Update()
    {
        //photoresistor
        string value = stream.ReadLine(); //Read the information
        string[] values = value.Split(' ');

        if (Int32.TryParse(values[0], out light))
        {
            Debug.Log("light: " + light);
            
        }

        if (Int32.TryParse(values[1], out distance))
        {
            Debug.Log("distance: " + distance);
        }

        if (sounddelay > 0)
        {
            stream.Write("c");
            sounddelay--;
        }



    }

    void OpenStream()
    {
        if(stream != null && !stream.IsOpen)
        {
            stream.Open(); //Open the Serial Stream.
        }
       
    }

    void OnApplicationQuit()
    {
        if (stream != null && stream.IsOpen)
        {
            stream.Close(); //Close the Serial Stream.
        }
    }

    void PlaySound()
    {
        sounddelay = soundlength;
    }

}