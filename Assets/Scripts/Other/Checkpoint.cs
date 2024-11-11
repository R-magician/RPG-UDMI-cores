//检查点

using System;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string checkpointId;
    //是否被激活
    public bool activated;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("生成ID")]
    //生成id
    private void GenereateId()
    {
        checkpointId = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            ActiveteCheckpoint();
        }
    }

    //检查点激活
    public void ActiveteCheckpoint()
    {
        anim.SetBool("active",true);
    }
}
