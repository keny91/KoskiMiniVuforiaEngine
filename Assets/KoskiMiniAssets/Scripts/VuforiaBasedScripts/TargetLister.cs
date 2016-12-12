using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class TargetLister : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StateManager sm = TrackerManager.Instance.GetStateManager();
        IEnumerable<TrackableBehaviour> tbs = sm.GetActiveTrackableBehaviours();

        foreach (TrackableBehaviour tb in tbs)
        {
            string name = tb.TrackableName;
          /*  if (tb.GetType().Equals(TrackableType.IMAGE_TARGET))
            {
                ImageTarget it = tb.Trackable as ImageTarget;
                Vector2 size = it.GetSize();

                Debug.Log("Active image target:" + name + "  -size: " + size.x + ", " + size.y);
            }
            */
        }
    }
}