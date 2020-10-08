using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{

    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghostPrefab;
    public Transform GhostsParent;

    public bool makeGhost = false;

    // Start is called before the first frame update
    void Start()
    {
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
                currentGhost.transform.SetParent(GhostsParent);
                ghostDelaySeconds = ghostDelay;
            }
        }
    }
}
