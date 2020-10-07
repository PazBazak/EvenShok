using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Random = System.Random;


public class AstroidScript : MonoBehaviour
{

    private CameraShaker cameraShakerScripts;
    private float startXPosition;
    private bool isFacingLeft;
    private GameObject[] locationsToSpawnAstroids;
    private int forceDirection;

    // Start is called before the first frame update
    void Start()
    {
        cameraShakerScripts = Camera.main.GetComponent<CameraShaker>();
        startXPosition = transform.position.x;

        forceDirection = startXPosition > 0 ? -250 : 250;

        Random rnd = new Random();

        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceDirection, -250));
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == Consts.GROUND)
        {    
            cameraShakerScripts.AstroidCameraShake();          
            Destroy(gameObject);
        }
    }

}
