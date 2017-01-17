using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Vuforia;

//[RequireComponent(typeof(SoundController))]
//[RequireComponent(typeof(GameControllerScript))]

public class MultiplayerGameControlScript : GameControllerScript
{


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

        for (int i = 0; i < coinObject.Length; i++)
        {
            Collectible theCollectible = (Collectible)coinObject[i].GetComponent(typeof(Collectible));
            if (theCollectible.requiredToPassLevel)
                collectibleRemaining++;

            if (theCollectible.value > 0)
                max_score += theCollectible.value;

        }

        //playerObject = GameObject.Find("Player");
        //thePlayer = (PlayerController)GameObject.Find("Player").GetComponent<PlayerController>();

        GameObject objectRespawn = GameObject.Find("RespawnPosition");
        Respawn = objectRespawn.transform;



        theSoundController = (SoundController)GetComponent<SoundController>();
        theSoundController.PlayBackGroundMusic();


        //Debug.LogWarning(Lives);

    }

}
