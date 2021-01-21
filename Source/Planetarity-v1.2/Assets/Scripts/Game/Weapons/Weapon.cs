using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon", fileName = "New Weapon")]
public class Weapon : ScriptableObject
{
    [Tooltip("Weapon's image")]
    [SerializeField] private Sprite objectSprite;
    public Sprite ObjectSprite { get => objectSprite; private set { } }

    [Tooltip("Weapon type")]
    [SerializeField] private WeaponType type;
    public WeaponType Type { get => type; private set { } }

    [Tooltip("Weapon's explosion effect")]
    [SerializeField] private GameObject explosionEffect;
    public GameObject ExplosionEffect { get => explosionEffect; private set { } }

    [Tooltip("Weapon's damage")]
    [SerializeField] private float damage;
    public float Damage { get => damage; private set { } }

    [Tooltip("Weapon's acceleration")]
    [SerializeField] private float acceleration;
    public float Acceleration { get => acceleration; private set { } }

    [Tooltip("Weapon's cooldown")]
    [SerializeField] private float cooldownTime;
    public float CooldownTime { get => cooldownTime; private set { } }
}

public enum WeaponType
{
    Rocket,
    Blaster,
    NuclearBomb
}