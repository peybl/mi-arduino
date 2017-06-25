using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustOutfit : MonoBehaviour {

    private Sprite day;
    private Sprite night;

    private enum STATUS { DAY, NIGHT };
    private STATUS currentStatus;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        day = Resources.Load("Sprites/nyancat_day", typeof(Sprite)) as Sprite;
        night = Resources.Load("Sprites/nyancat_night", typeof(Sprite)) as Sprite;
    }
	
	// Update is called once per frame
	void Update () {

        //night
        if (GameManager.Instance.Arduino.Brightness >= GameManager.Instance.Arduino.EnvironmentLight && currentStatus == STATUS.DAY)
        {
            spriteRenderer.sprite = night;
            currentStatus = STATUS.NIGHT;
        }
        //day
        if (GameManager.Instance.Arduino.Brightness < GameManager.Instance.Arduino.EnvironmentLight && currentStatus == STATUS.NIGHT)
        {
            spriteRenderer.sprite = day;
            currentStatus = STATUS.DAY;
        }
    }
}
