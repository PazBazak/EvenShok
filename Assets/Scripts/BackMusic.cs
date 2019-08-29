using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

/// <summary>
/// Background music
/// </summary>
public class BackMusic : MonoBehaviour
{
    #region Functions

    private void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag(Consts.SOUND);

        if (obj.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion
}