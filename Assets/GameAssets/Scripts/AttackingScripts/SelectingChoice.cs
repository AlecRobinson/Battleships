using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectingChoice : MonoBehaviour
{
    public static GamePlayManager managerScript;
    public static BuildingSystem buildingScript;

    [HideInInspector] private Dictionary<string, Vector3> allTiles = new Dictionary<string, Vector3>();
    [HideInInspector] public Dictionary<string, List<Vector3>> computerShipPos = new Dictionary<string, List<Vector3>>();

    public GameObject inputFeild;
    public bool hasHit;
    private Vector3 guessPos;

    private Dictionary<string, int> shipHitsCounter = new Dictionary<string, int>();
    private int numShipsSunk;

    //Prefabs for hitting and missing
    public GameObject hit;
    public GameObject miss;
    public GameObject missile;

    void Awake()
    {
        managerScript = this.GetComponent<GamePlayManager>();
        buildingScript = this.GetComponent<BuildingSystem>();
        allTiles = new Dictionary<string, Vector3>(buildingScript.allTiles);
        //Creates the computers ships positions
        CreateComputerShipPos();

        //Counters on how many hit each ship has
        shipHitsCounter.Add("PatrolBoat", 0);
        shipHitsCounter.Add("Submarine", 0);
        shipHitsCounter.Add("Destroyer", 0);
        shipHitsCounter.Add("Battleship", 0);
        shipHitsCounter.Add("Carrier", 0);
    }

    //Button to check if ship has been hit or missed
    public void CheckForHit()
    {
        hasHit = false;
        guessPos = new Vector3(0, 0, 0);

        bool got = allTiles.TryGetValue(inputFeild.GetComponent<TMP_InputField>().text, out guessPos);                     //Getting corrosponding vector if text was correct
        if(got == true)          //Checking if a correct value was entered
        {
            allTiles.Remove(inputFeild.GetComponent<TMP_InputField>().text);
            //Looping all ship pos to see if the guess was a hit
            foreach (var ship in computerShipPos)
            {
                foreach (var shipPos in ship.Value)
                {
                    if (guessPos == shipPos)
                    {
                        Debug.Log("Hit" + ship.Key + guessPos);
                        Instantiate(hit, guessPos + new Vector3(155, 10, 5), Quaternion.identity);          //155 so it can be displayed on the correct map
                        Instantiate(missile, guessPos + new Vector3(155, 100, 5), Quaternion.identity);     //Spawning missle for visual effects
                        hasHit = true;
                        //Incrementing the hit counter
                        shipHitsCounter.TryGetValue(ship.Key, out int val);
                        shipHitsCounter[ship.Key] = val + 1;
                        //Checking if ship has sunk
                        if (val + 1 == ship.Value.Count)
                        {
                            Debug.Log("Ship sunk: " + ship.Key);
                            numShipsSunk += 1;
                            managerScript.recentSink = true;
                        }

                        managerScript.AttackMode(true);
                        CheckForWin();
                    }
                }
            }

            if (!hasHit)
            {
                Debug.Log("Miss");
                Instantiate(miss, guessPos + new Vector3(155, 15, 5), Quaternion.identity);
                Instantiate(missile, guessPos + new Vector3(155, 100, 5), Quaternion.identity);
                managerScript.AttackMode(false);
                CheckForWin();
            }
            //Resets text box
            inputFeild.GetComponent<TMP_InputField>().text = "";
        }
        else        //Catch for incorrect values being entered
        {
            Debug.Log("Incorrect value");
            inputFeild.GetComponent<TMP_InputField>().text = "IncorrectVal";
        }
    }

    //Quick method for creating computers ships
    private void CreateComputerShipPos()
    {
        //Picking random setup
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0)
        {
            AIShipPosChoice1();
        }
        else
        {
            AIShipPosChoice2();
        }
    }

    //Checks if player has won game
    public void CheckForWin()
    {
        if (numShipsSunk == 5)
        {
            managerScript.gameOverName = "Player";
            managerScript.gameOver = true;
        }
    }

    //Setting up player ship pos. Game allows for more position settings to be added to increase the level of unknown
    private void AIShipPosChoice1()
    {
        List<Vector3> shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(0, 0, 90));
        shipPos.Add(new Vector3(10, 0, 90));
        computerShipPos.Add("PatrolBoat", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(0, 0, 80));
        shipPos.Add(new Vector3(10, 0, 80));
        shipPos.Add(new Vector3(20, 0, 80));
        computerShipPos.Add("Submarine", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(0, 0, 70));
        shipPos.Add(new Vector3(10, 0, 70));
        shipPos.Add(new Vector3(20, 0, 70));
        computerShipPos.Add("Destroyer", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(0, 0, 60));
        shipPos.Add(new Vector3(10, 0, 60));
        shipPos.Add(new Vector3(20, 0, 60));
        shipPos.Add(new Vector3(30, 0, 60));
        computerShipPos.Add("Battleship", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(0, 0, 50));
        shipPos.Add(new Vector3(10, 0, 50));
        shipPos.Add(new Vector3(20, 0, 50));
        shipPos.Add(new Vector3(30, 0, 50));
        computerShipPos.Add("Carrier", shipPos);
    }
    private void AIShipPosChoice2()
    {
        List<Vector3> shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(40, 0, 80));
        shipPos.Add(new Vector3(50, 0, 80));
        computerShipPos.Add("PatrolBoat", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(10, 0, 70));
        shipPos.Add(new Vector3(10, 0, 80));
        shipPos.Add(new Vector3(10, 0, 90));
        computerShipPos.Add("Submarine", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(50, 0, 50));
        shipPos.Add(new Vector3(60, 0, 50));
        shipPos.Add(new Vector3(70, 0, 50));
        computerShipPos.Add("Destroyer", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(0, 0, 40));
        shipPos.Add(new Vector3(10, 0, 40));
        shipPos.Add(new Vector3(20, 0, 40));
        shipPos.Add(new Vector3(30, 0, 40));
        computerShipPos.Add("Battleship", shipPos);
        shipPos = new List<Vector3>();
        shipPos.Add(new Vector3(80, 0, 30));
        shipPos.Add(new Vector3(80, 0, 40));
        shipPos.Add(new Vector3(80, 0, 50));
        shipPos.Add(new Vector3(80, 0, 60));
        computerShipPos.Add("Carrier", shipPos);
    }
}



