//黑洞技能控制

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackholeSkillController : MonoBehaviour
{
    //热键预制体
    [SerializeField] private GameObject hotKeyPrefab;
    //按键code
    [SerializeField] private List<KeyCode> keyCodes = new List<KeyCode>();
    
    //最大尺寸
    private float maxSize;
    //增长速度
    private float growSpeed;
    //缩小速度
    private float shinkSpeed;
    //持续时间
    private float blackholeTimer;
    
    //是否可以增长
    private bool canGrow = true;
    //可以缩小
    private bool canShrink;
    //是否可以创建热键
    private bool canCreateHotKey = true;
    //是否可以攻击
    private bool cloneAttackReleas;
    //玩家可以消失
    private bool playerCanDisapear = true;
    
    //攻击量
    private int amountOfAttacks = 4;
    //克隆攻击冷却
    private float cloneAttackCooldown = .3f;
    //克隆冷却计算器
    private float cloneAttackTimer;
    
    //触发器范围内有的敌人
    private List<Transform> targets = new List<Transform>();
    //创建的热键等于一个新的游戏对象列表
    private List<GameObject> createdHotkey = new List<GameObject>();

    //玩家退出状态
    public bool playerCanExitState{get; private set;}

    //设置黑洞
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeTimer)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shinkSpeed = _shinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeTimer;
        if (SkillManager.instance.clone.crystalInseadOfClone)
        {
            //玩家不能消失
            playerCanDisapear = false;
        }
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (targets.Count > 0)
            {
                //释放攻击
                ReleasCloneAttack();
            }
            else
            {
                //结束黑洞技能
                FinishBlackHoleAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleasCloneAttack();
        }
        
        CloneAttackLogic();
        
        //黑洞扩张
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime); 
        }

        //黑洞缩减
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    //释放克隆攻击
    private void ReleasCloneAttack()
    {
        if (targets.Count <= 0)
        {
            return;
        }
        
        //删除热键对象
        DestroyHotKeys();
        cloneAttackReleas = true;
        canCreateHotKey = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            //玩家设置透明
            PlayerManager.instance.Player.fx.MakeTransparent(true);
        }
        
    }

    //克隆攻击逻辑
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleas && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count-1);

            float xOffset;
            if (Random.Range(0, 100) > 50)
            {
                xOffset = 1.5f;
            }
            else
            {
                xOffset = -1.5f;
            }

            //水晶代替克隆
            if (SkillManager.instance.clone.crystalInseadOfClone)
            {
                //创建水晶
                SkillManager.instance.crystal.CreateCrystal();
                //选择最近的敌人
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                //创建克隆体，发动攻击
                SkillManager.instance.clone.CreateClone(targets[randomIndex],new Vector3(xOffset,-1.1f,0));
            }
            amountOfAttacks--;

            //攻击次数小于0
            if (amountOfAttacks <= 0)
            {
                //结束黑洞技能--延迟0.5s
                Invoke("FinishBlackHoleAbility",1f);
            }
        }
    }

    //结束黑洞技能
    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        //玩家已经退出
        playerCanExitState = true;
        //技能攻击结束后，开始缩小
        canShrink = true;
        cloneAttackReleas = false;
        //玩家设置透明
        PlayerManager.instance.Player.fx.MakeTransparent(false);
    }

    private void DestroyHotKeys()
    {
        if (createdHotkey.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            //敌人冻结时间
            other.GetComponent<Enemy>()?.FreezeTime(true);

            CreateHotKey(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            //冻结关闭
            other.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    //创建热键
    private void CreateHotKey(Collider2D other)
    {
        if (keyCodes.Count <= 0)
        {
            return;
        }
        
        //不能创建热键
        if (!canCreateHotKey )
        {
            return;
        }
        //添加按键提示预制体
        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0,2 ), Quaternion.identity);
        createdHotkey.Add(newHotKey);
        //获得随机按键
        KeyCode choosenKey = keyCodes[Random.Range(0, keyCodes.Count)];
        //移除选项
        keyCodes.Remove(choosenKey);
            
        //提示添加到敌人头上
        BlackHoleHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKeyController>();
        //设置按下键
        newHotKeyScript.SetupHotKey(choosenKey,other.transform,this);
    }

    //将敌人坐标添加到targets集合
    public void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform);
}
