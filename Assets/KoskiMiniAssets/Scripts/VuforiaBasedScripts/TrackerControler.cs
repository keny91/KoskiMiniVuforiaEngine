using UnityEngine;
using System.Collections;
using Vuforia;

public class TrackerControler : DefaultTrackableEventHandler {


    private TrackableBehaviour mTrackableBehaviour;
     
    // Use this for initialization
    void Start () {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
