using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for local planet behaviour (shooting, taking damage, randomizing color/weight, etc)
/// </summary>
public class PlanetController : MonoBehaviour
{
    [Tooltip("Position from which weapon starts moving forward")]
    [SerializeField] private Transform firePos;

    // These values determining how strong gravitation planet will have. The more this value, the more changes in behavior of weapons in planet's area
    [SerializeField] private float minWeight = 1;
    [SerializeField] private float maxWeight = 15;

    // Planet's health
    [SerializeField] private float minHp = 30f;
    [SerializeField] private float maxHp = 60f;

    // Assigned weapons and controller for UI elements
    public WeaponModel weapon;
    private PlanetUIController UIController;

    // Weapon's cooldown
    private float shotDelay = 5;
    // Static variable that holds general amount of health
    public float hpAmount;
    // Dynamic var that changes when planet takes damage
    public float hpCurrent;

    public float weight = 2f;

    private bool isCooldown = false;

    #region INITIALIZATION
    /// <summary>
    /// Initialization of new weapon with randomizing some values
    /// </summary>
    public void Init(WeaponModel _weapon)
    {
        UIController = GetComponent<PlanetUIController>();

        // Assign random weapon to planet
        weapon = _weapon;
        weight = Random.Range(minWeight, maxWeight + 1);

        hpAmount = Random.Range(minHp, maxHp);
        hpCurrent = hpAmount;

        UIController.UpdateHpProgress(hpCurrent, hpAmount);

        // Set random color to a planet
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        shotDelay = weapon.cooldown;

        StartCoroutine(CooldownCoroutine(0.5f));
    }

    /// <summary>
    /// Method for loading planet from save. No randomization, only straight and strong values.
    /// </summary>
    public void Load(WeaponModel _weapon, float weight, float hpAmount, float hpCurrent, Color color)
    {
        UIController = GetComponent<PlanetUIController>();

        weapon = _weapon;
        this.weight = weight;

        this.hpAmount = hpAmount;
        this.hpCurrent = hpCurrent;
        UIController.UpdateHpProgress(hpCurrent, hpAmount);

        GetComponent<Renderer>().material.color = color;

        shotDelay = weapon.cooldown;

        StartCoroutine(CooldownCoroutine(0.5f));
    }

    #endregion

    public PlanetUIController GetPlanetUIController()
    {
        return UIController;
    }

    public void TurnAI()
    {
        // Haha, be careful, AI (hahaha) is turned on!1!!
        StartCoroutine(ShootCoroutine(shotDelay));
    }

    public void Shoot(Vector3 target, bool isPlayer = false)
    {
        if (isCooldown)
            return;

        // This "2 * transform.position - target" is used to mirror back mirrored transform
        if (!isPlayer)
            gameObject.transform.LookAt(2 * transform.position - target);

        // Get weapon controller from pool
        WeaponController controller = WeaponPool.Instance.Get(WeaponPool.Instance.transform);

        controller.Init(weapon, firePos, gameObject);
        controller.transform.position = firePos.position;
        controller.transform.rotation = firePos.rotation;

        StartCoroutine(CooldownCoroutine(0.5f));
    }

    public void TakeDamage(float damageAmount)
    {
        hpCurrent -= damageAmount;
        if (hpCurrent <= 0)
        {
            // If this game object is player then notify death =(
            if (gameObject == GameController.Instance.playerGameObject)
                PlayerController.OnPlayerDeath.Invoke();

            GameController.Instance.planetMovement.RemovePlanet(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            UIController.UpdateHpProgress(hpCurrent, hpAmount);
        }
    }

    /// <summary>
    /// Method for player to have an ability to change weapon during gameplay. Actually not only for player, but bots somehow dont change their weapons (too dumb)
    /// </summary>
    public void ChangeWeapon(WeaponModel _weapon)
    {
        StopAllCoroutines();
        weapon = _weapon;
        shotDelay = weapon.cooldown;

        StartCoroutine(CooldownCoroutine(0.5f));
    }

    /// <summary>
    /// Get nearest planet in which bot will shoot
    /// </summary>
    private Vector3 GetNearestPlanet()
    {
        Vector3 nearestPlanet = Vector3.zero;
        float nearestDistance = 100f;

        foreach (var planet in GameController.Instance.planetMovement.planets)
        {
            if (Vector3.Distance(transform.position, planet.gameObject.transform.position) < nearestDistance && planet.gameObject.transform.position != transform.position)
            {
                nearestDistance = Vector3.Distance(transform.position, planet.gameObject.transform.position);
                nearestPlanet = planet.gameObject.transform.position;
            }
        }

        return nearestPlanet;
    }

    #region COROUTINES
    private IEnumerator ShootCoroutine(float updateTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTime);
            Shoot(GetNearestPlanet());
        }
    }

    private IEnumerator CooldownCoroutine(float updateTime)
    {
        isCooldown = true;

        float cooldownTime = 0;
        UIController.UpdateCooldownProgress(cooldownTime, shotDelay);

        while (cooldownTime < shotDelay)
        {
            yield return new WaitForSeconds(updateTime);

            cooldownTime += updateTime;
            UIController.UpdateCooldownProgress(cooldownTime, shotDelay);
        }

        UIController.UpdateCooldownProgress(cooldownTime, shotDelay);
        isCooldown = false;
    }
    #endregion
}
