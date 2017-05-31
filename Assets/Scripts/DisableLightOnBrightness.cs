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

        _light.enabled = IsEnabled();
    }
}
