using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int sceneIndex;

    public Vector2 playerDestination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //DataInstance.Instance.SetPlayerPosition(playerDestination, sceneIndex);
            DataInstance.Instance.SetPlayerPosition(playerDestination);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}