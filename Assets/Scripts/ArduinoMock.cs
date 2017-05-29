using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoMock : ArduinoBase {

    public static readonly int BRIGHTNESS_MAX = 300;


    private void Start()
    {
        _distance = 12;
        _brightness = BRIGHTNESS_MAX;
    }
    
    private void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _distance++;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _distance--;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _brightness = (_brightness == 0) ? BRIGHTNESS_MAX : 0;
        }
	}

    public override void PlaySound()
    {
        Debug.Log("PLAY SOUND");
    }

    public override void GameOver()
    {
        Debug.Log("Game Over");
    }
}
