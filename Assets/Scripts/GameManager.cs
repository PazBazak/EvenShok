using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
    #region Data Members

    private bool _isDead;
    private int _lives;
    private int _difficulty;
    private int _mode; // 0 = NormalGame, 10 = HellGame, 1 = TextGame, 2 = NumberGame, 3 = ColorGame, 4 = HandSignGame **
    private float _timePassed;
    private float _score;
    private GameObject[] _heartPictures;
    private static GameManager _instance;
    private Stopwatch _gameStopwatch;

    #endregion

    #region Properties

    public bool IsDead
    {
        get
        {
            return _isDead;
        }
        set
        {
            _isDead = value;
        }
    }

    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
        }
    }

    public int Difficulty
    {
        get
        {
            return _difficulty;
        }
        set
        {
            _difficulty = value;
        }
    }

    public int Mode
    {
        get
        {
            return _mode;
        }
        set
        {
            _mode = value;
        }
    }

    public float Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    public float TimePassed
    {
        get
        {
            return _timePassed;
        }
        set
        {
            _timePassed = value;
        }
    }

    public GameObject[] HeartPictures
    {
        get
        {
            return _heartPictures;
        }
        set
        {
            _heartPictures = value;
        }
    }

    public Stopwatch GameStopwatch
    {
        get
        {
            return _gameStopwatch;
        }
        set
        {
            _gameStopwatch = value;
        }
    }

    #endregion

    #region Constructor

    public GameManager()
    {
        Lives = 3;
        Score = 0;
        TimePassed = 0;
        HeartPictures = GameObject.FindGameObjectsWithTag("Heart");
        GameStopwatch = new Stopwatch();
    }

    #endregion

    #region Singleton

    public static GameManager Instance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }

    #endregion

    #region Methods

    public void Restart()
    {
        IsDead = false;
        Lives = 3;
        Score = 0;
        TimePassed = 0;
        HeartPictures = GameObject.FindGameObjectsWithTag("Heart");

        foreach (GameObject heart in HeartPictures)
        {
            heart.GetComponent<Image>().color =Color.green;
        }
    }

    #endregion
}
