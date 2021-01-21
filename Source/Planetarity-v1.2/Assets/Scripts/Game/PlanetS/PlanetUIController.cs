using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple UI controller. 24 yo. 
/// Can change hp and cooldown progress. Also can indicate player planet and responsible for identity rotation of all this views (but not good at it honestly)
/// </summary>
public class PlanetUIController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [Tooltip("A view to indicate which planet is player (because it was really hard without this)")]
    [SerializeField] private GameObject playerHello;
    [SerializeField] private Image hpProgress;
    [SerializeField] private Image cooldownProgress;

    private void Update()
    {
        if (canvas.gameObject.transform.rotation != Quaternion.identity)
            canvas.gameObject.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Show Player indicatior and start a coroutine to close it after some time. Also it's clickable.
    /// </summary>
    public void ShowPlayerHello()
    {
        playerHello.SetActive(true);
        StartCoroutine(PlayerHelloCoroutine(3f));
    }

    public void OnPlayerHelloPress()
    {
        playerHello.SetActive(false);
        StopAllCoroutines();
    }

    public void UpdateHpProgress(float current, float max)
    {
        hpProgress.fillAmount = current / max;
    }

    public void UpdateCooldownProgress(float current, float max)
    {
        if (current <= 0)
            cooldownProgress.gameObject.SetActive(false);
        else
        {
            if (!cooldownProgress.gameObject.activeSelf)
                cooldownProgress.gameObject.SetActive(true);

            cooldownProgress.fillAmount = current / max;
        }
    }

    private IEnumerator PlayerHelloCoroutine(float showTime)
    {
        yield return new WaitForSeconds(showTime);

        playerHello.SetActive(false);
    }
}
