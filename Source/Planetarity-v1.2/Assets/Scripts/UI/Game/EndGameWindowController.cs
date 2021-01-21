using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameWindowController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button btnLoad;

    public void Show(string titleText)
    {
        gameObject.SetActive(true);

        title.text = titleText;
        btnLoad.interactable = SaveLoadSystem.CheckForDataFile();
    }

    #region BTNS CALLBACKS
    public void OnBtnRestart()
    {
        PlayerPrefs.SetString(PrefsNames.GAME_MODE, PrefsNames.GAME_MODE_NEW_GAME);

        var op = SceneManager.LoadSceneAsync(Constants.SCENE_INDEX_GAME);
        op.completed += (async) =>
        {
            Time.timeScale = 1;
        };
    }

    public void OnBtnLoad()
    {
        PlayerPrefs.SetString(PrefsNames.GAME_MODE, PrefsNames.GAME_MODE_LOAD_GAME);

        var op = SceneManager.LoadSceneAsync(Constants.SCENE_INDEX_GAME);
        op.completed += (async) =>
        {
            Time.timeScale = 1;
        };
    }

    public void OnBtnMenu()
    {
        var op = SceneManager.LoadSceneAsync(Constants.SCENE_INDEX_MENU);
        op.completed += (async) =>
        {
            Time.timeScale = 1;
        };
    }
    #endregion
}
