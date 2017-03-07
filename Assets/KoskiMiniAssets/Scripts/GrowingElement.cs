using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]


public class GrowingElement : MonoBehaviour {

    public float distanceTH;
    public float distance2Player;
    protected Transform thePlayer;
    protected Animator animationElements;
    protected bool growthTriggered = false;

    // Use this for initialization
    protected void Start() {
        thePlayer = GameObject.Find("Player").transform;
        animationElements = this.GetComponent<Animator>();
    }
	
    protected void checkDistance()
    {
        distance2Player = (thePlayer.position - this.GetComponent<Transform>().position).magnitude;
        if (distance2Player <= distanceTH)
        {
            growthTriggered = true;
        }
    }



    // Update is called once per frame
    protected void Update () {

        

        if (growthTriggered)
        {
            animationElements.SetBool("DoGrowth", growthTriggered);
        }
        else
        {
            checkDistance();
        }        



    }
}
