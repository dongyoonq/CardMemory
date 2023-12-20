using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static StageManager stageManager;
    private static DataManager dataManager;
    private static UIManager uiManager;
    private static SoundManager soundManager;

    public static GameManager Instance { get { return instance; } }
    public static StageManager Stage { get { return stageManager; } set { stageManager = value; } }
    public static DataManager Data { get { return dataManager; } }
    public static UIManager UI { get { return uiManager; } }
    public static SoundManager Sound { get {  return soundManager; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject dataObj = new GameObject();
        dataObj.name = "DataManager";
        dataObj.transform.parent = transform;
        dataManager = dataObj.AddComponent<DataManager>();

        GameObject uiObj = new GameObject();
        uiObj.name = "UIManager";
        uiObj.transform.parent = transform;
        uiManager = dataObj.AddComponent<UIManager>();

        GameObject soundObj = new GameObject();
        soundObj.name = "SoundManager";
        soundObj.transform.parent = transform;
        soundManager = soundObj.AddComponent<SoundManager>();
    }
}