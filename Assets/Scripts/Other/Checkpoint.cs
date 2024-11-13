//检查点

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    [FormerlySerializedAs("checkpointId")] public string id;
    //是否被激活
    [FormerlySerializedAs("activated")] public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("生成ID")]
    //生成id
    private void GenereateId()
    {
        id = System.Guid.NewGuid().ToString();
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
        activationStatus = true;
        anim.SetBool("active",true);
    }
}
