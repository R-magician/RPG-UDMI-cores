//存储管理

using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System.Linq;

[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    
    
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;
    
    [ContextMenu("删除保存的存储文件")]
    //删除保存的文件
    private void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        dataHandler.Delete();
    }
    
    private void Awake()
    {
        //只有一个实例，若有了销毁 
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        saveManagers = FindAllSaveManagers();
        //加载游戏
        LoadGame();
    }

    //新游戏
    public void NewGame()
    {
        gameData = new GameData();
    }

    //加载游戏
    public void LoadGame()
    {
        //数据加载
        gameData = dataHandler.Load();
        
        if (this.gameData == null)
        {
            NewGame();
        }
        
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }
    
    //保存数据
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        //保存数据
        dataHandler.Save(gameData);
    }

    //当应用程序退出时调用
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //找到所有的存储管理
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}
