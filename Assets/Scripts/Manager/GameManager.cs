//游戏管理器
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //单例模式
    public static GameManager instance;
    
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
    
    //重新开始游戏
    public void RestartScene()
    {
        //获取当前激活的场景
        Scene scene = SceneManager.GetActiveScene();
        //加载当前场景
        SceneManager.LoadScene(scene.name);
    }
}
