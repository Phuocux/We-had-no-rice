using UnityEngine;

public class BlueSlimeAI : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Wander")]
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float wanderWaitTime = 2f;
    [SerializeField] private float wanderSpeedMultiplier = 0.5f;

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool canAttack = true;

    private EnemyAudio enemyAudio;

    private Vector2 spawnPosition;
    private Vector2 wanderTarget;
    private float wanderTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAudio = GetComponent<EnemyAudio>();

        if (PlayerController.Instance != null)
            player = PlayerController.Instance.transform;

        spawnPosition = transform.position;
        ChooseNewWanderPoint();
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        // Nếu thấy người chơi thì truy đuổi
        if (distance <= detectRange)
        {
            ChasePlayer(distance);
        }
        // Nếu không thấy thì đi lang thang quanh điểm spawn
        else
        {
            Wander();
        }
    }

    private void ChasePlayer(float distance)
    {
        Vector2 direction =
            (player.position - transform.position).normalized;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0;
        }

        if (distance > attackRange)
        {
            rb.MovePosition(
                rb.position +
                direction * moveSpeed * Time.fixedDeltaTime
            );
        }
        else
        {
            Attack();
        }
    }

    private void Wander()
    {
        Vector2 direction =
            wanderTarget - rb.position;

        if (direction.magnitude < 0.2f)
        {
            wanderTimer -= Time.fixedDeltaTime;

            if (wanderTimer <= 0f)
            {
                ChooseNewWanderPoint();
            }

            return;
        }

        direction.Normalize();

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0;
        }

        rb.MovePosition(
            rb.position +
            direction *
            (moveSpeed * wanderSpeedMultiplier) *
            Time.fixedDeltaTime
        );
    }

    private void ChooseNewWanderPoint()
    {
        wanderTarget =
            spawnPosition +
            Random.insideUnitCircle * wanderRadius;

        wanderTimer = wanderWaitTime;
    }

    private void Attack()
    {
        if (!canAttack)
            return;

        canAttack = false;

        Vector2 direction =
            (player.position - transform.position).normalized;

        rb.MovePosition(
            rb.position + direction * 2f
        );

        enemyAudio?.PlayAttack();

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            Application.isPlaying
                ? (Vector3)spawnPosition
                : transform.position,
            wanderRadius
        );
    }
#endif
}