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

    public bool gameRunning = true;
    public bool gameStopped = true;

    public LoadingScreen theLoadingScreen;

    //Hidden world
    public bool WorldHidden;
    public GameObject WorldObject;

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
    public GameObject GamePause;



    // Collectibles
    protected GameObject[] coinObject; 
    public int collectibleRemaining;
    protected bool allCollectiblesCollected;

    



    /*******************************************************/
    /*****************   WORLD ACTIONS  ********************/
    /*******************************************************/

       



    /*******************************************************/
    /*****************    COLLECTION    ********************/
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

    /// <summary>
    /// Hide the elements located on the world
    /// </summary>
    public void changeWorldVisible(bool hidTheWorld)
    {

        if (hidTheWorld != WorldHidden) // Do only if it changes status
        {
            try
            {
                Renderer[] rendererComponents = WorldObject.GetComponentsInChildren<Renderer>(true);
                //Collider[] colliderComponents = WorldObject.GetComponentsInChildren<Collider>(true);

                // Enable rendering:
                foreach (Renderer component in rendererComponents)
                {
                    component.enabled = !hidTheWorld;
                }

                if (hidTheWorld)
                    Stop();
                else
                    Run();

                WorldHidden = hidTheWorld;
            }
            catch (UnassignedReferenceException e)
            {
                Debug.Log("Suppressed Error: "+ e);
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
            theLoadingScreen = LoadingScreen.GetObject().GetComponent<LoadingScreen>();
            theLoadingScreen.HideLoadingScreen();
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


        // Set the interfaces to their proper states
        GameMenuWin.GetComponent<ScoreCanvasControl>().Hide();
        GameMenuLose.GetComponent<ScoreCanvasControl>().Hide();
        GameUIinGame.GetComponent<ScoreCanvasControl>().Show();
        GamePause.GetComponent<ScoreCanvasControl>().Hide();

        GameUIinGame.GetComponent<ScoreCanvasControl>().Start();  // Force to initialize the element
        GameUIinGame.GetComponent<ScoreCanvasControl>().changeLifeSprite(Lives);

        //Resume();

        // Set up maximum possible score and total of items required to pass the level
        coinObject = GameObject.FindGameObjectsWithTag("Collectable");
        collectibleRemaining = 0;
        //Debug.Log(coinObject[0].name);

        //Set material to transparent
        /*
                GameObject Object2Compensate = GameObject.Find("board_square_test");
                Material theMat = Object2Compensate.GetComponent<Renderer>().material;
                Color theNewColor = new Color();
                theNewColor.a = 0;
                theMat.color = theNewColor;

                Object2Compensate.GetComponent<Renderer>().receiveShadows = true;
                //Object2Compensate.GetComponent<Material>() = theMat;
                //theMat = (Material)Object2Compensate.AddComponent(typeof(Material));
                //Object2Compensate.AddComponent(theMat);

                */

        for (int i =0; i < coinObject.Length; i++)
        {
            Collectible theCollectible = (Collectible)coinObject[i].GetComponent(typeof(Collectible));
            if (theCollectible.requiredToPassLevel)
                collectibleRemaining++;

            if (theCollectible.value > 0)
                max_score += theCollectible.value;

        }



        GameObject objectRespawn = GameObject.Find("RespawnPosition");
        Respawn = objectRespawn.transform;

        

        theSoundController = (SoundController)GetComponent<SoundController>();
        theSoundController.PlayBackGroundMusic();

        
        //Debug.LogWarning(Lives);

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


    /// <summary>
    /// Execute level end OnDefeat
    /// </summary>
    protected void OnDefeat()
    {
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
        SceneManager.LoadScene("Main");
    }

    public void OnMenuButton()
    {
        ClearAxis();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNextButton()
    {
        ClearAxis();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    

}


