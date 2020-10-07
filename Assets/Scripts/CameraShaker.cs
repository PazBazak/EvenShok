using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class CameraShaker : MonoBehaviour
{
    #region variables

    private Camera myCamera;
   
    #endregion
    private void Awake()
    {
        myCamera = GetComponent<Camera>();
    }
   
    public IEnumerator Shake (float shakeDuration, float xPower, float yPower)
    {
        Vector3 originalCameraPosition = myCamera.transform.localPosition;  //to save the original camera position

        float timeCounter = 0.0f; 

        while (timeCounter < shakeDuration) 
        {
            float x = Random.Range(-xPower, xPower);
            float y = Random.Range(-yPower, yPower);

            myCamera.transform.localPosition = new Vector3(x, y, originalCameraPosition.z);  //the new camera position is getting random x and y inputs

            timeCounter += Time.deltaTime;

            yield return null; //will continue with the while loop each frame
        }

        myCamera.transform.localPosition = originalCameraPosition;
    }


    public void EasyCameraShake()
    {
        StartCoroutine(Shake(Consts.durationEasyShake, Consts.xPowerEasyShake, Consts.yPowerEasyShake));
    }


    public void MediumCameraShake()
    {
        StartCoroutine(Shake(Consts.durationMediumShake, Consts.xPowerMediumShake, Consts.yPowerMediumShake));
    }


    public void HardCameraShake()
    {
        StartCoroutine(Shake(Consts.durationHardShake, Consts.xPowerHardShake, Consts.yPowerHardShake));
    }

    public void AstroidCameraShake()
    {
        StartCoroutine(Shake(Consts.durationAstroidShake, Consts.xPowerAstroidShake, Consts.yPowerAstroidShake));
    }


    public void HellCameraShake()
    {
        StartCoroutine(Shake(Consts.durationHellShake, Consts.xPowerHellShake, Consts.yPowerHellShake));
    }
    
    public void CollisionCameraShake()
    {
        StartCoroutine(Shake(Consts.durationCollisionShake, Consts.xPowerCollisionShake, Consts.yPowerCollisionShake));
    }
}
