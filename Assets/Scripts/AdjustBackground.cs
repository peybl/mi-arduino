using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustBackground : MonoBehaviour
{

    private Material day;
    private Material night;
    private MeshRenderer meshRenderer;

    private enum STATUS { DAY, NIGHT };
    private STATUS currentStatus;



    // Use this for initialization
    void Start () {
        day = Resources.Load("Materials/day", typeof(Material)) as Material;
        night = Resources.Load("Materials/night", typeof(Material)) as Material;
        meshRenderer = this.GetComponent<MeshRenderer>();
        currentStatus = STATUS.DAY;
    }
	
	// Update is called once per frame
	void Update () {

        //night
        if (GameManager.Instance.Arduino.Brightness >=300 && currentStatus == STATUS.DAY)
        {
            meshRenderer.material = night;
            currentStatus = STATUS.NIGHT;
        }
        //day
        if (GameManager.Instance.Arduino.Brightness < 300 && currentStatus == STATUS.NIGHT)
        {
            meshRenderer.material = day;
            currentStatus = STATUS.DAY;
        }
	}
}
