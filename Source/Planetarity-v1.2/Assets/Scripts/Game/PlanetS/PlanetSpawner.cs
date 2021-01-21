using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for planets spawn on certain orbit around the Sun
/// </summary>
public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;

    [SerializeField] private int minDistanceBetweenPlanets = 3;
    [SerializeField] private float maxPlanetMovementSpeed = 100f;

    private int minPlanetsCount;
    private int maxPlanetsCount;

    /// <summary>
    /// Spawn new planet with randomization
    /// </summary>
    public void InitialPlanetsSpawn()
    {
        minPlanetsCount = PlayerPrefs.GetInt(PrefsNames.ENEMIES_MIN_VALUE, 3);
        maxPlanetsCount = PlayerPrefs.GetInt(PrefsNames.ENEMIES_MAX_VALUE, 5);

        // Generate random planets count depending on min and max values
        int count = Random.Range(minPlanetsCount, maxPlanetsCount);
        List<int> radiuses = new List<int>();

        for (int i = 0; i < count; i++)
        {
            // Randomize orbit radius preventing several planets on same orbit
            int radius = RandomizeRadius();
            while (radiuses.Contains(radius))
            {
                radius = RandomizeRadius();
            }
            radiuses.Add(radius);

            // Instantinate planet and spawn it on random pos on orbit
            GameObject planet = Instantiate(planetPrefab, new Vector2(0, radius), Quaternion.identity);
            planet.transform.RotateAround(Vector3.zero, Vector3.forward, Random.Range(0, 360));

            // Specify random speed with min 10 (light kms or whatever you like)
            float speed = Random.Range(10f, maxPlanetMovementSpeed);

            // Init planet
            planet.GetComponent<PlanetController>().Init(GameController.Instance.weaponManager.GetRandomWeapon());

            // Add planet to list of planets which should be rotated around the Sun
            GameController.Instance.planetMovement.AddPlanet(new Planet(planet, radius, speed));
        }

    }

    /// <summary>
    /// Spawning planet from save. Randomization not found (404)
    /// </summary>
    public void LoadPlanet(float[] position, int radius, float rotationSpeed, WeaponType weaponType, float[] color, float hpAmount, float hpCurrent, float weight)
    {
        GameObject planet = Instantiate(planetPrefab, new Vector2(0, radius), Quaternion.identity);
        planet.transform.position = new Vector3(position[0], position[1], position[2]);

        // Init planet
        planet.GetComponent<PlanetController>().Load(
            GameController.Instance.weaponManager.GetWeaponModelByType(weaponType), weight, hpAmount, hpCurrent, 
            new Color(color[0], color[1], color[2], color[3])
            );

        GameController.Instance.planetMovement.AddPlanet(new Planet(planet, radius, rotationSpeed));
    }

    /// <summary>
    /// Randomize planet's distance to sun with min distance: minDistanceBetweenPlanets, step: minDistanceBetweenPlanets
    /// </summary>
    private int RandomizeRadius()
    {
        int randomRadius = Random.Range(minDistanceBetweenPlanets, minDistanceBetweenPlanets * maxPlanetsCount);
        int numSteps = Mathf.FloorToInt(randomRadius / minDistanceBetweenPlanets);

        return numSteps * minDistanceBetweenPlanets;
    }
}
