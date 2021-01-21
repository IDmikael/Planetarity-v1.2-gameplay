using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Button btnLoad;
    [SerializeField] private GameObject coolStory;

    [SerializeField] private float coolStoryDestinationZ;
    [SerializeField] private float coolStoryMoveSpeed = 10f;

    private void Start()
    {
        btnLoad.interactable = SaveLoadSystem.CheckForDataFile();
        // Just read it and you can't hold back your tears
        StartCoroutine(CoolStoryMovementCoroutine(0.02f));
    }

    #region BTNS CALLBACKS
    public void OnBtnPlay()
    {
        PlayerPrefs.SetString(PrefsNames.GAME_MODE, PrefsNames.GAME_MODE_NEW_GAME);

        SceneManager.LoadSceneAsync(Constants.SCENE_INDEX_GAME);
    }

    public void OnBtnLoad()
    {
        PlayerPrefs.SetString(PrefsNames.GAME_MODE, PrefsNames.GAME_MODE_LOAD_GAME);

        SceneManager.LoadSceneAsync(Constants.SCENE_INDEX_GAME);
    }
    #endregion

    private IEnumerator CoolStoryMovementCoroutine(float updateTime)
    {
        Vector3 startPos = coolStory.transform.position;
        while (coolStory.transform.position.z < coolStoryDestinationZ)
        {
            coolStory.transform.position = new Vector3(coolStory.transform.position.x, coolStory.transform.position.y, coolStory.transform.position.z + coolStoryMoveSpeed);
            yield return new WaitForSeconds(updateTime);
        }

        coolStory.transform.position = startPos;
        StartCoroutine(CoolStoryMovementCoroutine(0.02f));
    }
}
