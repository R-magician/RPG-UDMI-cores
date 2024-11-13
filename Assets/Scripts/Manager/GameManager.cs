//游戏管理器

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour,ISaveManager
{
    //单例模式
    public static GameManager instance;
    //检查点列表
    [SerializeField] private Checkpoint[] checkpoints;
    //加载检查点
    [FormerlySerializedAs("loadedCheckpointId")] [FormerlySerializedAs("closestCheckpointLoaded")] [SerializeField] private string closestCheckpointId;
    
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
        //返回有Checkpoint组件的所有对象
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    //重新开始游戏
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        //获取当前激活的场景
        Scene scene = SceneManager.GetActiveScene();
        //加载当前场景
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string,bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActiveteCheckpoint();
                }
            }
        }
        
        closestCheckpointId = _data.closestCheckpointId;
        //将玩家放置在最接近的位置
        
        Invoke("PlacePlayerAtClosestpoint", 0.1f);
        PlacePlayerAtClosestpoint();
    }

    //将玩家放置在最接近的位置  
    private void PlacePlayerAtClosestpoint()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
            {
                PlayerManager.instance.Player.transform.position=checkpoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.closestCheckpointId = FindClosestCheckpoint().id;
        _data.checkpoints.Clear();
        
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id,checkpoint.activationStatus);
        }
    }

    //找到最近的检查点
    private Checkpoint FindClosestCheckpoint()
    {
        float closestDizstance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.Player.transform.position,
                checkpoint.transform.position);

            if (distanceToCheckpoint < closestDizstance && checkpoint.activationStatus == true)
            {
                closestDizstance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
}
