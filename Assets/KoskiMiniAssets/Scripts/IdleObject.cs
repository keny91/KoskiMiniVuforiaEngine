using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleObject : MonoBehaviour {

    bool doingNothing = true;
    float timeUnproductive = 0f;
    float idleTimerTrigger = 12f;
    float RegisteredTime = 0f;
    int numAnimatorBranches = 2;
    Animator animationElements;

    // Use this for initialization
    void Start () {
        RegisteredTime = Time.time;
        animationElements = this.GetComponent<Animator>();

    }
	

    void getTimeStamp()
    {
        RegisteredTime = Time.time;
    }


    public void SomethingWasDone()
    {
        getTimeStamp();
    }


    float GetIdleDuration()
    {
        return Time.time - RegisteredTime;
    }


    public IEnumerator Reset()
    {
        yield return new WaitForSeconds(1);
        getTimeStamp();
        doingNothing = true;
        yield return new WaitForSeconds(1);
    }

        // Update is called once per frame
        void Update () {

        timeUnproductive = GetIdleDuration();

        if (timeUnproductive > idleTimerTrigger && doingNothing)
        {
            animationElements.SetBool("DoIdle", true);
            int selected = (int)Random.Range(0, numAnimatorBranches);
            //Debug.LogError("Triggered: " + selected);
            animationElements.SetInteger("AnimVersion", selected);
            doingNothing = false;


            /*
             * NO PARTICLES ATM
             * 
            if (transform.FindChild("Particles").GetComponent<ParticleSystem>())
            {
                // Debug.LogError("Starting particles");
                transform.FindChild("Particles").GetComponent<ParticleSystem>().enableEmission = true;
                transform.FindChild("Particles").GetComponent<ParticleSystem>().Play();
            }
            */

            StartCoroutine(Reset());

        }
        else if (!doingNothing)
        {
        
        }

    }
}
