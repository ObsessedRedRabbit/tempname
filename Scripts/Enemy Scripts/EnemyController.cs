using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    private float damaged;
    private bool attacked;
    private int currentHealth;

    [Header("Components")]
    public Spawner spawner;
    public Animator animator;
    public HealthBar healthBar;
    public Transform spawnPosition;
    public GameObject FloatingTextPrefab;
    private Transform target;
    private TimeBody rewindTime;
    private PlayerMovement player;

    [Header("Stats")]
    public int damage;
    public int maxHealth;
    
    public float speed;
    public float minRange;
    public float maxRange;

    [Header("Timers")]
    public float attackTimerConstant;
    public float returnTimerConstant;

    private float attackTimer;
    private float returnTimer;

    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        rewindTime = FindObjectOfType<TimeBody>();
        player = FindObjectOfType<PlayerMovement>();
        target = FindObjectOfType<PlayerMovement>().transform;

        healthBar.SetMaxHealth(maxHealth);
        attackTimer = attackTimerConstant;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!rewindTime.isRewinding)
        {
            MoveToPlayer();
            AttackPlayer();
            AttackTimer();
        }
    }

    public void FollowPlayer()
    {
        animator.SetBool("isMoving", true);
        animator.SetFloat("Horizontal", (target.position.x - transform.position.x));
        animator.SetFloat("Vertical", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void ReturnToSpawn()
    {
        transform.position = Vector3.MoveTowards(transform.position, spawnPosition.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        damaged = damage;
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        ShowFloatingText();

        if (currentHealth <= 0)
        {
            Destroy(this.transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        spawner.enemiesSpawned -= 1;
    }

    public void ShowFloatingText()
    {
        GameObject floatingText = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        floatingText.GetComponent<TextMeshPro>().text = damaged.ToString();
    }

    public void MoveToPlayer()
    {
        if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange)
        {
            returnTimer = returnTimerConstant;
            FollowPlayer();
        }
        else if (Vector3.Distance(target.position, transform.position) >= maxRange)
        {
            if (returnTimer <= returnTimerConstant && returnTimer > 0)
            {
                returnTimer -= Time.deltaTime;
            }
            else if (returnTimer <= 0)
            {
                ReturnToSpawn();
            }
        }
    }

    public void AttackTimer()
    {
        if (attackTimer > 0 && attacked)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer <= 0)
        {
            attackTimer = attackTimerConstant;
            attacked = false;
        }
    }

    public void AttackPlayer()
    {
        if (Vector3.Distance(target.position, transform.position) <= minRange && attacked == false)
        {
            attacked = true;
            player.TakeDamage(damage);
        }
    }
}
