using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnBrightnessBase : MonoBehaviour
{

    public enum TYPE { DISABLE_WHEN_BRIGHT, DISABLE_WHEN_DARK };

    public TYPE type;
    

    protected bool IsEnabled()
    {
        bool isEnabled = false;

        int brightness = GameManager.Instance.Arduino.Brightness;
        switch (type)
        {
            case TYPE.DISABLE_WHEN_BRIGHT:
                if (brightness >= GameManager.Instance.Arduino.EnvironmentLight)
                    isEnabled = true;
                break;
            case TYPE.DISABLE_WHEN_DARK:
                if (brightness < GameManager.Instance.Arduino.EnvironmentLight)
                    isEnabled = true;
                break;
        }

        return isEnabled;
    }
}
