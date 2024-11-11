//UI主菜单管理

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    //场景名称
    [SerializeField] private string sceneName = "MainScene";
    //游戏继续按钮
    [SerializeField] private GameObject continueButton;
    //屏幕效果
    [SerializeField] private UI_FadeScreen fadeScreen;
    

    private void Start()
    {
        if (SaveManager.instance.HasSavedData()==false)
        {
            //无文件存储
            continueButton.SetActive(false);
        }
    }

    //继续游戏
    public void ContinueGame()
    {
        //加载场景
        StartCoroutine(LoadSceneWithFadeEffect(1f));
    }

    //新游戏
    public void NewGame()
    {
        //删除存储
        SaveManager.instance.DeleteSaveData();
        //加载场景
        StartCoroutine(LoadSceneWithFadeEffect(1f));
    }
    
    //退出游戏
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        //加载新场景
        SceneManager.LoadScene(sceneName);
    }
}
