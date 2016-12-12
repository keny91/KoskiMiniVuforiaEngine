using UnityEngine;
using System.Collections;


namespace Vuforia
{

    public class CustomBehaviourScript : DefaultTrackableEventHandler
    {

        TrackableBehaviour trackable;
        ////Tracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>(); // HOW MANY TRACKERS ARE THERE

       // ObjectTarget trackable = 
        // Use this for initialization
        void Start()
        {
            //Tracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
           ///////////////// ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
           /* bool success = objectTracker.PersistExtendedTracking(!DefaultTrackableEventHandler.mPersistExtendedTracking);
            if (success)
            {
                Debug.Log("PersistentExtendedTrackingEnabled");
            }
            */
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}