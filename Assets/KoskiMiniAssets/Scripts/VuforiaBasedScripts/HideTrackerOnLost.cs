using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class HideTrackerOnLost : MonoBehaviour
                                                
{


    public TrackableBehaviour.Status tracking;
    TrackerObj[] trackableList;
    //PlayerController thePlayer;
    public GameControllerScript theController;
    public bool allUntracked = true;

    // ObjectTarget trackable = 
    // Use this for initialization
    void Start() {

        //GameObject[] theList = GameObject.FindGameObjectsWithTag("Trackable");
        int listLenght = transform.childCount;
        this.transform.name = "Targets";

        //thePlayer = (PlayerController)GameObject.Find("Player").GetComponent<PlayerController>();
        theController = (GameControllerScript)GameObject.Find("GameControl").GetComponent<GameControllerScript>();


        //trackableList = new TrackerObj[theList.Length];
        trackableList = new TrackerObj[listLenght];
        for (int m = 0; m < listLenght; m++)
        {
            trackableList[m].setName(transform.GetChild(m).name);
            transform.GetChild(m).GetComponent<KoskiTrackableEventHandler>().Start();
            trackableList[m].setTracked(false);
        }
    }




    /// <summary>
    /// Check if the Overall multitracker should change
    /// </summary>
    private void checkAllUntracked()
    {
        allUntracked = true;
        for (int m = 0; m < trackableList.Length; m++)
        {
            if (trackableList[m].isTracked())
            {
                allUntracked = false;
                //Debug.LogWarning("AllUNTRACKED SET TO FALSE");
                break;
            }
                
        }
    }


    /// <summary>
    /// When one of the TrackedLists has been updated, trigger the multi-target hide/show if the "allUntracked" status changes
    /// </summary>
    /// <param name="theTrackerName"></param>
    /// <param name="isTracked"></param>
    public void UpdateTrackedList(string theTrackerName, bool isTracked, TrackableBehaviour.Status s = TrackableBehaviour.Status.NOT_FOUND)
        {


        try
        {
            for (int m = 0; m < trackableList.Length; m++)
            {
                if (trackableList[m].getName() == theTrackerName)
                    trackableList[m].setTracked(isTracked);
            }

            if ((isTracked && allUntracked) || !allUntracked)
            {
                checkAllUntracked();
            }


            //Debug.LogWarning("Updated Traked List: " + theTrackerName + isTracked.ToString());
            //Debug.LogWarning("Is all untracked:" + allUntracked);

            if(trackableList.Length == 1)
            {
                
                tracking = s;
                DoCheck();
                
            }
           
        }
        catch(System.NullReferenceException e)
        {
            Debug.Log("Suppresed Error: "+ e.Message);
        }
    }

    public void DoCheck()
    {
        theController.changeWorldVisible(allUntracked);
    }


}


/// <summary>
/// Struct that helps keeping the information of singular targetable trackers.
/// Each struct represents the status of an individual target.
/// </summary>
public struct TrackerObj
{
    string name;
    bool tracked;

    public void setName(string newName)
    {
        name = newName;
    }

    public void setTracked(bool newTrack)
    {
        tracked = newTrack;
    }

    public bool isTracked()
    {
        return tracked;
    }

    public string getName()
        {
            return name;
        }
    }