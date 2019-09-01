using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

/// <summary>
/// Managing death menu
/// </summary>
public class DeathMenu : MonoBehaviour
{
    #region Data Members

    public Image BackGround;
    private bool isShown = false;
    private float trans = 0.0f;
    public GameObject YouLost;
    public GameObject ScoreObj;
    public GameObject TimeObj;
    public Button PlayAgainBtn;
    public Button MenuBtn;
    #endregion

    #region Functions
    
    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
        YouLost.SetActive(false);
        ScoreObj.SetActive(false);
        TimeObj.SetActive(true);
        PlayAgainBtn.gameObject.SetActive(false);
        MenuBtn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShown)
        {
            trans += Time.deltaTime;
            BackGround.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.7f), trans);
        }
    }

    public void ToggleEndMenu()
    {
        gameObject.SetActive(true);
        TimeObj.SetActive(false);
        isShown = true;
        YouLost.SetActive(true);
        ScoreObj.SetActive(true);
        PlayAgainBtn.gameObject.SetActive(true);
        MenuBtn.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(Consts.MENU);
    }

    #endregion
}