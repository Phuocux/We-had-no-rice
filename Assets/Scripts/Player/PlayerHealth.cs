using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    private AudioManager audioManager;

    const string HEALTH_SLIDER_NAME = "Health Slider";
    const string GAME_OVER_SCENE_NAME = "GameOverScene";
    const string TOWN_TEXT = "Scene1";



    protected override void Awake()
    {
        isDead = false;
        base.Awake();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        currentHealth = maxHealth;

        UpdateHealthSlider();

    }



    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        audioManager.PlaySFX(audioManager.playerHurtSFX);
        StartCoroutine(flash.FlashRoutine());
        ScreenShakeManager.Instance.ShakeScreen();
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead )
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            Debug.Log("Player Death");
            //GetComponent<Animator>().SetTrigger(DEATH_HASH);
            //StartCoroutine(DeathLoadSceneRoutine());
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        //yield return new WaitForSeconds(2f);
        //Destroy(gameObject);
        //SceneManager.LoadScene(TOWN_TEXT);
        

    if (PlayerAmmo.Instance != null)
        Destroy(PlayerAmmo.Instance.gameObject);

    if (EconomyManager.Instance != null)
        Destroy(EconomyManager.Instance.gameObject);

    if (SceneManagement.Instance != null)
        Destroy(SceneManagement.Instance.gameObject);

    if (Stamina.Instance != null)
        Destroy(Stamina.Instance.gameObject);

    yield return null;

    SceneManager.LoadScene("menuScene");

    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_NAME).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
