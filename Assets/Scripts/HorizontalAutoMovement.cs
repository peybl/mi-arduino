using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAutoMovement : MonoBehaviour {

    public int speed = 4;

    private void FixedUpdate () {
        float dist = speed * Time.fixedDeltaTime;
        gameObject.transform.Translate(new Vector3(-dist, 0, 0));
	}
}
