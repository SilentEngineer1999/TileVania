using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    float levelLoadDelay = 3f;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
       yield return new WaitForSecondsRealtime(levelLoadDelay);
       int currentScreenIndex = SceneManager.GetActiveScene().buildIndex;
       int nextSceneIndex = currentScreenIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings){
                nextSceneIndex = 0;
        }
        
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }

   
}
