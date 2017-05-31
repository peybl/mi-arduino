using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnBrightnessBase : MonoBehaviour
{
    public static readonly int BRIGHTNESS_SWITCH_VALUE = 300;

    public enum TYPE { DISABLE_WHEN_BRIGHT, DISABLE_WHEN_DARK };

    public TYPE type;
    

    protected bool IsEnabled()
    {
        bool isEnabled = false;

        int brightness = GameManager.Instance.Arduino.Brightness;
        switch (type)
        {
            case TYPE.DISABLE_WHEN_BRIGHT:
                if (brightness >= BRIGHTNESS_SWITCH_VALUE)
                    isEnabled = true;
                break;
            case TYPE.DISABLE_WHEN_DARK:
                if (brightness < BRIGHTNESS_SWITCH_VALUE)
                    isEnabled = true;
                break;
        }

        return isEnabled;
    }
}
