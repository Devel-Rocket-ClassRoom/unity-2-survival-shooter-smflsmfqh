using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity   
{

    public Image uiDamaged;
    public Image uiHealth;
    public Image uiGameOver;
    public TextMeshProUGUI gameOverText;

    private float uiDamagedDuration = 0.2f; 
    private float uiGameOverDuration = 3.0f;
    private float dieAnimationTimer = 2f;
    private float maxHealth;    

    public AudioClip deathClip;
    public AudioClip hitClip;

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    private void Awake()
    {
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();

        gameOverText = gameOverText.GetComponent<TextMeshProUGUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Health *= 10;
        maxHealth = Health;

        playerMovement.enabled = true;  
        playerShooter.enabled = true;   

        uiDamaged.enabled = false;
        uiGameOver.enabled = false;
        gameOverText.enabled = false;
    }

    private void Update()
    {
        
        if (uiDamaged.enabled)
        {
            uiDamagedDuration -= Time.deltaTime;
            if (uiDamagedDuration <= 0f)
            {
                uiDamaged.enabled = false;
                uiDamagedDuration = 0.2f; 
            }
        }
        if (IsDead)
        {
            dieAnimationTimer -= Time.deltaTime;
            if (dieAnimationTimer <= 0f)
            {
                uiGameOver.enabled = true;
                gameOverText.enabled = true;
            }

            if (uiGameOver.enabled)
            {
                uiGameOverDuration -= Time.deltaTime;
                if (uiGameOverDuration <= 0f)
                {
                    uiGameOver.enabled = false;
                    gameOverText.enabled = false;

                    uiGameOverDuration = 3.0f;
                    RestartLevel();
                }
            }
        }

    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    { 
        if (!IsDead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
            uiDamaged.enabled = true;
            
            base.OnDamage(damage, hitPoint, hitNormal);
            uiHealth.fillAmount = Health / maxHealth; 

        }
    }

    public override void Die()
    {
        if (IsDead) return;
        base.Die();

        uiHealth.gameObject.SetActive(false);


        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
