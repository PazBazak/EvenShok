using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeImgTransition : MonoBehaviour
{

    private bool isShown = false;
    private float trans = 0.0f;

    public Image BackGround;

    private void Start()
    {
        StartCoroutine(ImageTransitionBetweenModes());
    }
   

    // Update is called once per frame
    void Update()
    {
        if (isShown)
        {
            trans += Time.deltaTime;
            BackGround.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(255, 255, 255, 5), trans);
        }
    }


    IEnumerator ImageTransitionBetweenModes()
    {
        yield return new WaitForSeconds(2f);
        isShown = true;

        yield return new WaitForSeconds(0.5f);
        isShown = false;
        trans = 0.0f;

        StartCoroutine(ImageTransitionBetweenModes());
    }
}
