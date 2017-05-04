using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnBrightness : MonoBehaviour {

    public static readonly int BRIGHTNESS_SWITCH_VALUE = 150;

	public enum TYPE { DISABLE_WHEN_BRIGHT, DISABLE_WHEN_DARK };

    public TYPE type;


    private void Update()
    {
        gameObject.GetComponent<Collider2D>().enabled = IsEnabled(GameManager.Instance.Arduino.Brightness);
    }

    private bool IsEnabled(int brightness)
    {
        bool isEnabled = false;

        switch (type)
        {
            case TYPE.DISABLE_WHEN_BRIGHT:
                if (brightness < BRIGHTNESS_SWITCH_VALUE)
                    isEnabled = true;
                break;
            case TYPE.DISABLE_WHEN_DARK:
                if (brightness >= BRIGHTNESS_SWITCH_VALUE)
                    isEnabled = true;
                break;
        }

        return isEnabled;
    }
}
