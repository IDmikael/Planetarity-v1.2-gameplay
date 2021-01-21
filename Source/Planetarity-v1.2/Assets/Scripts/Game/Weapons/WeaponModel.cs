using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds weapon. 
/// If there will be any weapon changes during game (for example player uses spell to increase cooldown of enemies), it can be done here
/// </summary>
public class WeaponModel
{
    // Assigned weapon
    public Weapon weapon;

    public float damage;
    public float acceleration;
    public float cooldown;

    public WeaponModel(Weapon _weapon)
    {
        weapon = _weapon;

        damage = weapon.Damage;
        acceleration = weapon.Acceleration;
        cooldown = weapon.CooldownTime;
    }
}
