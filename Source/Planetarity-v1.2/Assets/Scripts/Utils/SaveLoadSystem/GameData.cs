using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save model class. Holds amount of active planets and array of their values. Used for save file serialization and deserialization
/// </summary>
[Serializable]
public class GameData
{
    public int planetsCount;
    public PlanetData[] planets;

    public GameData()
    {
        planetsCount = GameController.Instance.planetMovement.planets.Count;

        List<PlanetData> rawPlanets = new List<PlanetData>();

        // Cycle through all active planets of planetMovement class and pack them into array of planets values
        foreach (var planet in GameController.Instance.planetMovement.planets)
        {
            GameObject planetObj = planet.gameObject;
            PlanetController controller = planetObj.GetComponent<PlanetController>();
            float[] position = new float[] { planetObj.transform.position.x, planetObj.transform.position.y, planetObj.transform.position.z };

            Color planetColor = planetObj.GetComponent<Renderer>().material.color;
            float[] color = new float[] { planetColor.r, planetColor.g, planetColor.b, planetColor.a };

            rawPlanets.Add(new PlanetData(
                position, planet.radius, planet.rotationSpeed, controller.weapon.weapon.Type,
                color, controller.hpAmount, controller.hpCurrent, controller.weight, planetObj == GameController.Instance.playerGameObject
                ));
        }

        planets = rawPlanets.ToArray();
    }
}

[Serializable]
public class PlanetData
{
    public float[] position;
    public int radius;
    public float rotationSpeed;

    public WeaponType weaponType;
    public float[] color;
    public float hpAmount;
    public float hpCurrent;
    public float weight;

    public bool isPlayer;

    public PlanetData(float[] position, int radius, float rotationSpeed, WeaponType weaponType, float[] color, float hpAmount, float hpCurrent, float weight, bool isPlayer)
    {
        this.position = position;
        this.radius = radius;
        this.rotationSpeed = rotationSpeed;
        this.weaponType = weaponType;
        this.color = color;
        this.hpAmount = hpAmount;
        this.hpCurrent = hpCurrent;
        this.weight = weight;
        this.isPlayer = isPlayer;
    }
}
