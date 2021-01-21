using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private PauseWindowController pauseWindow;
    [SerializeField] private EndGameWindowController endGameWindow;

    [SerializeField] private Weapon firstWeapon;
    [SerializeField] private Weapon secondWeapon;
    [SerializeField] private Weapon thirdWeapon;

    [SerializeField] private GameObject fWeaponBtn;
    [SerializeField] private GameObject sWeaponBtn;
    [SerializeField] private GameObject tWeaponBtn;

    private void Start()
    {
        // Setup bottom btns images
        fWeaponBtn.transform.GetChild(0).GetComponent<Image>().sprite = firstWeapon.ObjectSprite;
        sWeaponBtn.transform.GetChild(0).GetComponent<Image>().sprite = secondWeapon.ObjectSprite;
        tWeaponBtn.transform.GetChild(0).GetComponent<Image>().sprite = thirdWeapon.ObjectSprite;
    }

    public void ShowEndGameWindow(string title)
    {
        Time.timeScale = 0;
        endGameWindow.Show(title);
    }

    public void OnBtnPausePressed()
    {
        // Show pause UI
        Time.timeScale = 0;
        pauseWindow.Show();
    }

    #region BOTTOM BTNS CALLBACKS
    public void OnFirstWeaponBtnPressed()
    {
        GameController.Instance.playerGameObject.GetComponent<PlanetController>().ChangeWeapon(new WeaponModel(firstWeapon));

        UpdateTicksDependingOnCurWeapon();
    }

    public void OnSecondWeaponBtnPressed()
    {
        GameController.Instance.playerGameObject.GetComponent<PlanetController>().ChangeWeapon(new WeaponModel(secondWeapon));

        UpdateTicksDependingOnCurWeapon();
    }

    public void OnThirdWeaponBtnPressed()
    {
        GameController.Instance.playerGameObject.GetComponent<PlanetController>().ChangeWeapon(new WeaponModel(thirdWeapon));
        
        UpdateTicksDependingOnCurWeapon();
    }

    #endregion

    public void UpdateTicksDependingOnCurWeapon()
    {
        // Not a good code, yeah. I'll change it in 5 minutes (no)
        WeaponType type = GameController.Instance.playerGameObject.GetComponent<PlanetController>().weapon.weapon.Type;

        // Child with index 1 is a tick, so player could see what weapon he holds now
        switch (type)
        {
            case WeaponType.Rocket:
                fWeaponBtn.transform.GetChild(1).gameObject.SetActive(true);
                sWeaponBtn.transform.GetChild(1).gameObject.SetActive(false);
                tWeaponBtn.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case WeaponType.Blaster:
                fWeaponBtn.transform.GetChild(1).gameObject.SetActive(false);
                sWeaponBtn.transform.GetChild(1).gameObject.SetActive(true);
                tWeaponBtn.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case WeaponType.NuclearBomb:
                fWeaponBtn.transform.GetChild(1).gameObject.SetActive(false);
                sWeaponBtn.transform.GetChild(1).gameObject.SetActive(false);
                tWeaponBtn.transform.GetChild(1).gameObject.SetActive(true);
                break;
        }

    }
}
