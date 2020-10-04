using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Linq;
using System;
using UnityRandom = UnityEngine.Random;
using Random = System.Random;

/// <summary>
/// Managing rain of enemies
/// </summary>
public class RainManager : MonoBehaviour
{
    #region Data Members

    // Array of empty objects to indicate where the rain can possible come from
    private GameObject[] locationsToSpawn;

    public GameObject[] objectToSpawn;

    public CameraShaker cameraShakerScripts;

    // Time between another object to spawn 
    private float timeBetweenSpawns = 0.16f;
    public Text TimeBetweenSpawnsTxt;
    private float timeBetweenModes = 10f;

    private bool isDeadHere;

    private int currentDifficulity;

    private float Score;
    private float Timer = 0;
    private int modesCount = 3;  //Number of modes excluding normal game and hell
    public int activeMode = 0;      // 0 = NormalGame, 10 = HellGame, 1 = TextGame, 2 = NumberGame, 3 = ColorGame, 4 = HandSignGame ** 

    private int lastPrefabIndex = 0;
    private int lastSpawnedIndex = 0;
    private int lastDiffIndex = -1;

    public Transform CanvasRef;
    public Sprite NumberRock;// 1
    public Sprite NumberPaper;// 4
    public Sprite NumberScissors;// 7

    public Sprite Rock;
    public Sprite Paper;
    public Sprite Scissors;

    public Sprite TextRock;
    public Sprite TextPaper;
    public Sprite TextScissors;

    public Sprite ColorRock;// black
    public Sprite ColorPaper;// pink
    public Sprite ColorScissors;// orange

    #endregion

    #region Functions

