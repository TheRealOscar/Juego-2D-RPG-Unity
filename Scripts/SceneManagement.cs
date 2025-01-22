using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }
    public bool IsTransitioning { get; private set; }

    public void SetTransitionName(string sceneTransitionName) {
        this.SceneTransitionName = sceneTransitionName;
    }

    public void StartSceneTransition() {
        IsTransitioning = true;
        StartCoroutine(EndTransitionCooldown());
    }

    private IEnumerator EndTransitionCooldown() {
        yield return new WaitForSeconds(1f); // Ajusta el tiempo de cooldown según sea necesario
        IsTransitioning = false;
    }
}

