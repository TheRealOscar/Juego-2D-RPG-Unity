using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;

    public float yDistance = 9f;
    public float yMovement = 18f;

    public float xDistance = 16f;
    public float xMovement = 32f;

    public Vector3 cameraDestination;

    public float movementTime = 0.5f;
    public bool isMoving;

    EntitySceneControl entitySceneControl;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        SetCameraFirstPosition();
        UpdateEntitySceneControl();
    }
  
    public void UpdateEntitySceneControl()
    {
        entitySceneControl = FindFirstObjectByType<EntitySceneControl>();
        entitySceneControl.ActiveAllEntitiesScene(transform.position);
    }

    void Update()
    {
        if (!isMoving)
        {
            if (player.position.y - transform.position.y >= yDistance)
            {
                cameraDestination += new Vector3(0, yMovement, 0);
                StartCoroutine(MoveCamera());
            }
            else if (transform.position.y - player.position.y >= yDistance)
            {
                cameraDestination -= new Vector3(0, yMovement, 0);
                StartCoroutine(MoveCamera());
            }
            else if (player.position.x - transform.position.x >= xDistance)
            {
                cameraDestination += new Vector3(xMovement, 0, 0);
                StartCoroutine(MoveCamera());
            }
            else if (transform.position.x - player.position.x >= xDistance)
            {
                cameraDestination -= new Vector3(xMovement, 0, 0);
                StartCoroutine(MoveCamera());
            }
        }
    }

    private void SetCameraFirstPosition()
    {
        float x = Mathf.Round(player.position.x / xMovement) * xMovement;
        float y = Mathf.Round(player.position.y / yMovement) * yMovement;

        transform.position = new Vector3(x, y, transform.position.z);
        cameraDestination = transform.position;
    }
    
    IEnumerator MoveCamera()
    {
        isMoving = true;
        var currentPos = transform.position;
        var t = 0f;

        entitySceneControl.StopAllEntitiesScene(currentPos);

        while (t < 1)
        {
            t += Time.deltaTime / movementTime;
            transform.position = Vector3.Lerp(currentPos, cameraDestination, t);
            transform.position = new Vector3(transform.position.x, transform.position.y, currentPos.z);
            yield return null;
        }
        isMoving = false;

        entitySceneControl.ResetPositionEntitiesScene(currentPos);
        entitySceneControl.ActiveAllEntitiesScene(cameraDestination);
    }

    public void PauseEnemies()
    {
        entitySceneControl.StopAllEntitiesScene(transform.position);
    }

    public void ResumeEnemies()
    {
        entitySceneControl.ActiveAllEntitiesScene(transform.position);
    }
}