    void Start()
    {
        // Get a refrebce of all the empty game objects tagged with SpawnLocation
        locationsToSpawn = GameObject.FindGameObjectsWithTag(Consts.SPAWN_LOCATION);

        StartCoroutine(HandleStage(GetRandomStageModes(modesCount)));
        StartCoroutine(HandleDifficulities(GetRandomDifficulities()));
        StartCoroutine(HandleRandomShakes(4,10));
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
            spawnedObject = Instantiate(objectToSpawn[MyRandom(objectToSpawn.Length, ref lastPrefabIndex)], locationsToSpawn[MyRandom(locationsToSpawn.Length, ref lastSpawnedIndex)].transform.position, Quaternion.identity) as GameObject;

            // Resets the timer
            Timer = 0;

            // Making the clone child of object to be more orginized
            spawnedObject.transform.SetParent(CanvasRef);

            spawnedObject.AddComponent<DestroyOnGround>();

            HandleMode(activeMode, spawnedObject);

        }
    }


    IEnumerator HandleDifficulities(int[] stageDifficulities)
    {
        foreach (int i in stageDifficulities)
        {
            switch (i)
            {
                case 0:
                    timeBetweenSpawns = Consts.easyDifficulity;
                    TimeBetweenSpawnsTxt.text = Consts.EASY_DIFF;
                    currentDifficulity = 0;
                    break;

                case 1:
                    timeBetweenSpawns = Consts.mediumDifficulity;
                    TimeBetweenSpawnsTxt.text = Consts.MEDIUM_DIFF;
                    currentDifficulity = 1;
                    break;

                case 2:
                    timeBetweenSpawns = Consts.hardDifficulity;
                    TimeBetweenSpawnsTxt.text = Consts.HARD_DIFF;    
                    currentDifficulity = 2;                
                    break;

                case 3:
                    timeBetweenSpawns = Consts.hellDifficulity;
                    TimeBetweenSpawnsTxt.text = Consts.HELL_DIFF;
                    currentDifficulity = 3;
                    cameraShakerScripts.HellCameraShake();
                    break;
            }

            yield return new WaitForSeconds(timeBetweenModes);
        }
        StartCoroutine(HandleDifficulities(GetRandomDifficulities()));
    }


    IEnumerator HandleStage(int[] CurrentStage)
    {
        foreach (int i in CurrentStage)
        {
            activeMode = i;
            yield return new WaitForSeconds(timeBetweenModes);
        }
        StartCoroutine(HandleStage(GetRandomStageModes(modesCount)));
    }

 IEnumerator HandleRandomShakes(int minTime, int maxTime)
    {
        Random rnd = new Random();

        while (!isDeadHere)
        {
            yield return new WaitForSeconds(rnd.Next(minTime, maxTime)); 
            switch (currentDifficulity)
            {
                case 0:
                    cameraShakerScripts.EasyCameraShake();
                    break;

                case 1:
                    cameraShakerScripts.MediumCameraShake();
                    break;

                case 2:
                    cameraShakerScripts.HardCameraShake();                    
                    break;
            }               
        }
    }


    private int[] GetRandomStageModes(int modesCount)
    {
        Random rnd = new Random();

        int[] zeroArr = { 0 };
        int[] hellArr = { 10 };
        int[] stageArr = Enumerable.Range(1, modesCount).OrderBy(c => rnd.Next()).ToArray();
        int[] RandomizedStage = (zeroArr.Concat(stageArr).ToArray()).Concat(hellArr).ToArray();

        return RandomizedStage;
    }


    private int[] GetRandomDifficulities()
    {
        Random rnd = new Random();

        int[] diffArray = new int[modesCount + 2];
        for (int i = 0; i < diffArray.Length; i++) // TODO: try to change this for to a more sleek one
        {
            if (i == modesCount + 1)
            {
                // make hellMode always on Hell difficulity
                diffArray[i] = 3;
                break;
            }
            //0 = Easy, 1 = Medium, 2 = Hard, 3 = HELL
            diffArray[i] = MyRandom(3, ref lastDiffIndex);
        }
        return diffArray;
    }


    private string showArray(int[] myArr)
    {
        return string.Join(",", myArr);
    }


    private void HandleMode(int currentMode, GameObject obj)
    {
        SwitchCaseBetweenModes(currentMode, obj);
    }


    private void SwitchCaseBetweenModes(int Case, GameObject obj)
    {
        switch (Case)
        {
            case 0:
                NormalGame(obj);
                break;

            case 1:
                TextGame(obj);
                break;

            case 2:
                NumberGame(obj);
                break;

            case 3:
                ColorGame(obj);
                break;

            case 10:
                HellGame(obj);
                break;
        }
    }


    private int MyRandom(int itemsLength, ref int lastIndex)
    {
        if (itemsLength <= 1)
        {
            return 0;
        }

        int randomIndex = lastIndex;

        while (randomIndex == lastIndex)
        {
            randomIndex = UnityRandom.Range(0, itemsLength);
        }

        lastIndex = randomIndex;

        return randomIndex;
    }


    private void NormalGame(GameObject obj)
    {
        SpawnedObjectImageSelectorByMode(obj, Rock, Paper, Scissors);
    }

    private void TextGame(GameObject obj)
    {
        SpawnedObjectImageSelectorByMode(obj, TextRock, TextPaper, TextScissors);
    }


    private void NumberGame(GameObject obj)
    {
        SpawnedObjectImageSelectorByMode(obj, NumberRock, NumberPaper, NumberScissors);
    }


    private void ColorGame(GameObject obj)
    {
        SpawnedObjectImageSelectorByMode(obj, ColorRock, ColorPaper, ColorScissors);
    }


    private void SpawnedObjectImageSelectorByMode(GameObject obj, Sprite Rock, Sprite Paper, Sprite Scissors)
    {
        switch (obj.gameObject.tag)
        {
            case Consts.ROCK:
                obj.GetComponent<SpriteRenderer>().sprite = Rock;
                break;

            case Consts.PAPER:
                obj.GetComponent<SpriteRenderer>().sprite = Paper;
                break;

            case Consts.SCISSORS:
                obj.GetComponent<SpriteRenderer>().sprite = Scissors;
                break;
        }
    }


    private void HellGame(GameObject obj)
    {
        Random rnd = new Random();

        int RandomMode = rnd.Next(0, modesCount + 1);

        SwitchCaseBetweenModes(RandomMode, obj);
    }


    #endregion
}