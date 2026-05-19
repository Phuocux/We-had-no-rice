using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCD = .5f;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private bool attackButtonDown;
    private bool isAttacking;

    private GameObject slashAnim;

    private System.Action<InputAction.CallbackContext> attackStarted;
    private System.Action<InputAction.CallbackContext> attackCanceled;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();

        playerControls = new PlayerControls();

        attackStarted = _ => StartAttacking();
        attackCanceled = _ => StopAttacking();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Combat.Attack.started += attackStarted;
        playerControls.Combat.Attack.canceled += attackCanceled;
    }

    private void OnDisable()
    {
        playerControls.Combat.Attack.started -= attackStarted;
        playerControls.Combat.Attack.canceled -= attackCanceled;

        playerControls.Disable();
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (myAnimator == null) return;

        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;

            myAnimator.SetTrigger("Attack");

            if (weaponCollider != null)
                weaponCollider.gameObject.SetActive(true);

            slashAnim = Instantiate(
                slashAnimPrefab,
                slashAnimSpawnPoint.position,
                Quaternion.identity
            );

            slashAnim.transform.parent = transform.parent;

            StartCoroutine(AttackCDRoutine());
        }
    }

    private IEnumerator AttackCDRoutine()
    {
        yield return new WaitForSeconds(swordAttackCD);
        isAttacking = false;
    }

    public void DoneAttackingAnimEvent()
    {
        if (weaponCollider != null)
            weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        if (slashAnim == null) return;

        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController != null && playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        if (slashAnim == null) return;

        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController != null && playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        if (Camera.main == null) return;
        if (playerController == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();

        Vector3 playerScreenPoint =
            Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle =
            Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation =
                Quaternion.Euler(0, -180, angle);

            weaponCollider.transform.rotation =
                Quaternion.Euler(0, -180, 0);
        }
        else
        {
            activeWeapon.transform.rotation =
                Quaternion.Euler(0, 0, angle);

            weaponCollider.transform.rotation =
                Quaternion.Euler(0, 0, 0);
        }
    }
}