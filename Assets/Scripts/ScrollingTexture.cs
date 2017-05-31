using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
	public static readonly float SPEED = .1f;
    
	private void Update()
    {
        Vector2 offset = new Vector2(Time.time * SPEED, 0);
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}
