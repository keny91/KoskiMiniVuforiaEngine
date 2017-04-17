using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Attach this scene to an individual to a subScene element.
/// </summary>
public class SubSceneController : MonoBehaviour {

    public bool TargetReached = false;
    public bool isActive = false;
    protected Transform thePlayer;
    protected Transform position;
    public float distance2Player;
    public float distanceTH = 20f;

    bool built;

    GameObject[] placeHolders;
    GameObject[] otherAssets;
    GameObject[] physicalblocks;
    GameObject VisualReference;
    Transform BlockDirectory;
    Transform OthersDirectory;
    int blockCount;
    int othersCount;
    public CanvasGroupDisplay theSubSceneUI;
    GameControllerScript theController;



    /// <summary>
    /// Initialize a subscene tracing its internal elements and creating the relations. Then start by displaying the visual reference.
    /// </summary>
    public void InitSubScene()
    {
        TargetReached = false;
        isActive = true;
        thePlayer = GameObject.Find("Player").transform;
        theController = GameObject.Find("GameControl").GetComponent<GameControllerScript>();
        VisualReference = transform.FindChild("RefItem").gameObject;
        DisplayVisualReference();

        // Display reference marker
    }

    /// <summary>
    /// Make the scene invisible by disabling the bools that enable the process.
    /// </summary>
    public void ExitSubScene()
    {

       // theController.changeWorldVisible(false, false);
        StartCoroutine(theController.FadeIn());
        HideVisualReference();
        TargetReached = false;
        isActive = false;
        
        // Hide all components -> not anymore, done when inactive
    }



    /// <summary>
    /// Display and enable the elements in the child "VisualReference"
    /// </summary>
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


    /// <summary>
    /// Display and enable the elements in the child "OtherAssets"
    /// </summary>
    void DisplayOthers()
    {

        Renderer[] rendererComponents;
        // Enable rendering of all children.
        for (int i = 0; i < othersCount; i++)
        {
            rendererComponents = otherAssets[i].GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }
        }
    }


    /// <summary>
    /// Display and enable the elements in the child "placeHolders"
    /// </summary>
    void DisplayPlaceHolders()
    {
        for (int i = 0; i < blockCount; i++)
        {
            Renderer[] rendererComponents = placeHolders[i].GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }
            placeHolders[i].GetComponent<BoxCollider>().enabled = true;
        }
    }

    /// <summary>
    /// Display and enable the elements physical blocks. Note that the elements have to be paired with the same number of elements.
    /// </summary>
    void DisplayPhysicalBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            physicalblocks[i].GetComponent<Renderer>().enabled = true;
            physicalblocks[i].GetComponent<BoxCollider>().enabled = true;
        }
    }

    /***************************/

    /// <summary>
    /// Hide and disable the elements in the child "OtherAssets"
    /// </summary>
    void HideOthers()
    {
        Renderer[] rendererComponents;
        // Enable rendering of all children.
        for (int i = 0; i < othersCount; i++)
        {
            rendererComponents = otherAssets[i].GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }
        }
    }


    /// <summary>
    /// Hide and disable the elements in the child "OtherAssets"
    /// </summary>
    void HideVisualReference()
    {
        Renderer[] rendererComponents = VisualReference.GetComponentsInChildren<Renderer>(true);
        // Disable rendering of all children.
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }
    }


    /// <summary>
    /// Hide and disable the elements in the child "placeHolders"
    /// </summary>
    void HidePlaceHolders()
    {
        for (int i = 0; i < blockCount; i++)
        {
           // placeHolders[i].GetComponent<Renderer>().enabled = false;
            Renderer[] rendererComponents = placeHolders[i].GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }
            
            placeHolders[i].GetComponent<BoxCollider>().enabled = false;
        }
    }


    /// <summary>
    /// Hide and disable the Blocks.
    /// </summary>
    void HidePhysicalBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            physicalblocks[i].GetComponent<Renderer>().enabled = false;
            physicalblocks[i].GetComponent<BoxCollider>().enabled = false;
        }
    }


    /// <summary>
    ///     Make all of the different subscene components invisible.
    /// </summary>
    public void HideAllContent()
    {
        HidePhysicalBlocks();
        HidePlaceHolders();
        HideVisualReference();
        HideOthers();
    }


/**********************/

    /// <summary>
    /// Move the physical blocks by making the external Physical block container their new parent.
    /// </summary>
    public void TransferPhysicalBlocks2Scene()
    {
        HidePlaceHolders();
        DisplayPhysicalBlocks();
        DisplayOthers();

        for (int i = 0; i < blockCount; i++)
        {
            physicalblocks[i].transform.SetParent(BlockDirectory);
        }

        if (othersCount > 0)
        {
            for (int j = 0; j < othersCount; j++)
                otherAssets[j].transform.SetParent(OthersDirectory);
        }
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

    /// <summary>
    /// Trigger what happens in the Building phase of the subscene. Change from visual reference to the placeHolders while hiding the world.
    /// </summary>
    void SetBuildingPhase()
    {
        HideVisualReference();
        DisplayPlaceHolders();
        //theController.changeWorldVisible(true, false);
        StartCoroutine(theController.FadeOut());
        //theController.FadeOut();
        //theSubSceneUI.Show();

    }


    /// <summary>
    /// Roll to the previous version of the building phase.
    /// </summary>
    void RollBackToRefStatus()
    {
       // theSubSceneUI.Hide();
        DisplayVisualReference();
        HidePlaceHolders();
    }

    /*******************************************/

    // Use this for initialization
   public void Start () {
        TargetReached = false;
        thePlayer = GameObject.Find("Player").transform;
        built = false;

        if (transform.FindChild("Placeholders").childCount == transform.FindChild("Blocks").childCount)
        {
            blockCount = transform.FindChild("Placeholders").childCount;
            placeHolders = new GameObject[blockCount];
            physicalblocks = new GameObject[blockCount];
            othersCount = transform.FindChild("Others").childCount;
            otherAssets = new GameObject[othersCount];
            Debug.LogWarning(othersCount);

            for(int j = 0; j < othersCount; j++)
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
                built = true;
            }

            else if (!TargetReached && !built)
            {
                //RollBackToRefStatus();
                DisplayVisualReference();
            }
        }
        // get to building phase


    }

}
