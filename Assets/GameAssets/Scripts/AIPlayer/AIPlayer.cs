using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AIPlayer : MonoBehaviour
{
    public static GamePlayManager managerScript;
    public static BuildingSystem buildingScript;
    public static SelectingChoice playerchoiceScript;
    public GameObject refGameObject;

    private List<string> neighbourTiles;

    [HideInInspector] private Dictionary<string, Vector3> allTiles = new Dictionary<string, Vector3>();
    [HideInInspector] public Dictionary<string, List<Vector3>> playerShipPos = new Dictionary<string, List<Vector3>>();

    private bool randomPick = true;

    public bool hasHit;
    public string lastHit;
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
        playerchoiceScript = refGameObject.GetComponent<SelectingChoice>();
        buildingScript = this.GetComponent<BuildingSystem>();
        allTiles = new Dictionary<string, Vector3>(buildingScript.allTiles);
        playerShipPos = buildingScript.allShipPos;
        neighbourTiles = new List<string>();

        //Counters on how many hit each ship has
        shipHitsCounter.Add("PatrolBoat", 0);
        shipHitsCounter.Add("Submarine", 0);
        shipHitsCounter.Add("Destroyer", 0);
        shipHitsCounter.Add("Battleship", 0);
        shipHitsCounter.Add("Carrier", 0);
    }

    public void MakeDecision()
    {
        //Randomly picking a tile
        if (randomPick)
        {
            int posPick = UnityEngine.Random.Range(0, allTiles.Count);
            string tilePicked = allTiles.ElementAt(posPick).Key;
            CheckForHit(tilePicked);
            allTiles.Remove(tilePicked);                    //Removing so cant pick same tile twice
            neighbourTiles = new List<string>();
        }
        else
        {
            int posPick = UnityEngine.Random.Range(0, neighbourTiles.Count);
            CheckForHit(neighbourTiles[posPick]);
            allTiles.Remove(neighbourTiles[posPick]);           //Removing so cant pick same tile twice
            neighbourTiles.RemoveAt(posPick);           //Removing so cant pick same tile twice
        }
        //Creates neighbours once hit has occured
        if (hasHit)
        {
            char letterUp = '0';
            char letterDown = '0';
            char numberLeft = '0';
            char numberRight = '0';

            char[] splitUp = lastHit.ToCharArray();

            //Letter manipulation
            if ((splitUp[0] == 'A'))
            {
                char x = splitUp[0];
                letterUp = Convert.ToChar((Convert.ToUInt16(x) + 1));
            }
            else if ((splitUp[0] == 'J'))
            {
                char x = splitUp[0];
                letterDown = Convert.ToChar((Convert.ToUInt16(x) - 1));
            }
            else
            {
                //Finds the neighbouring letters
                char x = splitUp[0];
                letterUp = Convert.ToChar((Convert.ToUInt16(x) + 1));
                letterDown = Convert.ToChar((Convert.ToUInt16(x) - 1));
            }


            //Number manipulation
            if ((splitUp[1] == '1') && splitUp.Length == 2)
            {
                int a = Convert.ToUInt16(splitUp[1]);
                numberRight = Convert.ToChar(a + 1);
            }
            //Checks if it is 10 as 10 is representaed as 1 and 0 in a char array
            else if ((splitUp[1] == '1') && splitUp.Length == 3)
            {
                numberLeft = Convert.ToChar(57);        //Ascii value for 9
            }
            else
            {
                //Finds the neighbouring numbers
                int a = Convert.ToUInt16(splitUp[1]);
                numberRight = Convert.ToChar(a + 1);
                numberLeft = Convert.ToChar(a - 1);
            }

            //Finding neighbours
            if (letterDown == '0')
            {
                string tile1 = letterUp.ToString() + splitUp[1].ToString();
                string tile2 = splitUp[0].ToString() + numberLeft.ToString();
                string tile3 = splitUp[0].ToString() + numberRight.ToString();

                if (allTiles.ContainsKey(tile1))
                {
                    neighbourTiles.Add(tile1);
                }
                if (allTiles.ContainsKey(tile2))
                {
                    neighbourTiles.Add(tile2);
                }
                if (allTiles.ContainsKey(tile3))
                {
                    neighbourTiles.Add(tile3);
                }
            }
            else if (letterUp == '0')
            {
                string tile1 = letterDown.ToString() + splitUp[1].ToString();
                string tile2 = splitUp[0].ToString() + numberLeft.ToString();
                string tile3 = splitUp[0].ToString() + numberRight.ToString();

                if (allTiles.ContainsKey(tile1))
                {
                    neighbourTiles.Add(tile1);
                }
                if (allTiles.ContainsKey(tile2))
                {
                    neighbourTiles.Add(tile2);
                }
                if (allTiles.ContainsKey(tile3))
                {
                    neighbourTiles.Add(tile3);
                }
            }
            else if (numberLeft == '0')
            {
                string tile1 = splitUp[0].ToString() + numberRight.ToString();
                string tile2 = letterDown.ToString() + splitUp[1].ToString();
                string tile3 = letterUp.ToString() + splitUp[1].ToString();

                if (allTiles.ContainsKey(tile1))
                {
                    neighbourTiles.Add(tile1);
                }
                if (allTiles.ContainsKey(tile2))
                {
                    neighbourTiles.Add(tile2);
                }
                if (allTiles.ContainsKey(tile3))
                {
                    neighbourTiles.Add(tile3);
                }
            }
            else if (numberRight == '0')
            {
                string tile1 = splitUp[0].ToString() + numberLeft.ToString();
                string tile2 = letterDown.ToString() + splitUp[1].ToString();
                string tile3 = letterUp.ToString() + splitUp[1].ToString();

                if (allTiles.ContainsKey(tile1))
                {
                    neighbourTiles.Add(tile1);
                }
                if (allTiles.ContainsKey(tile2))
                {
                    neighbourTiles.Add(tile2);
                }
                if (allTiles.ContainsKey(tile3))
                {
                    neighbourTiles.Add(tile3);
                }
            }
            else
            {
                string tile1 = letterUp.ToString() + splitUp[1].ToString();
                string tile2 = letterDown.ToString() + splitUp[1].ToString();
                string tile3 = splitUp[0].ToString() + numberLeft.ToString();
                string tile4 = splitUp[0].ToString() + numberRight.ToString();

                if (allTiles.ContainsKey(tile1))
                {
                    neighbourTiles.Add(tile1);
                }
                if (allTiles.ContainsKey(tile2))
                {
                    neighbourTiles.Add(tile2);
                }
                if (allTiles.ContainsKey(tile3))
                {
                    neighbourTiles.Add(tile3);
                }
                if (allTiles.ContainsKey(tile4))
                {
                    neighbourTiles.Add(tile4);
                }
            }
            managerScript.AttackMode(false);
            CheckForWin();
        }
        else
        {
            managerScript.AttackMode(true);
            CheckForWin();
        }
    }
    

    //Checks if guess has hit player ship
    public void CheckForHit(string AIGuess)
    {
        hasHit = false;

        allTiles.TryGetValue(AIGuess, out guessPos);
        //Looping all ship pos to see if the guess was a hit
        foreach (var ship in playerShipPos)
        {
            foreach (var shipPos in ship.Value)
            {
                if (guessPos == shipPos)
                {
                    Debug.Log("Hit" + ship.Key + guessPos);
                    Instantiate(hit, guessPos + new Vector3(5, 10, 5), Quaternion.identity);                //155 so it can be displayed on the correct map
                    Instantiate(missile, guessPos + new Vector3(5, 100, 5), Quaternion.identity);           //Spawning missle for visual effects
                    hasHit = true;
                    randomPick = false;
                    lastHit = AIGuess;
                    //Incrementing the hit counter
                    shipHitsCounter.TryGetValue(ship.Key, out int val);
                    shipHitsCounter[ship.Key] = val + 1;
                    //Checking if ship has sunk
                    if (val + 1 == ship.Value.Count)
                    {
                        Debug.Log("Ship sunk: " + ship.Key);
                        numShipsSunk += 1;
                        randomPick = true;
                        managerScript.recentSink = true;
                        managerScript.sunkShipName = ship.Key;
                    }
                }
            }
        }

        if (!hasHit)
        {
            Debug.Log("Miss");
            Instantiate(miss, guessPos + new Vector3(5, 15, 5), Quaternion.identity);
            Instantiate(missile, guessPos + new Vector3(5, 100, 5), Quaternion.identity);
        }
    }

    //Checks if AI has won game
    public void CheckForWin()
    {
        if(numShipsSunk == 5)
        {
            managerScript.gameOverName = "AI";
            managerScript.gameOver = true;
        }
    }
}
