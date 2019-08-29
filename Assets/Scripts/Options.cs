using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class Options : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene(Consts.MENU);
    }
}
