using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;


    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private Animator myAnimator;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        myAnimator = GetComponent<Animator>();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        audioManager.PlaySFX(audioManager.bow_AttackSFX);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
