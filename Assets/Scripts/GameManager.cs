using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Session Data")]
    [Space(5)]
    [SerializeField] private PlayerSessionInfoSO playerSessionInfo;

    [Header("Game Session Settings")]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private float sceneLoadDelay;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        gameOverCanvas.gameObject.SetActive(false);

#if UNITY_EDITOR
        ResetGameSession();
#endif
    }

    public void DisplayGameOverCanvas()
    {
        gameOverCanvas.gameObject.SetActive(true);
    }

    public void ResetGameSession()
    {
        playerSessionInfo.Health = PlayerSessionInfoSO.DefaultHealth;
        playerSessionInfo.CoinScore = PlayerSessionInfoSO.DefaultCoinScore;
    }

    public void RestartGame()
    {
        gameOverCanvas.gameObject.SetActive(false);
        ResetGameSession();
        StartCoroutine(LoadFirstScene());
    }

    private IEnumerator LoadFirstScene()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(0);
    }
}
