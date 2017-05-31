using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArduinoBase : MonoBehaviour
{
    protected int _brightness = 0;
    public int Brightness { get { return _brightness; } }

    protected int _distance = 0;
    public int Distance { get { return _distance; } }

    protected int _digits = 0;
    public int Digits { set { _digits = value; } }

    protected bool _gameover = false;

    public abstract void PlaySound();

    public abstract void GameOver();
}
