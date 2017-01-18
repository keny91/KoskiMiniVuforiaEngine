using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using Vuforia;

//[RequireComponent(typeof(SoundController))]
//[RequireComponent(typeof(GameControllerScript))]

public class MultiplayerGameControlScript : GameControllerScript
{

    // Multiplayer 
    [HideInInspector]
    public List <PlayerInfo> playerList;
    MultiPlayerController[] players;
    int currentNumberofPlayers = 0;
    int maxNumberOfPlayers = 5;
    public int DefaultLives = 3;



    new public void Start()
    {

        // Box Collider init


        // Init params
        score = 0;
        allCollectiblesCollected = false;

        //SetGoal and respawn positions
        Respawn = (Transform)GameObject.Find("RespawnPosition").GetComponent<Transform>();
        WorldObject = GameObject.Find("WorldScene");

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

        // Set up maximum possible score and total of items required to pass the level
        coinObject = GameObject.FindGameObjectsWithTag("Collectable");
        collectibleRemaining = 0;


        for (int i = 0; i < coinObject.Length; i++)
        {
            Collectible theCollectible = (Collectible)coinObject[i].GetComponent(typeof(Collectible));
            if (theCollectible.requiredToPassLevel)
                collectibleRemaining++;

            if (theCollectible.value > 0)
                max_score += theCollectible.value;

        }

        if (playerList == null)
        {
            playerList = new List<PlayerInfo>();
        }


        GameObject objectRespawn = GameObject.Find("RespawnPosition");
        Respawn = objectRespawn.transform;



        theSoundController = (SoundController)GetComponent<SoundController>();
        theSoundController.PlayBackGroundMusic();


        //Debug.LogWarning(Lives);

    }


    public void RefreshPlayerList()
    {
        players = GameObject.Find("PlayersContainer").transform.GetComponentsInChildren<MultiPlayerController>(false);
        currentNumberofPlayers = players.Length;
    }

    /// <summary>
    /// Check if a new player can join.
    /// </summary>
    /// <returns></returns>
    public bool CheckJoinPlayer()
    {
        if (currentNumberofPlayers < maxNumberOfPlayers)
        {
            return true;

        }
        else
            return false;
        }
    

    /// <summary>
    /// Ask for the next player ID when it is created and register a new player
    /// </summary>
    /// <returns>ID of the new player</returns>
    public int RegisterNewPlayer()
    {


        Debug.LogWarning(currentNumberofPlayers);
        currentNumberofPlayers++;
        Debug.LogWarning(currentNumberofPlayers);
        PlayerInfo theNewPlayer = new PlayerInfo();
        theNewPlayer.SetToDefault();
        playerList.Add(theNewPlayer);



        return currentNumberofPlayers;        
    }

    public void updatePlayer()
    {

    }


}



/// <summary>
/// Struct to store the point positions required for the rayCasts.
/// Each of the corners of a 3D box
/// </summary>
public struct PlayerInfo
{
    int id;
    int score;
    int lives;
    bool vulnerable;
    int status;

    public void SetToDefault(){
        score = 0;
        lives = 3;
        vulnerable = false;
        status = 0;
    }
}
