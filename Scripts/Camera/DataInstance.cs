using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataInstance : MonoBehaviour
{
    private static DataInstance instance;

    public int slotIndex;

    public Vector2 playerPosition;

    public int currentHearts;
    public int hp;
    public int currentKeys;
    /*
    public int sceneIndex;

    public int currentKeys;
    public int coins;
    public bool unlockBow;
    public bool unlockBomb;
    public bool unlockWand;
    public int selectedWeapon;
    public int[] selectedWeaponAmmo;

    public GameData gameData;
*/
    static string SaveDataKey = "SaveDataKey";
    SaveData saveData;
    SceneData sceneData;

    public static DataInstance Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("DataInstance");
                instance = go.AddComponent<DataInstance>();
                DontDestroyOnLoad(go);
                instance.playerPosition = FindFirstObjectByType<PlayerController>().transform.position;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //DeleteSaveData(); //para borrarar los datos guardados
    }
    /*
    public void LoadData()
    {
        Delete();
        if(!PlayerPrefs.HasKey(SaveDataKey)) CreateGameData();

        string json = PlayerPrefs.GetString(SaveDataKey);
        gameData = JsonUtility.FromJson<GameData>(json);
    }

    public void SetSlotData(int index)
    {
        slotIndex = index;
        sceneIndex = gameData.saveData[index].sceneIndex;
    }
    */
    public void SetPlayerPosition(Vector2 playerPos)
    {
        playerPosition = playerPos;

        GameManager gm = FindAnyObjectByType<GameManager>();

        currentHearts = gm.currentHearts;
        hp = gm.hp;
        currentKeys = gm.currentKeys;
        //this.sceneIndex = sceneIndex;

        SavePlayerData();
    }



    /*
    public void UnlockBomb()
    {
        if (!unlockBomb)
        {
            unlockBomb = true;

            SavePlayerData();
        }
    }

    public void UnlockBow()
    {
        unlockBow = true;

        SavePlayerData();
    }

    public void UnlockWand()
    {
        unlockWand = true;

        SavePlayerData();
    }
    */
    private void SavePlayerData()
    {
        saveData.currentHearts = currentHearts;
        saveData.hp = hp;
        saveData.currentKeys = currentKeys;

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveDataKey, json);
        PlayerPrefs.Save();
    }
    
    public void SaveSceneData(string name)
    {
        if(sceneData == null || sceneData.sceneName != SceneManager.GetActiveScene().name)
        {
            sceneData = new SceneData();
            sceneData.sceneName = SceneManager.GetActiveScene().name;
            sceneData.objectsName = new List<string>();
        }

        if (saveData.sceneData.Contains(sceneData)) saveData.sceneData.Remove(sceneData);

        sceneData.objectsName.Add(name);

        saveData.sceneData.Add(sceneData);

        SavePlayerData();
    }
    
    public void LoadData()
    {
        if (!PlayerPrefs.HasKey(SaveDataKey)) CreateSaveData();

        string json = PlayerPrefs.GetString(SaveDataKey);
        saveData = JsonUtility.FromJson<SaveData>(json);

        currentHearts = saveData.currentHearts;
        currentKeys = saveData.currentKeys;
        hp = saveData.hp;

        foreach (SceneData sceneData in saveData.sceneData)
        {
            if (sceneData.sceneName == SceneManager.GetActiveScene().name)
            {
                this.sceneData = sceneData;

                foreach (string name in sceneData.objectsName)
                {
                    GameObject gameObject = GameObject.Find(name);

                    if(gameObject != null)
                    {
                        if(gameObject.GetComponent<Chest>()) gameObject.GetComponent<Chest>().OpenedChest();
                        else gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    /*
    private void CreateGameData()
    {
        GameData gameData = new GameData();
        gameData.saveData = new List<SaveData>();
        gameData.saveData.Add(CreateSaveData(0));
        gameData.saveData.Add(CreateSaveData(1));
        gameData.saveData.Add(CreateSaveData(2));

        gameData.musicVolume = 1;
        gameData.sfxVolume = 1;

        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(SaveDataKey, json);
        PlayerPrefs.Save();
    }*/

    private void CreateSaveData()
    {
        SaveData saveData = new SaveData();
        //saveData.saveDataSlot = index;
        //saveData.playerPosition = new Vector2(0, -1);
        //saveData.sceneIndex = 1;
        saveData.currentHearts = 3;
        saveData.hp = 12;
        saveData.currentKeys = 0;
        saveData.sceneData = new List<SceneData>();
        //saveData.coins = 0;

        String json = JsonUtility.ToJson(saveData); 
        PlayerPrefs.SetString(SaveDataKey, json);
        PlayerPrefs.Save();
        //saveData.unlockBow = false;

        //return saveData;
    }

    public void DeleteSaveData()
    {
        if(PlayerPrefs.HasKey(SaveDataKey)) PlayerPrefs.DeleteKey(SaveDataKey);
        //gameData.saveData[index] = CreateSaveData(index);
    }
    
}
/*
[System.Serializable]
public class GameData
{
    public List<SaveData> saveData;
    public float musicVolume;
    public float sfxVolume;
}
*/

[System.Serializable]
public class SaveData
{
    public int saveDataSlot;

    public Vector2 playerPosition;
    public int sceneIndex;

    public int currentHearts;
    public int hp;
    public int currentKeys;
    /*public int coins;

    public bool unlockBow;
    public bool unlockBomb;
    public bool unlockWand;

    public int selectedWeapon;
    public int[] selectedWeaponAmmo;*/
    public List<SceneData> sceneData;
}

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public List<string> objectsName;
}
