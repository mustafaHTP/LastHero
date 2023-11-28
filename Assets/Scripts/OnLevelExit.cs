using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnLevelExit : MonoBehaviour
{
    [SerializeField] private float sceneLoadingDelay;
    [SerializeField] private Canvas sceneTransition;

    private void Awake()
    {
        sceneTransition.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {

        int levelCount = SceneManager.sceneCountInBuildSettings;
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        //Check it is the last scene
        if (nextLevelIndex == levelCount)
        {
            yield return null;
        }

        Animator sceneTransitionAnimator = sceneTransition.GetComponent<Animator>();
        sceneTransitionAnimator.SetTrigger("LevelStart");

        yield return new WaitForSeconds(sceneLoadingDelay);

        SceneManager.LoadScene(nextLevelIndex);
        
    }
}
