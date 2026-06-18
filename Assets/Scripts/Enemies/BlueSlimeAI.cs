using UnityEngine;

public class BlueSlimeAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool canAttack = true;

    private EnemyAudio enemyAudio;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyAudio = GetComponent<EnemyAudio>();

        if (PlayerController.Instance != null)
            player = PlayerController.Instance.transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        Vector2 direction =
            (player.position - transform.position).normalized;

        // Flip
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0;
        }

        if (distance > detectRange)
            return;

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

    private void Attack()
    {
        if (!canAttack) return;

        canAttack = false;

        Vector2 direction =
            (player.position - transform.position).normalized;

        rb.MovePosition(
            rb.position + direction * 2f
        );

        enemyAudio?.PlayAttack();
        animator.SetTrigger("Attack");

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}