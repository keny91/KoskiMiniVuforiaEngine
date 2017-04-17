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

    // Axis controllet
    CrossPlatformInputManager.VirtualAxis theAxis;

    public Text LvlNumber;
    protected Text NameText;
    public Transform Respawn;
    protected Transform EndGame;
    protected Transform CheckPoint1;



    // Game Status
    public bool gameStarted = false;  // The game has passed the building phase ?
    public bool gameRunning = false;  // The game is running ?
    public bool gameStopped = true;
    public bool gameEnded = false;
    public bool changingSceneStatus = false;
    public int subSceneIndex = 1;
    bool buildingphase = false;

    //public LoadingScreen theLoadingScreen;

    //Hidden world
    public bool WorldHidden;
    public GameObject WorldObject;
    public bool trackerDetected = false;
    public HideTrackerOnLost theTracker;
    Vector3 OriginalPosition;


    public SoundController theSoundController;

    //initial stats
    public int Lives = 3;
    public int score = 0;
    protected int max_score;
    protected int score_th = 15;

    // Panel that should appear once the level has been completed
    // Elements inside all scenes
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
    /// Make the worldscene dissapear below a board dedicated to occlude. Has a sinking feeling.
    /// </summary>
    public IEnumerator FadeOut()
    {
        if (!changingSceneStatus)
        {
            changingSceneStatus = true;
            //Debug.LogError("FADE OUT process");
            EnableOccludingCut();
            Vector3 FinalPosition = GameObject.Find("SinkBoardReference").transform.position;
            float alpha = 0f;
            float journeyLength = Vector3.Distance(WorldObject.transform.position, FinalPosition);
            float step;
            while (WorldObject.transform.position.y != FinalPosition.y)
            {
                if (Vector3.Distance(WorldObject.transform.position, FinalPosition) < 5)
                    step = Vector3.Distance(WorldObject.transform.position, FinalPosition);
                else
                    step = Time.deltaTime*300;
                WorldObject.transform.position =Vector3.MoveTowards(WorldObject.transform.position, FinalPosition, step);
                // Reduce alpha by fadeSpeed amount.
                alpha++;
                yield return new WaitForEndOfFrame();

            }
            //Debug.LogError("Arrived");
            changingSceneStatus = false;
        }
       else
        {
            // Nothing for now I guess
        }
    }



    public void EnableOccludingCut()
    {
        GameObject.Find("OcclusionBoard").GetComponent<MeshRenderer>().enabled = true;
    }

    public void DisableOccludingCut()
    {
        GameObject.Find("OcclusionBoard").GetComponent<MeshRenderer>().enabled = false;
    }


    /// <summary>
    /// Opposite to FadeOut() make the scene reapear from a hidden State.
    /// </summary>
    public IEnumerator FadeIn()
    {
        if (!changingSceneStatus)
        {
            changingSceneStatus = true;
            //Debug.LogError("FADE IN");
            SubSceneController theSub = PresentSubScene;
            float alpha = 0f;
            Vector3 FinalPosition = OriginalPosition;
            float journeyLength = Vector3.Distance(WorldObject.transform.position, FinalPosition);
            float step;
            while (WorldObject.transform.position.y != FinalPosition.y)
            {

                if (Vector3.Distance(WorldObject.transform.position, FinalPosition) < 5)
                    step = Vector3.Distance(WorldObject.transform.position, FinalPosition);
                else
                    step = Time.deltaTime * 300;
                WorldObject.transform.position = Vector3.MoveTowards(WorldObject.transform.position, FinalPosition, step);
                // Reduce alpha by fadeSpeed amount.
                alpha++;
                yield return new WaitForEndOfFrame();

            }
            DisableOccludingCut();
            theSub.TransferPhysicalBlocks2Scene();
            changingSceneStatus = false;
        }
        else
        {

        }
    }



    public IEnumerator AlphaWait()
    {
        yield return new WaitForEndOfFrame();
    }


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




    /*******************************************************/
    /*****************   Collectibles   ********************/
    /*******************************************************/

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



    /************************************************************/
    /*****************    World visibility   ********************/
    /************************************************************/

    /// <summary>
    /// When the tracker is lost trigger the lost screen and disable the visible world.
    /// This does not properly work with a multiTracker. 
    /// </summary>
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

    /// <summary>
    /// Opposite to EnableLostUI, make the given UI dissapear as the tracked object has been found.
    /// </summary>
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
    /// Enable or disable the scene object´s renderers.
    /// </summary>
    /// <param name="visible">Enablo or disable (true or false)</param>
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
    public void changeWorldVisible(bool untracked, bool displayUi = true)
    {
        if (!gameEnded)
        {
            if (!untracked) // Do only if it changes status
            {
                //Debug.LogError("changing detection");
                if(true)
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

        if (!displayUi)
            DisableLostUI();

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

        DisableOccludingCut();

        // Init params
        score = 0;
        allCollectiblesCollected = false;

        //SetGoal and respawn positions
        Respawn = (Transform)GameObject.Find("RespawnPosition").GetComponent<Transform>();
        WorldObject = GameObject.Find("WorldScene");
        OriginalPosition = WorldObject.transform.position;


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


        // Get reference to object's material.
        // m_Material = GameObject.Find("Cube").GetComponent<Renderer>().material;

        // Get material's starting color value.
        // m_Color = m_Material.color;


        UIs = GameObject.Find("UI").transform.GetComponentsInChildren<CanvasGroupDisplay>();

        //theTracker = GameObject.Find("Targets").GetComponent<HideTrackerOnLost>();

        GameUIinGame.GetComponent<ScoreCanvasControl>().Start();  // Force to initialize the element
        GameUIinGame.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);

        gameRunning = true;
        gameStarted = true;
        Run();

        // Set up maximum possible score and total of items required to pass the level
        coinObject = GameObject.FindGameObjectsWithTag("Collectable");
        collectibleRemaining = 0;

        CheckNumberOfCollectables(); // Find out the number of collectibles in the scene.

        // Originally disable the scene render
        makeSceneVisible(true);
        StartGame();
        GameUIinGame.GetComponent<ScoreCanvasControl>().Show();

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
    
    /// <summary>
    /// Start a subscene given its index
    /// </summary>
    /// <param name="id">The Id of the scene to be initialized.</param>
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




    /// <summary>
    /// Occlude everything in everysingle subscene
    /// </summary>
    void OcludeAllSubscenes()
    {
        int numberOfChildsSubScenes = SubScenesContainer.transform.childCount;
        
        for (int i = 0; i < numberOfChildsSubScenes; i++)
        {
            SubScenesContainer.transform.FindChild(prefix + i).GetComponent<SubSceneController>().HideAllContent();

        }
            int numberOfChilds = SubScenesContainer.transform.FindChild(prefix + subSceneIndex).transform.FindChild("Blocks").childCount;
    }



    // Update is called once per frame
    protected void Update () {
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


    public void NextSubScene()
    {
        //SceneManager.LoadScene("PreScene1");
        //PreSceneControl.GetPreScene().makePreSceneVisible(false);
        //PresentSubScene.TransferPhysicalBlocks2Scene();
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


