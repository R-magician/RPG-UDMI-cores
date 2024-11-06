//自定义序列化字典数据

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey,TValue>:Dictionary<TKey,TValue>,ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    
    public void OnBeforeSerialize()
    {
        //在序列化前执行
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        //在反序列化后执行
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.Log("key 的数量与 value的数量不相等");
        }
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i],values[i]);
        }
    }
}
