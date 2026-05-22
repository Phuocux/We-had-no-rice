using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float swordAttackCD = .5f;

    private Transform weaponCollider;
    private Animator myAnimator;


    private GameObject slashAnim;

    private System.Action<InputAction.CallbackContext> attackStarted;
    private System.Action<InputAction.CallbackContext> attackCanceled;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();

        //attackStarted = _ => StartAttacking();
        //attackCanceled = _ => StopAttacking();
    }

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;

    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        if (myAnimator == null) return;

            //isAttacking = true;

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

    private IEnumerator AttackCDRoutine()
    {
        yield return new WaitForSeconds(swordAttackCD);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
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

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        if (slashAnim == null) return;

        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos =
            Mouse.current.position.ReadValue();

        Vector3 playerScreenPoint =
            Camera.main.WorldToScreenPoint(
                PlayerController.Instance.transform.position
            );

        Vector2 direction =
            mousePos - playerScreenPoint;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation =
                Quaternion.Euler(-180, 0, -angle);

            weaponCollider.transform.rotation =
                Quaternion.Euler(-180, 0, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation =
                Quaternion.Euler(0, 0, angle);

            weaponCollider.transform.rotation =
                Quaternion.Euler(0, 0, 0);
        }
    }
}
