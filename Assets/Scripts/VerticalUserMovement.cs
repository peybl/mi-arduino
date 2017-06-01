using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalUserMovement : MonoBehaviour
{
    public static readonly int DISTANCE_MIN = 5;    // Top position
    public static readonly int DISTANCE_MAX = 20;   // Bottom position

    private float[] yPosition;
    private int lastPosition;

    private void Start()
    {
        yPosition = new float[5];
        lastPosition = 0;
    }
    
	private void FixedUpdate()
    {
        // Get arduino vertical input and clip to range
        int dist = GameManager.Instance.Arduino.Distance;
        dist = dist < DISTANCE_MIN ? DISTANCE_MIN : dist;
        dist = dist > DISTANCE_MAX ? DISTANCE_MAX : dist;

        // Calc percentage from top
        dist -= DISTANCE_MIN;
        float percentage = dist > 0 ? 100.0f / (DISTANCE_MAX - DISTANCE_MIN) * dist : 0;    // e.g. 33.3
        percentage /= 100;  // e.g. 0.333
        percentage = 1 - percentage; //to turn the vertical control upside down

        // Set y position of object
        Vector3 pos = gameObject.transform.position;

        pos.y = smoothTransition(GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.Y, percentage));
        gameObject.transform.position = pos;
    }

    private float smoothTransition(float currentYPosition)
    {
        yPosition[4] = yPosition[3];
        yPosition[3] = yPosition[2];
        yPosition[2] = yPosition[1];
        yPosition[1] = yPosition[0];
        yPosition[0] = currentYPosition;

        return (yPosition[0] + yPosition[1] + yPosition[2] + yPosition[3] + yPosition[4]) /5.0f;
    }
}
