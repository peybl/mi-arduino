using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAutoMovement : MonoBehaviour
{
    public static readonly int SPEED = 4;

    private void FixedUpdate() {
        float dist = SPEED * Time.fixedDeltaTime;
        gameObject.transform.Translate(new Vector3(-dist, 0, 0));
	}
}
