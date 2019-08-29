using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

/// <summary>
/// class contains a function that destroys enemies when touching the ground
/// </summary>
public class DestroyOnGround : MonoBehaviour
{
    #region Functions

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == Consts.GROUND)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}