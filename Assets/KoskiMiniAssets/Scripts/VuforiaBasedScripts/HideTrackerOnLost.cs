using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class HideTrackerOnLost : MonoBehaviour
                                                
{

    TrackerObj[] trackableList;
    PlayerController thePlayer;
    public GameControllerScript theController;
    public bool allUntracked = true;

    // ObjectTarget trackable = 
    // Use this for initialization
    void Start() {

        //GameObject[] theList = GameObject.FindGameObjectsWithTag("Trackable");
        int listLenght = transform.childCount;


        thePlayer = (PlayerController)GameObject.Find("Player").GetComponent<PlayerController>();
        theController = (GameControllerScript)GameObject.Find("Player").GetComponent<GameControllerScript>();

        //trackableList = new TrackerObj[theList.Length];
        trackableList = new TrackerObj[listLenght];
        for (int m = 0; m < listLenght; m++)
        {
            trackableList[m].setName(transform.GetChild(m).name);
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
                //Debug.LogWarning("AllUNTRACKED SET TO FALSE");
                break;
            }
                
        }
    }


    // Update is called once per frame
    public void UpdateTrackedList(string theTrackerName, bool isTracked)
        {

        

        for (int m = 0; m < trackableList.Length; m++)
        {
            if(trackableList[m].getName() == theTrackerName)
                trackableList[m].setTracked(isTracked);
        }

        if ((isTracked && allUntracked) || !allUntracked)
        {
            checkAllUntracked();
        }


        //Debug.LogWarning("Updated Traked List: " + theTrackerName + isTracked.ToString());
        //Debug.LogWarning("Is all untracked:" + allUntracked);
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