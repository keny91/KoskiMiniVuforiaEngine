using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Vuforia;
using System;

[RequireComponent(typeof(SoundController))]

public class GameControllerScript : MonoBehaviour 
    {


    CrossPlatformInputManager.VirtualAxis theAxis;



    public Text LvlNumber;
    protected Text NameText;
    
    public Transform Respawn;
    protected Transform EndGame;
    protected Transform CheckPoint1;


    public bool gameStarted = false;  // The game has passed the building phase ?
    public bool gameRunning = false;  // The game is running ?
    public bool gameStopped = true;
    public bool gameEnded = false;

    public int subSceneIndex = 1;
    bool buildingphase = false;

    //public LoadingScreen theLoadingScreen;



    //Hidden world
    public bool WorldHidden;
    public GameObject WorldObject;
    public bool trackerDetected = false;
    public HideTrackerOnLost theTracker;

    public SoundController theSoundController;

    //initial stats
    public int Lives = 3;
    public int score = 0;
    protected int max_score;
    protected int score_th = 15;

    // Panel that should appear once the level has been completed
    protected GameObject GameMenuLose;
    protected GameObject GameMenuWin;
    protected GameObject GameUIinGame;
    protected GameObject LostTrackerUI;
    public GameObject GamePause;
    CanvasGroupDisplay [] UIs;

    GameObject NewBlocksContainer;
    GameObject SubScenesContainer;
    SubSceneController PresentSubScene;
    int numberSubscenes;
    string prefix = "Sub";


    // Collectibles
    protected GameObject[] coinObject; 
    public int collectibleRemaining;
    protected bool allCollectiblesCollected;





    /// <summary>
    ///  Start and unpause the game, enable movement, set the proper UI assets.
    /// </summary>
    public void StartGame()
    {


        LostTrackerUI.GetComponent<CanvasGroupDisplay>().Hide();

        gameStarted = true;
        changeWorldVisible(false);
        //PreSceneControl.GetPreScene().makePreSceneVisible(false);

    }

    
    public void EnableLostUI()
    {

        for (int i = 0; i < UIs.Length; i++)
        {
            if (UIs[i].transform.name == LostTrackerUI.transform.name)
                UIs[i].Show();
            else
            {
                UIs[i].Hide();
            }
        }
        gameRunning = false;
    }


    public void DisableLostUI()
    {
        string thename;
        if (gameStarted)
            thename = "InGameUI";
        else
            thename = "PreSceneUI";

            for (int i = 0; i < UIs.Length; i++)
            {
                if (UIs[i].transform.name == thename)
                {
                //Debug.LogError("Enabling UI: "+ UIs[i].transform.name);
                UIs[i].Show();
                }
                    
                else
                {
                    UIs[i].Hide();
                }
            }
        gameRunning = true;
    }


    /// <summary>
    /// Display UI elements and functionality to show how to physically build the scene with blocks.
    /// </summary>
    private void LoadPreScene()
    {
        // Set the interfaces to their proper states
        GameMenuWin.GetComponent<ScoreCanvasControl>().Hide();
        GameMenuLose.GetComponent<ScoreCanvasControl>().Hide();
        GameUIinGame.GetComponent<ScoreCanvasControl>().Hide();
        GamePause.GetComponent<ScoreCanvasControl>().Hide();
        LostTrackerUI.GetComponent<CanvasGroupDisplay>().Show();

        gameStarted = false;
        //PreSceneControl.GetPreScene().InitPreScene();
        //PreSceneControl.GetPreScene().makePreSceneVisible(true);
    }






    /// <summary> 
    /// A collectible has been picked. Depending on its properties modify game variables
    /// </summary>
    /// <param name="collect">The type collectible stores a value or a powerup.</param>
    public void collectibleCollected(Collectible collect)
    {


        if (!collect.isPowerUp)
        {            

            modifyScore(collect.value);
            theSoundController.playClip(theSoundController.SoundCoinCollected);
        }          
        else
        {
            // depending on the powerup collected
            // This only runs in case of global powerup and not player modifiers
            switch (collect.powerUpType)
            {
                // Null powerUp
                case 0:
                    break;
                default:
                    break;

                case 1:
                    ChangeLiveCount(+1);
                    break;
                
                    // ... MORE IN THE FUTURE
            }
        }

        if (collect.requiredToPassLevel)
        {
            collectibleRemaining--;
            if (collectibleRemaining == 0)
                allCollectiblesCollected = true;
        }
            
    }


    



    /*******************************************************/
    /*****************    PLAY/PAUSE    ********************/
    /*******************************************************/
    /// <summary>
    /// Resume the process when paused
    /// </summary>
    public void Resume()
    {
        gameRunning = true;
    }

    /// <summary>
    /// Pause any operations
    /// </summary>
    public void Pause()
    {
        theSoundController.audioS.Stop();
        gameRunning = false;
    }


    /// <summary>
    /// Stop the game when target lost
    /// </summary>
    public void Stop()
    {
        gameStopped = true;
    }
    /// <summary>
    /// Resume on target reapeared
    /// </summary>
    public void Run()
    {
        gameStopped = false;
    }


    public void makeSceneVisible(bool visible)
    {
        // Originally disable the scene render
        Renderer[] rendererComponents = WorldObject.GetComponentsInChildren<Renderer>(true);
        //Collider[] colliderComponents = WorldObject.GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = visible;
        }

    }




    /// <summary>
    /// Hide the elements located on the worldScene
    /// </summary>
    public void changeWorldVisible(bool untracked)
    {
        if (!gameEnded)
        {
            if (!untracked) // Do only if it changes status
            {
                //Debug.LogError("changing detection");
                if(theTracker.tracking != TrackableBehaviour.Status.EXTENDED_TRACKED)
                    DisableLostUI();
                if (gameStarted)
                {
                    try
                    {
                        makeSceneVisible(!untracked);

                        if (untracked)
                            Stop();
                        else
                            Run();

                        WorldHidden = untracked;
                    }
                    catch (UnassignedReferenceException e)
                    {
                        Debug.Log("Suppressed Error: " + e);
                    }
                }

                else
                {
                    //PreSceneControl.GetPreScene().makePreSceneVisible(!untracked);
                }

            }
            else if (gameRunning)
            {
                EnableLostUI();
                if (gameStarted)
                {
                    makeSceneVisible(!untracked);
                }
                else
                {
                    //PreSceneControl.GetPreScene().makePreSceneVisible(!untracked);
                }
            }


            else
            {
                if (gameStarted)
                {
                    makeSceneVisible(!untracked);
                }
                else
                {
                    PreSceneControl.GetPreScene().makePreSceneVisible(!untracked);
                }
            }
        }
        // WorldObject.FIIIIINISHTHISSHIT;
    }




    /*******************************************************/
    /***************** SCORES AND LIVES ********************/
    /*******************************************************/

    /// <summary> 
    /// Check if the game has more 0 lives.
    /// </summary>
    protected bool Alive()
    {
        if (Lives == 0)
            return false;
        else
            return true;
    }


    /// <summary> Simple method to modify the score. If the score beats a certain threshold and event is activated.
    /// <para>Positive integers create an increment while negative ones decrement it.</para>
    /// <param name="int value"> Amount incremented to the score.</param>
    /// </summary>
    public void modifyScore(int value)
    {
        score += value;
        GameUIinGame.GetComponent<ScoreCanvasControl>().changeScoreSprite(score);
        if (score > score_th)
        {
            ChangeLiveCount(1);
            theSoundController.playClip(theSoundController.Sound1Up);
        }
            
    }

    /// <summary> Simple method to modify the life count. If the score beats a certain threshold and event is activated.
    /// <para>If the count reaches zero, another event will be triggered.</para>
    /// </summary>
    /// <param name="int value"> Amount incremented to the live system.</param>
    /// <seealso cref="GameControllerScript.Alive() "/>
    /// <seealso cref="ScoreCanvasControl.changeLifeSprite(int LiveNumber) "/>

    public void ChangeLiveCount(int value)
    {
        Lives += value;
        if (Alive())    // If alive 
        {
            GameUIinGame.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives); //Update Ingame UI
        }
        else
        {
            // make fail menu visible
            OnDefeat();
            GameUIinGame.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);
            Pause();
        }

    }


   

    // Use this for initialization
	public void Start () {

        // Box Collider init
        

        // Init params
        score = 0;
        allCollectiblesCollected = false;

        //SetGoal and respawn positions
        Respawn = (Transform)GameObject.Find("RespawnPosition").GetComponent<Transform>();
        WorldObject = GameObject.Find("WorldScene");

        try
        {
            LoadingScreen.GetObject().GetComponent<LoadingScreen>().HideLoadingScreen(); 

        }
        catch(NullReferenceException e)
        {
            Debug.LogError("No LoadingScreen Object" + e.Message);
        }

        // Get all set of hiddable UI elements
        GameMenuWin = (GameObject)GameObject.Find("WinMenu");
        GameMenuLose = (GameObject)GameObject.Find("LossMenu");
        GameUIinGame = (GameObject)GameObject.Find("InGameUI");
        GamePause = (GameObject)GameObject.Find("PauseMenu");
        LostTrackerUI = (GameObject)GameObject.Find("LostTrackingMenu");
        NewBlocksContainer = (GameObject)GameObject.Find("NewBlocksContainer"); 
        SubScenesContainer = (GameObject)GameObject.Find("SubscenesContainer"); ;
        numberSubscenes = SubScenesContainer.transform.childCount;




        UIs = GameObject.Find("UI").transform.GetComponentsInChildren<CanvasGroupDisplay>();

        theTracker = GameObject.Find("Targets").GetComponent<HideTrackerOnLost>();

        GameUIinGame.GetComponent<ScoreCanvasControl>().Start();  // Force to initialize the element
        GameUIinGame.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);

        gameRunning = true;
        gameStarted = true;

        // Set up maximum possible score and total of items required to pass the level
        coinObject = GameObject.FindGameObjectsWithTag("Collectable");
        collectibleRemaining = 0;

        CheckNumberOfCollectables();
        /*
        for (int i =0; i < coinObject.Length; i++)
        {
            Collectible theCollectible = (Collectible)coinObject[i].GetComponent(typeof(Collectible));
            if (theCollectible.requiredToPassLevel)
                collectibleRemaining++;

            if (theCollectible.value > 0)
                max_score += theCollectible.value;

        }
        */

        // Originally disable the scene render
        makeSceneVisible(false);



        GameObject objectRespawn = GameObject.Find("RespawnPosition");
        Respawn = objectRespawn.transform;


        // Start on the PreSceneScreen
        
        //LoadPreScene(); //
        

        theSoundController = (SoundController)GetComponent<SoundController>();
        theSoundController.PlayBackGroundMusic();
        InitSubScene(subSceneIndex);


    }



    void CheckNumberOfCollectables()
    {

        collectibleRemaining = 0;
        for (int i = 0; i < coinObject.Length; i++)
        {
            Collectible theCollectible = (Collectible)coinObject[i].GetComponent(typeof(Collectible));
            if (theCollectible.requiredToPassLevel)
                collectibleRemaining++;

            if (theCollectible.value > 0)
                max_score += theCollectible.value;

        }
    }





    /*****************   SubSceneControll   *******************/
    
    void InitSubScene(int id)
    {
        if(id <= numberSubscenes)
        {
            buildingphase = false;
            Debug.LogWarning(prefix + subSceneIndex);
            if (SubScenesContainer.transform.FindChild(prefix + subSceneIndex))
            {
                PresentSubScene = SubScenesContainer.transform.FindChild(prefix + subSceneIndex).GetComponent<SubSceneController>();
                //    PresentSubScene.Start();
                PresentSubScene.InitSubScene();
            }
        }
        else
        {
            Debug.LogWarning("No more subScenes to be loaded.");
        }

    }

    void ChangeToBuildingStatus()
    {

    }



    /// <summary>
    /// Occlude everything in everysingle subscene
    /// </summary>
    void OcludeAllSubscenes()
    {
        int numberOfChildsSubScenes = SubScenesContainer.transform.childCount;
        
        for (int i = 0; i < numberOfChildsSubScenes; i++)
        {
            SubScenesContainer.transform.FindChild(prefix + i).GetComponent<SubSceneController>().HideAllContent();
            /*
            // Determine the number of blocks in the subscene
            int numberOfElements = SubScenesContainer.transform.FindChild(prefix + i).transform.FindChild("Placeholders").childCount;
            // Disable all renderers for both 
            for (int j = 0; j < numberOfElements; j++)
            {
                SubScenesContainer.transform.FindChild(prefix + i).transform.FindChild("Placeholders").GetChild(j).GetComponent<Renderer>().enabled = false;
                SubScenesContainer.transform.FindChild(prefix + i).transform.FindChild("Placeholders").GetChild(j).GetComponent<BoxCollider>().enabled = false;
                SubScenesContainer.transform.FindChild(prefix + i).transform.FindChild("Blocks").GetChild(j).GetComponent<Renderer>().enabled = false;
                SubScenesContainer.transform.FindChild(prefix + i).transform.FindChild("Blocks").GetChild(j).GetComponent<BoxCollider>().enabled = false;
            }
*/

        }
            int numberOfChilds = SubScenesContainer.transform.FindChild(prefix + subSceneIndex).transform.FindChild("Blocks").childCount;
    }


   public void ReachedProximityTrigger()
    {
        SubScenesContainer.transform.FindChild(prefix + subSceneIndex).transform.FindChild("placeholders").GetComponentInChildren<Renderer>().enabled = true;
    }


    // Update is called once per frame
    protected void Update () {

        //if(trackerDetected)


    }

    /*******************************************************/
    /*****************  Victory/Defeat  ********************/
    /*******************************************************/
    /// <summary>
    /// Check Victory Conditions
    /// </summary>
    public void GoalReached()
    {
        if (allCollectiblesCollected)
        {
            gameEnded = true;
            OnVictory();
        }

    }

    /// <summary>
    /// Display Victory Screen
    /// </summary>
    void OnVictory()
    {
        Pause();  // Pause the game
        theSoundController.audioS.Stop();
        theSoundController.playClip(theSoundController.SoundVictory);
        //HideUI();
        GameMenuWin.GetComponent<ScoreCanvasControl>().Show();     //Make Win Menu Visible
        GameUIinGame.GetComponent<ScoreCanvasControl>().Hide();     //Make player controls Menu InVisible
        // Update data
        GameMenuWin.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);
        GameMenuWin.GetComponent<ScoreCanvasControl>().changeScoreSprite(score);
    }


    /// <summary>
    /// Display PauseMenu on screen
    /// </summary>
    public void OnPauseButton()
    {
        Pause();
        GamePause.GetComponent<ScoreCanvasControl>().Show();     //Make Pause Menu Visible
        GameUIinGame.GetComponent<ScoreCanvasControl>().Hide();     //Make the player controls InVisible
        GamePause.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);
        GamePause.GetComponent<ScoreCanvasControl>().changeScoreSprite(score);
    }


    /// <summary>
    /// Display controls on resume
    /// </summary>
    public void OnResumeButton()
    {
        Resume();
        GamePause.GetComponent<ScoreCanvasControl>().Hide();     //Make Defeat Menu Visible
        GameUIinGame.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);
        GameUIinGame.GetComponent<ScoreCanvasControl>().changeScoreSprite(score);
        GameUIinGame.GetComponent<ScoreCanvasControl>().Show();     //Make Defeat Menu InVisible

    }

    public void OnPressSceneContinueButton()
    {
        //SceneManager.LoadScene("PreScene1");
        //PreSceneControl.GetPreScene().makePreSceneVisible(false);
        PresentSubScene.TransferPhysicalBlocks2Scene();
        PresentSubScene.ExitSubScene();
        subSceneIndex += 1;
        InitSubScene(subSceneIndex);
        
    }

    /// <summary>
    /// Execute level end OnDefeat
    /// </summary>
    protected void OnDefeat()
    {
        gameEnded = true;
        Pause();  // Pause the game
        theSoundController.audioS.Stop();
        theSoundController.playClip(theSoundController.SoundDefeat);
        GameMenuLose.GetComponent<ScoreCanvasControl>().Show();     //Make Defeat Menu Visible
        GameUIinGame.GetComponent<ScoreCanvasControl>().Hide();     //Make Defeat Menu InVisible
        // Update data
        GameMenuLose.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);
        GameMenuLose.GetComponent<ScoreCanvasControl>().changeScoreSprite(score);



    }




    /*******************************************************/
    /*****************  Buttons/Actions  *******************/
    /*******************************************************/


    protected void ClearAxis()
    {
        string horizontalAxisName = "Horizontal";
        string verticalAxisName = "Vertical";

        if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);

        }

        if (CrossPlatformInputManager.AxisExists(verticalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
        }
    }

    public void OnRetryButton()
    {
        ClearAxis();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenuButton()
    {
        ClearAxis();
        SceneManager.LoadScene("MainMenuTrue");
    }



    public void OnNextButton()
    {
        ClearAxis();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    

}


