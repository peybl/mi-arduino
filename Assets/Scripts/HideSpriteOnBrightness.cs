using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpriteOnBrightness : OnBrightnessBase
{
    public Sprite invisible = null;

    private SpriteRenderer _renderer = null;
    private Sprite _visible = null;

    private void Update()
    {
        /* Disabling SpriteRenderer does not work -> bug in unity?! */

        if (_renderer == null)
        {
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _visible = _renderer.sprite;
        }

        if (IsEnabled())
            _renderer.sprite = _visible;
        else
            _renderer.sprite = invisible;
    }
}
