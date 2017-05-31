using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameOnObstracleCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == SceneManager.OBSTRACLE_TAG)
        {
            SceneManager.Instance.DestroyPlayer();
            GameManager.Instance.EndGame();
        }
    }
}
