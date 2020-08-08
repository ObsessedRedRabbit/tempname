using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : MonoBehaviour
{
    [Header("Components")]
    public TimeBody time;
    public GameObject phase;

    [Header("PhaseStats")]
    public float phaseDelay;
    public float timeToDestroy;
    private float phaseDelaySeconds;
    public bool makePhase = false;

    void Start()
    {
        phaseDelaySeconds = phaseDelay;
    }

    void Update()
    {
        get_input();
    }

    void get_input()
    {
        if (makePhase)
        {
            if (phaseDelaySeconds > 0)
            {
                phaseDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentPhase = Instantiate(phase, transform.position, transform.rotation);
                currentPhase.tag = "clone";
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentPhase.GetComponent<SpriteRenderer>().sprite = currentSprite;
                phaseDelaySeconds = phaseDelay;
                Destroy(currentPhase, timeToDestroy);
                if (time.isRewinding)
                {
                    Destroy(currentPhase);
                }
            }
        }
    }
}
