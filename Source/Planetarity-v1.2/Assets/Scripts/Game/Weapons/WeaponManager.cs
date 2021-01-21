using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds weapon models, returns a random one or returns model depending on desired weapon type. Thats all
/// </summary>
public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] allWeapons;

    private List<WeaponModel> weaponsList;

    public void Init()
    {
        // Setup weapons list
        weaponsList = new List<WeaponModel>();

        foreach (var weapon in allWeapons)
        {
            weaponsList.Add(new WeaponModel(weapon));
        }
    }

    public WeaponModel GetRandomWeapon()
    {
        return weaponsList[Random.Range(0, weaponsList.Count)];
    }

    public WeaponModel GetWeaponModelByType(WeaponType type)
    {
        WeaponModel model = weaponsList[0];

        foreach (var weapon in weaponsList)
        {
            if (weapon.weapon.Type == type)
                return weapon;
        }

        return model;
    }
}
