using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLightOnBrightness : OnBrightnessBase
{
    private Light _light = null;

    private void Update()
    {
        if (_light == null)
            _light = gameObject.GetComponent<Light>();

        if (IsEnabled())
        {
            _light.intensity = 1.2f;
        }
        else
        {
            _light.intensity = 0.5f;
        }
    }
}
