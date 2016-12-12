using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class CustomBehaviourScript : MonoBehaviour,
                                                ITrackableEventHandler
{

        //TrackableBehaviour mtrackable;
        private TrackableBehaviour mTrackableBehaviour;
        private ImageTargetBehaviour mTarget;
        Tracker objectTracker; // HOW MANY TRACKERS ARE THERE
        PlayerController thePlayer;
        StateManager sm;


    // ObjectTarget trackable = 
    // Use this for initialization
    void Start()
    {
        thePlayer = (PlayerController)GameObject.Find("Player").GetComponent<PlayerController>();
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        mTarget = GetComponent<ImageTargetBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
            Debug.LogWarning("Trackable registered.");
        }
    }


    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public new void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    private new void OnTrackingFound()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
        thePlayer.jumpActivated = true;
        Debug.Log("Trackable " + mTarget.ImageTarget.Name + " found");
        if (mTarget.ImageTarget.Name == "board-02")
            Debug.Log("Is the selected board");
    }



    private new void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }
        thePlayer.jumpActivated = false;
        Debug.Log("Trackable " + mTarget.ImageTarget.Name + " lost");
    }


    // Update is called once per frame
    void Update()
        {

        }
    }

