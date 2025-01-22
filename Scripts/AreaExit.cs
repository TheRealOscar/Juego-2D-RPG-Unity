using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    //private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>() && !SceneManagement.Instance.IsTransitioning) {
        SceneManagement.Instance.StartSceneTransition();
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        SceneManager.LoadScene(sceneToLoad);
        //UIFade.Instance.FadeToBlack();
    }
    }
    /*
    private IEnumerator LoadSceneRoutine() {
        while (waitToLoadTime >= 0) 
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }*/
}