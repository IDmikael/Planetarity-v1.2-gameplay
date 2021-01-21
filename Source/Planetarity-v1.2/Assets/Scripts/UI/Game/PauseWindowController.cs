using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWindowController : MonoBehaviour
{
    [SerializeField] private Button btnLoad;

    public void Show()
    {
        gameObject.SetActive(true);
        btnLoad.interactable = SaveLoadSystem.CheckForDataFile();
    }

    #region BTNS CALLBACKS
    public void OnBtnResume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void OnBtnSave()
    {
        SaveLoadSystem.SaveData();
    }

    public void OnBtnLoad()
    {
        Time.timeScale = 1;

        PlayerPrefs.SetString(PrefsNames.GAME_MODE, PrefsNames.GAME_MODE_LOAD_GAME);

        var op = SceneManager.LoadSceneAsync(Constants.SCENE_INDEX_GAME);
        op.completed += (async) =>
        {
            Time.timeScale = 1;
        };
    }

    public void OnBtnMenu()
    {
        Time.timeScale = 1;

        var op = SceneManager.LoadSceneAsync(Constants.SCENE_INDEX_MENU);
        op.completed += (async) =>
        {
            Time.timeScale = 1;
        };
    }
    #endregion
}
