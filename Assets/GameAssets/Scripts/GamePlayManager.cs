using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    //Referances to all scripts and objects of value in game
    public static BuildingSystem buildingScript;
    public static SelectingChoice playerchoiceScript;
    public static AIPlayer AIScript;
    public GameObject refGameObject;
    public GameObject cameraPos1;
    public GameObject cameraPos2;
    public GameObject playerGuessButton;

    public bool gameOver;
    public string gameOverName;
    public string sunkShipName;
    public bool recentSink = false;

    //Variables for UI
    public GameObject playerAttckingPanel;
    public GameObject gameOverPlayer;
    public GameObject gameOverAI;
    public GameObject shipSunkUI;

    void Awake()
    {
        //Getting ref to scripts
        buildingScript = refGameObject.GetComponent<BuildingSystem>();
        playerchoiceScript = refGameObject.GetComponent<SelectingChoice>();
        AIScript = refGameObject.GetComponent<AIPlayer>();
    }

    public void Update()
    {
        //Checking if the game has ended to show a winner
        if (gameOver)
        {
            Time.timeScale = 0;
            ShowWinner();
            gameOver = false;
        }
    }

    public void AttackMode(bool isPlayersTurn)
    {
        //Finding which player is attacking
        if (isPlayersTurn)
        {
            StartCoroutine(playerWaitingFuctions());
        }
        else
        {
            StartCoroutine(AIWaitingFuctions());
        }
    }

    //Function for changing where the camera is 
    public void ChangeCameraPos(Vector3 pos)
    {
        Camera.main.transform.position = pos;           
    }

    //IEnumerators used to allow for waiting before functions happen
    IEnumerator playerWaitingFuctions()
    {
        yield return new WaitForSeconds(2);
        if (recentSink)
        {
            shipSunkUI.SetActive(true);                                     //Displaying if a ship has sunk
            recentSink = false;
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        ChangeCameraPos(cameraPos2.transform.position);                     //Moving camera for other players turn
        playerAttckingPanel.SetActive(true);
        playerGuessButton.SetActive(true);
        shipSunkUI.SetActive(false);
    }
    IEnumerator AIWaitingFuctions()
    {
        playerGuessButton.SetActive(false);
        yield return new WaitForSeconds(3);
        if (recentSink)
        {
            shipSunkUI.SetActive(true);                             //Displaying if a ship has sunk
            GameObject shipGameObject = GameObject.Find(sunkShipName);      //Rotating ship if sunk to show its sunk
            shipGameObject.transform.Rotate(new Vector3(-10, 0, 25));
            recentSink = false;
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        ChangeCameraPos(cameraPos1.transform.position);
        playerAttckingPanel.SetActive(false);                       //Changing the UI depending which players turn it is
        shipSunkUI.SetActive(false);
        yield return new WaitForSeconds(1);
        AIScript.MakeDecision();                                    //Setting the AI to make a decision
    }

    public void ShowWinner()
    {
        //Finding the winner
        if (gameOverName == "Player")
        {
            refGameObject.GetComponent<AudioSource>().Stop();
            playerAttckingPanel.SetActive(false);
            gameOverPlayer.SetActive(true);
            gameOverPlayer.GetComponent<AudioSource>().Play();      //Changing the music
        }
        if (gameOverName == "AI")
        {
            refGameObject.GetComponent<AudioSource>().Stop();
            gameOverAI.SetActive(true);
            gameOverAI.GetComponent<AudioSource>().Play();
        }
    }
}
