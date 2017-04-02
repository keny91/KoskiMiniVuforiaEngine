using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSceneController : MonoBehaviour {

    public bool TargetReached = false;
    public bool isActive = false;
    protected Transform thePlayer;
    protected Transform position;
    public float distance2Player;
    public float distanceTH = 20f;

    GameObject[] placeHolders;
    GameObject[] otherAssets;
    GameObject[] physicalblocks;
    GameObject VisualReference;
    Transform BlockDirectory;
    Transform OthersDirectory;
    int blockCount;
    int othersCount;
    public CanvasGroupDisplay theSubSceneUI;

    public void InitSubScene()
    {
        TargetReached = false;
        isActive = true;
        thePlayer = GameObject.Find("Player").transform;
        VisualReference = transform.FindChild("RefItem").gameObject;
        DisplayVisualReference();

        // Display reference marker
    }


    public void ExitSubScene()
    {


        TargetReached = false;
        isActive = false;
        
        // Hide all components
    }

    /**********************************/

    void DisplayVisualReference()
    {
        //VisualReference.GetComponentInChildren<Renderer>().enabled = true;

        Renderer[] rendererComponents = VisualReference.GetComponentsInChildren<Renderer>(true);
        //Collider[] colliderComponents = WorldObject.GetComponentsInChildren<Collider>(true);
        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

    }


    void DisplayOthers()
    {
        //VisualReference.GetComponentInChildren<Renderer>().enabled = true;

        Renderer[] rendererComponents;
        // Enable rendering:
        for (int i = 0; i < blockCount; i++)
        {
            rendererComponents = otherAssets[i].GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }
        }
    }


    void HideOthers()
    {
        //VisualReference.GetComponentInChildren<Renderer>().enabled = true;

        Renderer[] rendererComponents;
        // Enable rendering:
        for (int i = 0; i < blockCount; i++)
        {
            rendererComponents = otherAssets[i].GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }
        }
    }



    void HideVisualReference()
    {
        //      VisualReference.GetComponentInChildren<Renderer>().enabled = false;
        Renderer[] rendererComponents = VisualReference.GetComponentsInChildren<Renderer>(true);
        //Collider[] colliderComponents = WorldObject.GetComponentsInChildren<Collider>(true);
        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }
    }

    void DisplayPlaceHolders()
    {
        for (int i = 0; i < blockCount; i++)
        {
            placeHolders[i].GetComponent<Renderer>().enabled = true;
            placeHolders[i].GetComponent<BoxCollider>().enabled = true;
        }
    }

    void HidePlaceHolders()
    {
        for (int i = 0; i < blockCount; i++)
        {
            placeHolders[i].GetComponent<Renderer>().enabled = false;
            placeHolders[i].GetComponent<BoxCollider>().enabled = false;
        }
    }

    void DisplayPhysicalBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            physicalblocks[i].GetComponent<Renderer>().enabled = true;
            physicalblocks[i].GetComponent<BoxCollider>().enabled = true;
        }
    }

    void HidePhysicalBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            physicalblocks[i].GetComponent<Renderer>().enabled = false;
            physicalblocks[i].GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void TransferPhysicalBlocks2Scene()
    {
        HidePlaceHolders();
        DisplayPhysicalBlocks();
        DisplayOthers();

        for (int i = 0; i < blockCount; i++)
        {
            physicalblocks[i].transform.SetParent(BlockDirectory);
        }

        for(int j = 0;j< othersCount; j++)
            otherAssets[j].transform.SetParent(OthersDirectory);

        theSubSceneUI.Hide();
    }

    void HideAllContent()
    {
        HidePhysicalBlocks();
        HidePlaceHolders();
        HideVisualReference();
        HideOthers();
    }



    /// <summary>
    /// Check the relative position of the player according to the 
    /// </summary>
    protected void checkDistance()
    {
        distance2Player = (thePlayer.position - position.position).magnitude;
        if (distance2Player <= distanceTH)
        {
            TargetReached = true;
        }
        else
            TargetReached = false;
    }


    void SetBuildingPhase()
    {
        theSubSceneUI.Show();
        HideVisualReference();
        DisplayPlaceHolders();
    }

    void RollBackToRefStatus()
    {
        theSubSceneUI.Hide();
        DisplayVisualReference();
        HidePlaceHolders();
    }

    /*******************************************/

    // Use this for initialization
   public void Start () {
        TargetReached = false;
        thePlayer = GameObject.Find("Player").transform;


        if (transform.FindChild("Placeholders").childCount == transform.FindChild("Blocks").childCount)
        {
            blockCount = transform.FindChild("Placeholders").childCount;
            placeHolders = new GameObject[blockCount];
            physicalblocks = new GameObject[blockCount];
            othersCount = transform.FindChild("Others").childCount;

            for(int j =0; j < othersCount; j++)
            {
                otherAssets[j] = transform.FindChild("Others").GetChild(j).transform.gameObject;
            }
                

            for (int i = 0; i< blockCount; i++)
            {
                placeHolders[i] = transform.FindChild("Placeholders").GetChild(i).transform.gameObject;
                physicalblocks[i] = transform.FindChild("Blocks").GetChild(i).transform.gameObject;
            }
            VisualReference = transform.FindChild("RefItem").gameObject;
            BlockDirectory = (Transform)GameObject.Find("NewBlocksContainer").transform;
            OthersDirectory = (Transform)GameObject.Find("OtherContainer").transform;


            theSubSceneUI = GameObject.Find("PreSceneUI").GetComponent<CanvasGroupDisplay>();
            theSubSceneUI.Hide();
            HideAllContent();
            position = transform.FindChild("ReferencePoint");



        }

        else
                {
                    Debug.LogError("Missmatch in the number of placeHolder and blocks at Subscene:" + transform.name);
                    Application.Quit();
                }

    }
	
	// Update is called once per frame
	void Update () {

        if (isActive)
        {
            checkDistance();
            if (TargetReached)
            {
                SetBuildingPhase();
            }
            else if (!TargetReached)
            {
                RollBackToRefStatus();
            }
        }
        // get to building phase


    }





}
