using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main game class. Holds all main objects and initializes them on start. Subscribed on most valuable game actions (lose/win).
/// Has a singleton so accessible from anywhere. Also starts new game or loads old depending on value GAME_MODE in PlayerPrefs.
/// </summary>
public class GameController : MonoBehaviour
{
    public PlanetMovementController planetMovement;
    public PlanetSpawner planetSpawner;

    public WeaponManager weaponManager;

    public GameUI gameUI;

    public static GameController Instance { get; private set; }

    // Needed for player identification from other planet's objects
    public GameObject playerGameObject;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        weaponManager.Init();

        planetMovement.Init();

        string gameMode = PlayerPrefs.GetString(PrefsNames.GAME_MODE, PrefsNames.GAME_MODE_NEW_GAME);

        if (gameMode == PrefsNames.GAME_MODE_LOAD_GAME)
        {
            LoadGame();
        }
        else
        {
            StartNewGame();
        }

        PlayerController.OnPlayerDeath += PlayerController_OnPlayerDeath;
        PlanetMovementController.OnWin += PlanetMovement_OnWin;
        PlanetMovementController.OnDraw += PlanetMovement_OnDraw;
    }

    #region GAME START
    private void StartNewGame()
    {
        planetSpawner.InitialPlanetsSpawn();

        SetupNewPlayer();

        FinishSetup();

        planetMovement.StartMovement();
    }

    private void LoadGame()
    {
        GameData data = SaveLoadSystem.LoadData();

        for (int i = 0; i < data.planetsCount; i++)
        {
            PlanetData planet = data.planets[i];
            planetSpawner.LoadPlanet(planet.position, planet.radius, planet.rotationSpeed, planet.weaponType, planet.color, planet.hpAmount, planet.hpCurrent, planet.weight);

            if (planet.isPlayer)
            {
                LoadPlayer(planetMovement.planets[i]);
            }
        }

        FinishSetup();

        planetMovement.StartMovement();

        PlayerPrefs.SetString(PrefsNames.GAME_MODE, PrefsNames.GAME_MODE_NEW_GAME);
    }

    #endregion

    #region PLAYER LOADING
    private void SetupNewPlayer()
    {
        // Nearest to sun planet will be a player
        int nearestRadiusToSun = 100;
        int nearestPlanetIndex = 0;

        for (int i = 0; i < planetMovement.planets.Count; i++)
        {
            if (planetMovement.planets[i].radius < nearestRadiusToSun)
            {
                nearestRadiusToSun = planetMovement.planets[i].radius;
                nearestPlanetIndex = i;
            }
        }

        Planet player = planetMovement.planets[nearestPlanetIndex];
        PlayerController controller = player.gameObject.AddComponent<PlayerController>();
        controller.Init();
        playerGameObject = player.gameObject;
    }

    private void LoadPlayer(Planet player)
    {
        PlayerController controller = player.gameObject.AddComponent<PlayerController>();
        controller.Init();
        playerGameObject = player.gameObject;
    }
    
    #endregion

    private void FinishSetup()
    {
        // Activating enemies planets
        foreach (var planet in planetMovement.planets)
        {
            if (planet.gameObject != playerGameObject)
            {
                planet.gameObject.GetComponent<PlanetController>().TurnAI();
            }
            else
            {
                planet.gameObject.GetComponent<PlanetController>().GetPlanetUIController().ShowPlayerHello();
            }
        }

        gameUI.UpdateTicksDependingOnCurWeapon();
    }

    #region ACTOINS SUBSCRIPTION CALLBACKS
    private void PlayerController_OnPlayerDeath()
    {
        gameUI.ShowEndGameWindow("You lose!");
    }

    private void PlanetMovement_OnWin()
    {
        gameUI.ShowEndGameWindow("You win!");
    }

    /// <summary>
    /// Theoretically its possible, so...
    /// </summary>
    private void PlanetMovement_OnDraw()
    {
        gameUI.ShowEndGameWindow("Wow, it's draw!");
    }

    #endregion

    // Unsubscription from this channel
    private void OnDestroy()
    {
        PlayerController.OnPlayerDeath -= PlayerController_OnPlayerDeath;
        PlanetMovementController.OnWin -= PlanetMovement_OnWin;
        PlanetMovementController.OnDraw -= PlanetMovement_OnDraw;
    }
}
