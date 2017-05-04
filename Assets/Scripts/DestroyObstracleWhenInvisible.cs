using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstracleWhenInvisible : MonoBehaviour {

    private void OnBecameInvisible()
    {
        SceneManager.Instance.DestroyObstracle(gameObject);
    }
}
