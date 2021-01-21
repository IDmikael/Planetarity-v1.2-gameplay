using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls weapon (or bullet?) flight. Direction, acceleration, explosion and so on.
/// </summary>
public class WeaponController : MonoBehaviour
{
    [Tooltip("0 - rocket, 1 - blaster, 2 - nuclear bomb obj")]
    [SerializeField] private GameObject[] models;

    public WeaponModel weaponModel;
    private GameObject explosionEffect;

    private Rigidbody rb;
    private float acceleration;
    private float damageAmount;
    // Position from which weapon starts flight
    private Transform firePos;

    // Objects who shot this weapon
    private GameObject shooter;
    // Additional force applied only if weapon is in area of other planet
    private Vector3 gravity;

    /// <summary>
    /// Initialization and clearing some values
    /// </summary>
    public void Init(WeaponModel _weaponModel, Transform _firePos, GameObject _shooter)
    {
        weaponModel = _weaponModel;
        EnableObjDepeningOnWeaponType(weaponModel.weapon.Type);
        SetupValues();

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        firePos = _firePos;
        shooter = _shooter;

        gravity = Vector3.zero;

        StartCoroutine(MoveCoroutine(0.1f));
        rb.freezeRotation = true;
        StartCoroutine(LifetimeCoroutine(3f));
    }

    /// <summary>
    /// Because weapons come from pool this done every init to prevent suspicious behaviour because of old data impact
    /// </summary>
    private void SetupValues()
    {
        acceleration = weaponModel.acceleration;
        explosionEffect = weaponModel.weapon.ExplosionEffect;
        damageAmount = weaponModel.damage;
    }

    /// <summary>
    /// There are 3 models in weapon prefab and depending on type of weapon this method turns one of them and turns off others
    /// </summary>
    private void EnableObjDepeningOnWeaponType(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Rocket:
                models[0].SetActive(true);
                models[1].SetActive(false);
                models[2].SetActive(false);
                break;
            case WeaponType.Blaster:
                models[0].SetActive(false);
                models[1].SetActive(true);
                models[2].SetActive(false);
                break;
            case WeaponType.NuclearBomb:
                models[0].SetActive(false);
                models[1].SetActive(false);
                models[2].SetActive(true);
                break;
        }
    }

    #region TRIGGERS AND COLLISIONS
    /// <summary>
    /// If weapon gets in planets area a force that magnifying it to this planet applies to weapons moving direction
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != shooter)
        {
            // Apply other planet's gravity
            if (other.CompareTag(Constants.TAG_SUN))
                gravity = (other.transform.position - transform.position).normalized * Constants.GRAVITY_SUN;
            else if (other.CompareTag(Constants.TAG_PLANET))
                gravity = (other.transform.position - transform.position).normalized * other.GetComponent<PlanetController>().weight;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != shooter)
        {
            // Remove other planet's gravity
            gravity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Constants.TAG_SUN))
        {
            ReturnToPool(weaponModel.weapon.Type);
        }
        if (collision.collider.CompareTag(Constants.TAG_PLANET) && collision.collider.gameObject != shooter)
        {
            ReturnToPool(weaponModel.weapon.Type);

            // Apply damage to collided planet
            collision.collider.GetComponent<PlanetController>().TakeDamage(damageAmount);
        }
    }

    #endregion

    #region COROUTINES
    private IEnumerator MoveCoroutine(float updateTime)
    {
        while (true)
        {
            rb.AddForce(gravity + firePos.up * acceleration, ForceMode.Impulse);

            yield return new WaitForSeconds(updateTime);
        }
    }

    /// <summary>
    /// After some amount of time weapon explodes and returns to pool
    /// </summary>
    private IEnumerator LifetimeCoroutine(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        ReturnToPool(weaponModel.weapon.Type);
    }

    private void ReturnToPool(WeaponType type)
    {
        GameController.Instantiate(explosionEffect, transform.position, Quaternion.identity);

        WeaponPool.Instance.ReturnToPool(this);

        StopAllCoroutines();
    }
    #endregion
}
