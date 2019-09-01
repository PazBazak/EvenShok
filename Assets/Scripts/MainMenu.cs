using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;

/// <summary>
/// Managing main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    #region Data Members

    public Text HighScore;

    #endregion

    #region Functions

    private void Start()
    {
        HighScore.text = Consts.HIGH_SCORE + (int)PlayerPrefs.GetFloat(Consts.HIGH_SCORE);
    }

    public void ToPlay()
    {
        SceneManager.LoadScene(Consts.PLAY_SCENE);
    }

    public void ToOptions()
    {
        SceneManager.LoadScene(Consts.OPTIONS);
    }

    #endregion
}