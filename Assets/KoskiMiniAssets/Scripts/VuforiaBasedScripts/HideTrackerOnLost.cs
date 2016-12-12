using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class HideTrackerOnLost : MonoBehaviour
                                                
{

    TrackerObj[] trackableList;
    PlayerController thePlayer;
    public GameControllerScript theController;
    bool allUntracked = true;

    // ObjectTarget trackable = 
    // Use this for initialization
    void Start() {

        GameObject[] theList = GameObject.FindGameObjectsWithTag("Trackable");
        thePlayer = (PlayerController)GameObject.Find("Player").GetComponent<PlayerController>();
        theController = (GameControllerScript)GameObject.Find("Player").GetComponent<GameControllerScript>();

        trackableList = new TrackerObj[theList.Length];

        for(int m = 0; m < theList.Length; m++)
        {
            trackableList[m].setName(theList[m].name);
            trackableList[m].setTracked(false);
        }


    }





    private void checkAllUntracked()
    {
        allUntracked = true;
        for (int m = 0; m < trackableList.Length; m++)
        {
            if (trackableList[m].isTracked())
            {
                allUntracked = false;
                Debug.LogWarning("AllUNTRACKED SET TO FALSE");
                break;
            }
                
        }
    }


    // Update is called once per frame
    public void UpdateTrackedList(string theTrackerName, bool isTracked)
        {

        Debug.LogWarning("Updated Traked List: "+ theTrackerName+ isTracked.ToString());

        for (int m = 0; m < trackableList.Length; m++)
        {
            if(trackableList[m].getName() == theTrackerName)
                trackableList[m].setTracked(isTracked);
        }

        if (!isTracked && !allUntracked)
        {
            checkAllUntracked();
        }
            

        theController.changeWorldVisible(allUntracked);
    }


}

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