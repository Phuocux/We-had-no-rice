using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Magazine")]
    [SerializeField] private int magazineSize = 12;
    [SerializeField] private float reloadTime = 1.5f;

    private int currentMagazineAmmo;

    private Animator myAnimator;
    private AudioManager audioManager;
    private SpriteRenderer mySpriteRenderer;

    private bool isReloading;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake()
    {
        audioManager =
            GameObject.FindGameObjectWithTag("Audio")
            .GetComponent<AudioManager>();

        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        currentMagazineAmmo = magazineSize;
    }

    private void Update()
    {
        MouseFollowWithOffset();

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reload();
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

        ActiveWeapon.Instance.transform.rotation =
            Quaternion.Euler(0, 0, angle);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipY = true;
        }
        else
        {
            mySpriteRenderer.flipY = false;
        }
    }

    public void Attack()
    {
        if (isReloading)
            return;

        if (currentMagazineAmmo <= 0)
        {
            Reload();
            return;
        }

        currentMagazineAmmo--;

        if (myAnimator != null)
        {
            myAnimator.SetTrigger(FIRE_HASH);
        }

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                ActiveWeapon.Instance.transform.rotation
            );

        bullet.GetComponent<Projectile>()
            .UpdateProjectileRange(weaponInfo.weaponRange);

        audioManager.PlaySFX(audioManager.bow_AttackSFX);

        Debug.Log(
            $"Ammo: {currentMagazineAmmo}/{PlayerAmmo.Instance.reserveAmmo}"
        );
    }

    public void Reload()
    {
        if (isReloading)
            return;

        if (currentMagazineAmmo >= magazineSize)
            return;

        if (PlayerAmmo.Instance.reserveAmmo <= 0)
            return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo =
            magazineSize - currentMagazineAmmo;

        int ammoToLoad =
            Mathf.Min(
                neededAmmo,
                PlayerAmmo.Instance.reserveAmmo
            );

        currentMagazineAmmo += ammoToLoad;

        PlayerAmmo.Instance.RemoveAmmo(ammoToLoad);

        isReloading = false;

        Debug.Log(
            $"Reloaded: {currentMagazineAmmo}/{PlayerAmmo.Instance.reserveAmmo}"
        );
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public int GetCurrentAmmo()
    {
        return currentMagazineAmmo;
    }

    public int GetMagazineSize()
    {
        return magazineSize;
    }
}