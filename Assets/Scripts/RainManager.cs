using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

/// <summary>
/// Managing rain of enemies
/// </summary>
public class RainManager : MonoBehaviour
{
    #region Data Members

    // Array of empty objects to indicate where the rain can possible come from
    private GameObject[] locationsToSpawn;

    // [0]=Paper// [1]=Rock// [2]=Scissors
    [SerializeField] GameObject[] objectToSpawn;

    // Time between another object to spawn 
    float timeBetweenSpawns = 0.04f;

    private bool isDeadHere;
    private float Score;
    private float Timer = 0;
    private int LastPrefabIndex = 0;

    [SerializeField] private Transform CanvasRef;
    [SerializeField] private Sprite NumberRock;// 1
    [SerializeField] private Sprite NumberPaper;// 4
    [SerializeField] private Sprite NumberScissors;// 7
    [SerializeField] private Sprite Rock;// 1
    [SerializeField] private Sprite Paper;// 4
    [SerializeField] private Sprite Scissors;// 7

    #endregion

    #region Functions

    void Start()
    {
        // Get a refrebce of all the empty game objects tagged with SpawnLocation
        locationsToSpawn = GameObject.FindGameObjectsWithTag(Consts.SPAWN_LOCATION);
    }

    void Update()
    {
        // The Score here is the same score as in player's script
        Score = GameObject.Find(Consts.PLAYER).GetComponent<Player>().score;

        // isDeadHere in this script is the same as Player's Script isDead
        isDeadHere = GameObject.Find(Consts.PLAYER).GetComponent<Player>().isDead;

        // Counting seconds by adding the time it takes to finish frame each frame so it adds up to 1 second each real time second
        Timer += Time.deltaTime;

        // If the timer is bigger than the time between spawns
        if (Timer > timeBetweenSpawns && !isDeadHere)
        {
            GameObject spawnedObject;

            // The spawned object is a copy of random object from ObjectsToSpawn var with the starting location of random empty game object from LocationsToSpawn with rotation to ground
            spawnedObject = Instantiate(objectToSpawn[RandomPrefab()], locationsToSpawn[Random.Range(0, locationsToSpawn.Length)].transform.position, Quaternion.identity) as GameObject;

            // Resets the timer
            Timer = 0;

            // Making the clone child of object to be more orginized
            spawnedObject.transform.SetParent(CanvasRef);

            spawnedObject.AddComponent<CDest>();

            // Game stages
            if (Time.time > 2)
            {
                timeBetweenSpawns = 0.14f;
                TextGame(spawnedObject);
            }

            if (Time.time > 10)
            {
                timeBetweenSpawns = 0.14f;
                NumberGame(spawnedObject);
            }

            if (Time.time > 15)
            {
                timeBetweenSpawns = 0.1f;
                NormalGame(spawnedObject);
            }

            if (Time.time > 20)
            {
                timeBetweenSpawns = 0.1f;
                NumberGame(spawnedObject);
            }
        }
    }

    // Making my own random so it wont spawn 1-1 never
    private int RandomPrefab()
    {
        if (objectToSpawn.Length <= 1)
        {
            return 0;
        }

        // The random index equals the last prefab index
        int randomIndex = LastPrefabIndex;

        // As long as it equals then random index gets value between 0and3 untill they dont equals
        while (randomIndex == LastPrefabIndex)
        {
            randomIndex = Random.Range(0, objectToSpawn.Length);
        }

        // The last prefab index changing to the random one
        LastPrefabIndex = randomIndex;

        // Return the random value
        return randomIndex;
    }

    private void NormalGame(GameObject obj)//////////////////////////////
    {
        obj.GetComponent<SpriteRenderer>().flipY = true;

        if (obj.gameObject.tag == Consts.ROCK)
        {
            obj.GetComponent<SpriteRenderer>().sprite = Rock;
        }

        if (obj.gameObject.tag == Consts.PAPER)
        {
            obj.GetComponent<SpriteRenderer>().sprite = Paper;
        }

        if (obj.gameObject.tag == Consts.SCISSORS)
        {
            obj.GetComponent<SpriteRenderer>().sprite = Scissors;
        }
    }

    private void TextGame(GameObject obj)/////////////////////////////////////////////2
    {
        obj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }

    private void ColorGame(GameObject obj)//////////////////////////////////////////3
    {
        obj.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void NumberGame(GameObject obj)//////////////////////////////////////4
    {
        obj.GetComponent<SpriteRenderer>().flipY = false;

        if (obj.gameObject.tag == Consts.ROCK)
        {
            obj.GetComponent<SpriteRenderer>().sprite = NumberRock;
        }

        if (obj.gameObject.tag == Consts.PAPER)
        {
            obj.GetComponent<SpriteRenderer>().sprite = NumberPaper;
        }

        if (obj.gameObject.tag == Consts.SCISSORS)
        {
            obj.GetComponent<SpriteRenderer>().sprite = NumberScissors;
        }
    }

    #endregion
}