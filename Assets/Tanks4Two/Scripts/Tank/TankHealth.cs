using UnityEngine;
using UnityEngine.UI;


public class TankHealth : MonoBehaviour
{
    #region Declarations
    public bool isPlayerDead;
    [Range(0, 100)] public int playerHealth = 100;
    [Range(0, 100)] public int startingHealth = 100;

    [SerializeField] Image imgFrontHealthFill;
    [SerializeField] Image imgBackHealthFill;
    [SerializeField] Color32 colorFullHealth;
    [SerializeField] Color32 colorEmptyHealth;
    [SerializeField] GameObject prefabExplosion;

    AudioSource sourceExplosion;
    ParticleSystem particleExplosion;
    #endregion

    #region Main Methods
    private void Awake()
    {
        // Link explosion prefab children
        prefabExplosion.SetActive(false);
        sourceExplosion = prefabExplosion.GetComponent<AudioSource>();
        particleExplosion = prefabExplosion.GetComponent<ParticleSystem>();
     
        // Change background slider color
        imgBackHealthFill.color = colorEmptyHealth;
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        prefabExplosion.SetActive(false);
        playerHealth = startingHealth;
        isPlayerDead = false;
        UpdateHealthUI();
    }
    #endregion

    #region Helper Methods
    //
    private void KillPlayer()
    {
        // Play the effects for the death of the tank and deactivate it
        prefabExplosion.SetActive(true);
        particleExplosion.Play();
        sourceExplosion.Play();
        GetComponent<TankMovement>().DisableMovement();

        gameObject.SetActive(false);
    }

    //
    public void HealDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead
        playerHealth += Mathf.CeilToInt(amount);
        UpdateHealthUI();
    }

    //
    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead
        playerHealth -= Mathf.FloorToInt(amount);
        UpdateHealthUI();

        if (playerHealth <= 0 && !isPlayerDead)
        {
            KillPlayer();
        }
    }

    //
    private void UpdateHealthUI()
    {
        // Adjust the value and colour of the slider
        imgFrontHealthFill.fillAmount = playerHealth / 100;
        imgFrontHealthFill.color = Color.Lerp(colorEmptyHealth, colorFullHealth, playerHealth / startingHealth);
    }
    #endregion
}
