//场景管理
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    //单例模式
    public static ScenesManager instance;

    public TMP_FontAsset fontAsset;

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
    
    
    private void OnValidate()
    {
        //设置当前场景所有Game Object字体样式
        // SetSceneFontAsset();
    }

    //设置当前场景所有Game Object字体样式
    private void SetSceneFontAsset()
    {
        if (fontAsset == null)
        {
            return;
        }
        // 获取当前活动场景
        Scene currentScene = SceneManager.GetActiveScene();
        
        // 获取场景中的所有根对象
        GameObject[] rootObjects = currentScene.GetRootGameObjects();
        
        // 存储所有的GameObject
        List<GameObject> allGameObjects = new List<GameObject>();

        // 遍历每个根对象
        foreach (GameObject rootObject in rootObjects)
        {
            // 将根对象添加到列表中
            allGameObjects.Add(rootObject);
            
            // 递归获取所有子对象
            GetChildren(rootObject, allGameObjects);
        }

        // 输出所有GameObject的名字
        foreach (GameObject go in allGameObjects)
        {
            
            TextMeshProUGUI a = go.GetComponent<TextMeshProUGUI>();
            if (a)
            {
                a.font = fontAsset;
            }
        }
    }

    // 递归获取子对象
    void GetChildren(GameObject obj, List<GameObject> allGameObjects)
    {
        foreach (Transform child in obj.transform)
        {
            // 将子对象添加到列表中
            allGameObjects.Add(child.gameObject);
            
            // 递归获取子对象的子对象
            GetChildren(child.gameObject, allGameObjects);
        }
    }
}
