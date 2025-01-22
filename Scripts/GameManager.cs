using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image[] playerHearts;
    public Sprite[] heartStatus;
    public int currentHearts;
    public int hp;

    static int minHearts = 3;
    static int maxHearts = 12;

    public Image[] playerKeys;
    public int currentKeys;

    private void Awake()
    {
        DataInstance.Instance.LoadData();
        currentHearts = DataInstance.Instance.currentHearts;
        hp = DataInstance.Instance.hp;
        currentKeys = DataInstance.Instance.currentKeys;
    }

    void Start ()
    {
        currentHearts = Mathf.Clamp(currentHearts, minHearts, maxHearts);
        hp = Mathf.Clamp(hp, 12, currentHearts * 4);
        UpdateCurrentHearts();
        UpdateCurrentKeys(0);
    }

    public bool CanHeal(){
        return hp < currentHearts * 4;
    }

    public void IncreaseMaxHP(){
        currentHearts++;
        currentHearts = Mathf.Clamp(currentHearts, minHearts, maxHearts);
        hp = currentHearts * 4;
        UpdateCurrentHearts();
    }

    public void UpdateCurrentHP(int x)
    {
        hp += x;
        hp = Mathf.Clamp(hp, 0, currentHearts * 4);
        UpdateCurrentHearts();
    }

    private void UpdateCurrentHearts()
    {
        int aux = hp;
        for (int i = 0; i < maxHearts; i++)
        {
            if (i < currentHearts)
            {
                playerHearts[i].enabled = true;
                playerHearts[i].sprite = GetHearStatus(aux);
                aux -= 4;
            }
            else{
                playerHearts[i].enabled = false;
            }
        }
    }

    private Sprite GetHearStatus(int x)
    {
        switch (x)
        {
            case >= 4: return heartStatus[4];
            case 3: return heartStatus[3];
            case 2: return heartStatus[2];
            case 1: return heartStatus[1];
            default: return heartStatus[0];
        }
    }

    public void UpdateCurrentKeys(int x)
    {
        currentKeys = Mathf.Clamp(currentKeys += x, 0, 99);

        for(int i = 0; i < playerKeys.Length; i++)
        {
            playerKeys[i].enabled = currentKeys > i;
        }
    }
}
