using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalUserMovement : MonoBehaviour {

    public static readonly int DISTANCE_MIN = 5;    // Top position
    public static readonly int DISTANCE_MAX = 20;   // Bottom position

    
	private void FixedUpdate () {
        // TODO include delta time and make movemente dynamic!

        // Get arduino vertical input and clip to range
        int dist = GameManager.Instance.Arduino.Distance;
        dist = dist < DISTANCE_MIN ? DISTANCE_MIN : dist;
        dist = dist > DISTANCE_MAX ? DISTANCE_MAX : dist;

        // Calc percentage from top
        dist -= DISTANCE_MIN;
        float percentage = dist > 0 ? 100.0f / (DISTANCE_MAX - DISTANCE_MIN) * dist : 0;    // e.g. 33.3
        percentage /= 100;  // e.g. 0.333
        
        // Set y position of object
        Vector3 pos = gameObject.transform.position;
        pos.y = GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.Y, percentage);
        gameObject.transform.position = pos;
    }
}
