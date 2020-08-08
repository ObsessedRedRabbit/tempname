using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Rigidbody2D rb;
    public TimeBody time;

    [Header("Health")]
    public HealthBar healthBar;
    public int maxHealth;
    public int currentHealth;
    public bool isInvulnerable;

    [Header("Movements")]
    public float sneakSpeed;
    public float walkSpeed;
    public float runSpeed;
    public bool isMoving;
    public bool canMove;
    private float moveSpeed;

    [Header("Phasing")]
    public Ghost ghost;
    public bool isPhasing;
    public float phaseTime;
    public float phaseSpeed;
    public float phaseCooldown;
    public float constantPhaseTime;
    public float phaseCooldownConstant;


    public void Start()
    {
        currentHealth = maxHealth;
        phaseTime = constantPhaseTime;
        phaseCooldown = phaseCooldownConstant;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void Update()
    {
        if (canMove)
        {
            Movement();
        }

        if (isMoving && Input.GetKey("space") && phaseTime == constantPhaseTime && phaseCooldown == phaseCooldownConstant && !time.isRewinding)
        {
            StartPhasing();
        }
        else if (phaseTime < constantPhaseTime && phaseTime > 0)
        {
            Phasing();
        }
        else if (phaseTime <= 0)
        {
            FinishPhasing();
        }

        if (phaseCooldown < phaseCooldownConstant && phaseCooldown > 0)
        {
            phaseCooldown -= Time.deltaTime;
        }
        else if (phaseCooldown <= 0)
        {
            phaseCooldown = phaseCooldownConstant;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable == false)
        {
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);
        }
    }

    void Phasing()
    {
        Vector3 phaseMovement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        transform.position = Vector2.MoveTowards(transform.position, transform.position + phaseMovement, Time.deltaTime * moveSpeed);
        phaseTime -= Time.deltaTime;
        ghost.makeGhost = true;
        isInvulnerable = true;
        isPhasing = true;
    }

    void StartPhasing()
    {
        phaseTime -= Time.deltaTime;
        moveSpeed = phaseSpeed;
        canMove = false;
    }

    void FinishPhasing()
    {
        canMove = true;
        isPhasing = false;
        isInvulnerable = false;
        phaseTime = constantPhaseTime;
        phaseCooldown -= Time.deltaTime;
        ghost.makeGhost = false;
        moveSpeed = 3f;
    }

    //void phase()
    //{
    //    Vector3 phaseMovement = new Vector3(animator.GetFloat("lastMoveX"), animator.GetFloat("lastMoveY") , 0.0f);
    //    transform.position = Vector2.MoveTowards(transform.position, transform.position + phaseMovement, Time.deltaTime * moveSpeed);
    //}

    public void Movement()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        transform.position = Vector2.MoveTowards(transform.position, transform.position + movement, Time.deltaTime * moveSpeed);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        if (Input.GetAxis("Horizontal") > 0.01 || Input.GetAxis("Horizontal") < -0.01 || Input.GetAxis("Vertical") > 0.01 || Input.GetAxis("Vertical") < -0.01)
        {
            animator.SetFloat("lastMoveX", Input.GetAxis("Horizontal"));
            animator.SetFloat("lastMoveY", Input.GetAxis("Vertical"));
        }

        if (Input.GetKey("left shift"))
        {
            moveSpeed = runSpeed;
        }
        else if (Input.GetKey("left ctrl"))
        {
            moveSpeed = sneakSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            //Debug.Log(movement);
        }
    }
}
