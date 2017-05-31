using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliderOnBrightness : OnBrightnessBase
{
    private Collider2D _collider = null;

    private void Update()
    {
        if (_collider == null)
            _collider = gameObject.GetComponent<Collider2D>();

        _collider.enabled = IsEnabled();
    }
}
