using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class AstroidScript : MonoBehaviour
{

    private CameraShaker cameraShakerScripts;

    // Start is called before the first frame update
    void Start()
    {
        cameraShakerScripts = Camera.main.GetComponent<CameraShaker>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == Consts.GROUND)
        {    
            cameraShakerScripts.AstroidCameraShake();          
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}