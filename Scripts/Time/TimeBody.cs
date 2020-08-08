using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    [Header("Components")]
    public Ghost ghost;
    public HealthBar healthBar;
    public GameObject returnSprite;
    public GameObject phaseHitBox;
    public GameObject finalHitBox;
    public GameObject explosionHitBox;
    public PlayerMovement playerMovement;

    [Header("Timers")]
    public float cooldownConstant;
    public float animationTime;
    public bool isOnCooldown;
    private float cooldown;

    [Header("Rewind")]
    public float rewindTime;
    public bool isRewinding;

    [Header("Exploded")]
    public bool Exploded;

    [Header("Trail")]
    public Phase phase;

    [Header("Record")]
    List<PointInTime> pointsInTime;
    public int returnPosition;
    private int frames = 0;

    void Start()
    {
        isOnCooldown = false;
        phase.makePhase = true;
        cooldown = cooldownConstant;
        phaseHitBox.SetActive(false);
        finalHitBox.SetActive(false);
        pointsInTime = new List<PointInTime>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerMovement.canMove == true && cooldown == cooldownConstant)
        {
            StartRewind();
            PauseTrail();
        }

        if (cooldown < cooldownConstant && cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            
            if (playerMovement.animator.GetBool("rewinding"))
            {
                playerMovement.canMove = false;
                if (!Exploded)
                {
                    StartCoroutine(Explosion());
                    Exploded = true;
                }
            }
            else if (!playerMovement.animator.GetBool("rewinding"))
            {
                Exploded = false;
                playerMovement.canMove = true;
                finalHitBox.SetActive(false);
            }

            if (cooldown >= 5)
            {
                DestroyClones();
            }
        }
        else if (cooldown <= 0)
        {
            isOnCooldown = false;
            cooldown = cooldownConstant;
        }
    }

    private void FixedUpdate()
    {
        frames++;
        if (isRewinding)
            Rewind();
        else if (frames % 5 == 0)
            Record();
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            playerMovement.currentHealth = pointInTime.health;

            GameObject currentReturn = Instantiate(returnSprite, pointInTime.position, transform.rotation);
            Destroy(currentReturn, 0.25f);
            pointsInTime.RemoveAt(0);

            healthBar.SetHealth(playerMovement.currentHealth);
            playerMovement.isInvulnerable = true;
            phaseHitBox.SetActive(true);
        }
        else
        {
            StopRewind();
        }
    }

    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(rewindTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
        pointsInTime.Insert(0, new PointInTime(transform.position, playerMovement.currentHealth));
    }

    public void StartRewind()
    {
        isRewinding = true;
        isOnCooldown = true;
        cooldown -= Time.deltaTime;
        playerMovement.canMove = false;
        returnPosition = pointsInTime.Count - 1;
        FindObjectOfType<AudioManager>().Play("RewindStart");
    }

    public void StopRewind()
    {
        isRewinding = false;
        finalHitBox.SetActive(true);
        phaseHitBox.SetActive(false);
        playerMovement.canMove = true;
        playerMovement.isInvulnerable = false;
        playerMovement.animator.SetBool("rewinding", true);
        FindObjectOfType<AudioManager>().Play("RewindEnd");
    }
    
    public void DestroyClones()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag("clone");
        foreach (GameObject clone in clones)
            Destroy(clone, 1);
    }

    public void PauseTrail()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag("clone");
        foreach (GameObject clone in clones)
            clone.GetComponent<pauseClones>().animator.speed = 0;
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.3f);
        explosionHitBox.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        explosionHitBox.SetActive(false);
    }
}
