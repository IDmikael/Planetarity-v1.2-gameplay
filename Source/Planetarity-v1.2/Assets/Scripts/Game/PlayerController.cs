using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gets mouse position, changes it to world point, rotating player planet towards this direction and shooting
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static Action OnPlayerDeath = delegate { };

    private PlanetController planetController;

    private float offset = 4f;

    public void Init()
    {
        planetController = GetComponent<PlanetController>();
    }
    
    private void Update()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane - offset));

        if (Input.GetMouseButtonDown(0))
        {
            // Same as in planet controller
            transform.LookAt(2 * transform.position - worldPosition);
            planetController.Shoot(Vector3.zero, true);
        }
    }
}
