using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for all planets movement around the Sun. Also holds a list of all available planets
/// </summary>
public class PlanetMovementController : MonoBehaviour
{
    [SerializeField] private Transform sun;

    [SerializeField] private Vector3 axis = Vector3.up;

    [Tooltip("Speed with which planet returns on it's orbit if not on it")]
    [SerializeField] private float radiusSpeed = 0.5f;

    [Tooltip("Movement update time, the lower this value, the smoother planets movement become")]
    [SerializeField] private float updateTime = 0.05f;

    private Vector3 desiredPosition;

    // list of all available planets
    public List<Planet> planets;

    public static Action OnWin = delegate { };
    public static Action OnDraw = delegate { };

    public void Init()
    {
        planets = new List<Planet>();
    }

    /// <summary>
    /// Starts a coroutine that moves planets
    /// </summary>
    public void StartMovement()
    {
        StartCoroutine(PlanetsMovementCoroutine(updateTime));
    }

    public void AddPlanet(Planet planet)
    {
        planets.Add(planet);
    }

    public void RemovePlanet(GameObject planetObj)
    {
        for (int i = 0; i < planets.Count; i++)
        {
            if (planetObj == planets[i].gameObject)
            {
                planets.Remove(planets[i]);
            }
        }

        if (planets.Count == 1)
        {
            // Check if last planet is player
            if (planets[0].gameObject == GameController.Instance.playerGameObject)
            {
                // And win! Conghratulations, wow, you're the best! It seems that was a hard task for you, but you've kicked it's ass!
                OnWin();
            }
        }
        else if (planets.Count == 0)
        {
            // If no more planets then let it be the draw
            OnDraw();
        }
    }

    private IEnumerator PlanetsMovementCoroutine(float updateTime)
    {
        while (planets.Count > 0)
        {
            foreach (var planet in planets)
            {
                Transform tr = planet.gameObject.transform;
                tr.RotateAround(sun.position, axis, planet.rotationSpeed * updateTime);
                desiredPosition = (tr.position - sun.position).normalized * planet.radius + sun.position;
                tr.position = Vector3.MoveTowards(tr.position, desiredPosition, updateTime * radiusSpeed);
            }

            yield return new WaitForSeconds(updateTime);
        }
    }
}

/// <summary>
/// Class that holds main values needed for planet rotation and gameObject of actual planet so it's accessible from anywhere
/// </summary>
public struct Planet
{
    public GameObject gameObject;
    public int radius;
    public float rotationSpeed;

    /// <summary>
    /// Constructor for planet movement. Holds GameObject gameObject, float radius, float radiusSpeed
    /// </summary>
    /// <param name="gameObject">Planet's game object</param>
    /// <param name="radius">Desired movement radius (relative to the Sun)</param>
    /// <param name="rotationSpeed">Desired movement speed</param>
    public Planet(GameObject gameObject, int radius, float rotationSpeed)
    {
        this.gameObject = gameObject;
        this.radius = radius;
        this.rotationSpeed = rotationSpeed;
    }
}