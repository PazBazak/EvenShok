using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class AstroidScript : MonoBehaviour
{

    private CameraShaker cameraShakerScripts;
    private int astroidDirection;
    private float startXPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraShakerScripts = Camera.main.GetComponent<CameraShaker>();
        startXPosition = transform.position.x;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == Consts.GROUND)
        {    
            cameraShakerScripts.AstroidCameraShake();          
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.x <= -30)
        {
            astroidDirection = 10;
        }
        else
        {
            astroidDirection = -10;
        }
        Vector2 direction = new Vector2 (startXPosition + astroidDirection, 0);
        transform.Translate(direction * Time.deltaTime);   //travel to last position
    }
}
