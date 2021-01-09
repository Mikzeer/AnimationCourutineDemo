using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : GenericSingleton<GameSceneManager>
{
    public enum GAMESCENE { WELCOME, LOGINREGISTRATION, MAINMENU, GAME, LOADING, NONE}
    List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
    [SerializeField] private GameObject prefabLoadingScreen = default;
    LoadingScreen loadingScreen;

    private void Start()
    {
        loadingScreen = CreateLoadingScreen();
        loadingScreen.gameObject.SetActive(false);
    }

    public void LoadScene(GAMESCENE gameScene)
    {
        switch (gameScene)
        {
            case GAMESCENE.WELCOME:
                SceneManager.LoadScene(0);
                break;
            case GAMESCENE.LOGINREGISTRATION:
                SceneManager.LoadScene(0);
                break;
            case GAMESCENE.MAINMENU:
                SceneManager.LoadScene(3);
                break;
            case GAMESCENE.GAME:
                SceneManager.LoadScene(0);
                break;
            case GAMESCENE.LOADING:
                SceneManager.LoadScene(0);
                break;
            case GAMESCENE.NONE:
                Application.Quit();
                break;
            default:
                break;
        }

    }

    public void LoadSceneAsync(GAMESCENE gameScene)
    {
        Debug.Log("Enter Load Async");
        asyncOperations.Clear();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneIndex = 0;
        switch (gameScene)
        {
            case GAMESCENE.WELCOME:
                SceneManager.LoadScene(0);
                break;
            case GAMESCENE.LOGINREGISTRATION:
                SceneManager.LoadScene(0);
                break;
            case GAMESCENE.MAINMENU:
                Debug.Log("Enter Load Async SWITCH");
                sceneIndex = 3;
                break;
            case GAMESCENE.GAME:
                sceneIndex = 2;
                break;
            case GAMESCENE.LOADING:
                SceneManager.LoadScene(0);
                break;
            case GAMESCENE.NONE:
                Application.Quit();
                break;
            default:
                break;
        }
        asyncOperations.Add(SceneManager.UnloadSceneAsync(currentSceneIndex));
        asyncOperations.Add(SceneManager.LoadSceneAsync(sceneIndex));
        LoadingScreen();
    }

    private IEnumerator LoadingScreen()
    {
        Debug.Log("Enter Login Screen");
        loadingScreen.gameObject.SetActive(true);

        yield return null;

        for (int i = 0; i < asyncOperations.Count; i++)
        {
            while (asyncOperations[i].isDone == false)
            {
                float totalSceneLoaded = 0;

                foreach (AsyncOperation operation in asyncOperations)
                {
                    totalSceneLoaded += operation.progress;
                }

                totalSceneLoaded = (totalSceneLoaded / asyncOperations.Count) * 100;
                loadingScreen.SetLoadedAmount(totalSceneLoaded);
                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
    }

    private LoadingScreen CreateLoadingScreen()
    {
        GameObject popBanAux = Instantiate(prefabLoadingScreen);
        LoadingScreen loadScreen = popBanAux.GetComponent<LoadingScreen>();
        return loadScreen;
    }

    public void SetActiveWaitForLoad(bool isActive)
    {
        if (isActive)
        {
            loadingScreen = CreateLoadingScreen();
            loadingScreen.StartWaitForLoadScreen();
        }
        else
        {
            loadingScreen.StopWaitForLoadScreen();
        }

    }

}