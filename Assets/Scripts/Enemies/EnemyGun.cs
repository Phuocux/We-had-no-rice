using System.Collections;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float weaponRange = 10f;

    [Header("AI")]
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int bulletsPerBurst = 5;
    [SerializeField] private float timeBetweenShots = 0.15f;

    private Transform player;

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private EnemyAudio enemyAudio;

    private bool canAttack = true;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyAudio = GetComponent<EnemyAudio>();
    }

    private void Start()
    {
        if (PlayerController.Instance != null)
            player = PlayerController.Instance.transform;
    }

    private void Update()
    {
        if (player == null)
            return;

        AimAtPlayer();

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (distance <= attackRange && canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void AimAtPlayer()
    {
        Vector2 direction =
            player.position - transform.position;

        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            Fire();

            yield return new WaitForSeconds(timeBetweenShots);
        }

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private void Fire()
    {
        if (bulletPrefab == null ||
            bulletSpawnPoint == null)
            return;

        if (myAnimator != null)
        {
            myAnimator.SetTrigger(FIRE_HASH);
        }

        enemyAudio?.PlayAttack();

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                transform.rotation
            );

        Projectile projectile =
            bullet.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.UpdateProjectileRange(weaponRange);

            // Bật dòng này nếu đã thêm hàm
            // projectile.SetEnemyProjectile(true);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
#endif
}