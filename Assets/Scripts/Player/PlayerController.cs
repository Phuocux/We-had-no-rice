using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get; private set; }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Knockback knockback;

    private float startingMoveSpeed;

    private bool isDashing = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();

        startingMoveSpeed = moveSpeed;
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        UpdateCameraReference();

        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }
    private void UpdateCameraReference()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {   
        if(knockback.gettingKnockedBack) return;
        rb.MovePosition(
            rb.position + movement * (moveSpeed * Time.fixedDeltaTime)
        );
    }

    private void AdjustPlayerFacingDirection()
    {
        if (mainCamera == null)
            return;

        Vector2 mousePos =
            Mouse.current.position.ReadValue();

        Vector3 playerScreenPoint =
            mainCamera.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;

        yield return new WaitForSeconds(dashTime);

        moveSpeed = startingMoveSpeed;

        if (myTrailRenderer != null)
        {
            myTrailRenderer.emitting = false;
        }

        yield return new WaitForSeconds(dashCD);

        isDashing = false;
    }
}