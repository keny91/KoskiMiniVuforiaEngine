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
    protected bool doOneTime = false;
    public int numAnimatorBranches = 2;

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

        

        if (growthTriggered && !doOneTime)
        {
            animationElements.SetBool("DoGrowth", growthTriggered);
            int selected = (int)Random.Range(0, numAnimatorBranches);
            //Debug.LogError("Triggered: " + selected);
            animationElements.SetInteger("AnimVersion", selected);
            if (transform.FindChild("Particles").GetComponent<ParticleSystem>())
            {
                Debug.LogError("Starting particles");
                transform.FindChild("Particles").GetComponent<ParticleSystem>().enableEmission = true;
                transform.FindChild("Particles").GetComponent<ParticleSystem>().Play();
            }
            doOneTime = true;
        }
        else if(!doOneTime)
        {
            checkDistance();
        }        



    }
}
