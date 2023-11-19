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
        float gameOverCanvasPopupDuration = GetGameOverCanvasPopUpDuration();
        StartCoroutine(ProcessGameOverCanvas(gameOverCanvasPopupDuration));
    }

    private IEnumerator ProcessGameOverCanvas(float popupTime)
    {
        gameOverCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(popupTime);
        Time.timeScale = 0;
    }

    public void ResetGameSession()
    {
        playerSessionInfo.Health = PlayerSessionInfoSO.DefaultHealth;
        playerSessionInfo.CoinScore = PlayerSessionInfoSO.DefaultCoinScore;
    }

    public void RestartGame()
    {
        gameOverCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        ResetGameSession();
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(0);
    }

    private float GetGameOverCanvasPopUpDuration()
    {
        Animator gameOverCanvasAnimator = gameOverCanvas.GetComponent<Animator>();
        AnimationClip[] clips = gameOverCanvasAnimator.runtimeAnimatorController.animationClips;

        float animationLength = 0f;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "GameOver")
            {
                animationLength = clip.length;
                break;
            }
        }

        return animationLength;
    }
}
