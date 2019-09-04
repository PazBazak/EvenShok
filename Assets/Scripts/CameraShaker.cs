using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    #region variables

    private Camera myCamera;

    public float Xpower;
    public float Ypower;

    #endregion
    private void Start()
    {
        myCamera = GetComponent<Camera>();
    }
    /// <summary>
    /// Coroutine of shake function
    /// </summary>
    /// <param name="shakeDuration"></param> The shake animation duration
    /// <param name="shakePower"></param> The shake animatuib power
    /// <returns></returns>
    public IEnumerator Shake (float shakeDuration)
    {
        Vector3 originalCameraPosition = myCamera.transform.localPosition;  //to save the original camera position

        float timeCounter = 0.0f; 

        while (timeCounter < shakeDuration) 
        {
            float x = Random.Range(-Xpower, Xpower);
            float y = Random.Range(-Ypower, Ypower);

            myCamera.transform.localPosition = new Vector3(x, y, originalCameraPosition.z);  //the new camera position is getting random x and y inputs

            timeCounter += Time.deltaTime;

            yield return null; //will continue with the while loop each frame
        }

        myCamera.transform.localPosition = originalCameraPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shake(0.15f));
        }
    }

}
