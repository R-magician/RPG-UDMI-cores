//游戏管理器

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour,ISaveManager
{
    //单例模式
    public static GameManager instance;

    private Transform player;
    //检查点列表
    [SerializeField] private Checkpoint[] checkpoints;
    //加载检查点
    [FormerlySerializedAs("loadedCheckpointId")] 
    [FormerlySerializedAs("closestCheckpointLoaded")] 
    [SerializeField] private string closestCheckpointId;

    [Header("丢失数量")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    //丢失数量
    public int lostCurrencyAmount;
    //丢失坐标
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    
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
        player = PlayerManager.instance.Player.transform;
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
        //将玩家放置在最接近的位置
        StartCoroutine(LoadWthDelay(_data));
    }

    private void LoadCheckpoints(GameData _data)
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
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if (FindClosestCheckpoint() != null)
        {
            _data.closestCheckpointId = FindClosestCheckpoint().id;
        }
        _data.checkpoints.Clear();
        
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id,checkpoint.activationStatus);
        }
    }

    //将玩家放置在最接近的位置  
    private void PlacePlayerAtClosestpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null)
        {
            return;
        }
        
        closestCheckpointId = _data.closestCheckpointId;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
            {
                player.transform.position=checkpoint.transform.position;
            }
        }
    }
    
    //找到最近的检查点
    private Checkpoint FindClosestCheckpoint()
    {
        float closestDizstance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.transform.position,
                checkpoint.transform.position);

            if (distanceToCheckpoint < closestDizstance && checkpoint.activationStatus == true)
            {
                closestDizstance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    //加载前一次死亡姿态
    private IEnumerator LoadWthDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f); 
        
        PlacePlayerAtClosestpoint(_data);
        LoadClosestCheckpoint(_data);
        //加载检查点
        LoadCheckpoints(_data);
    }
    
    private void LoadClosestCheckpoint(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;
        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY),Quaternion.identity);
            newLostCurrency.GetComponent<lostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }
}
