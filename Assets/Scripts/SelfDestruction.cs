using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    private float _delay = 0;

    void Start()
    {
        StartCoroutine(WaitAndDestroy());
    }

    public void SetDelay(float delay)
    {
        _delay = delay;
    }

    #region Coroutines

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(_delay);
        Destroy(gameObject);
    }

    #endregion
}
