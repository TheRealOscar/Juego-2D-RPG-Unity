using UnityEngine;

public class Chest : BasicInteraction
{
    GameManager gameManager;
    private bool opened;
    public bool locked;
    public GameObject item;
    public Sprite openSprite;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public override bool Interact(Vector2 playerFacing, Vector2 playerPos)
    {
        if (opened) return false;

        bool success = FacingObject(playerFacing);

        if (success) return OpenChest(playerPos);

        return success;
    }

    private bool OpenChest(Vector2 playerPos)
    {
        if (locked)
        {
            if (gameManager.currentKeys > 0)
            {
                gameManager.UpdateCurrentKeys(-1);
                return GetItem(playerPos);
            }
        }
        else
        {
            return GetItem(playerPos);
        }

        return false;
    }

    private bool GetItem(Vector2 playerPos)
    {
        Instantiate(item, playerPos, Quaternion.identity);
        GetComponent<SpriteRenderer>().sprite = openSprite;
        opened = true;
        DataInstance.Instance.SaveSceneData(name);
        return true;
    }

     public void OpenedChest()
    {
        GetComponent<SpriteRenderer>().sprite = openSprite;
        opened = true;
    }
}
