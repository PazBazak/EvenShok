using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    // delay between ghost instantiation
    public float ghostDelay;

    // ghost instantiation timer
    private float ghostDelaySeconds;

    // actual ghost prefab to be displayed
    public GameObject ghostPrefab;

    // parent ghosts objects
    public Transform GhostsParent;

    // defining if it should create ghosts
    public bool makeGhost = false;

    private float animationTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // starting the timer
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else // genereate ghost
            {
                GameObject currentGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);

                // get the sprite of the current player
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;

                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                currentGhost.transform.SetParent(GhostsParent);

                // setting the rotation of the ghost same as the player ( *-1 seems like a temporary solution, until further issues of re-doing our images)
                currentGhost.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y * -1, transform.localScale.y);

                // reseting the timer
                ghostDelaySeconds = ghostDelay;

                // Destroying the ghost after set time
                Destroy(currentGhost, animationTime);
            }
        }
    }
}
