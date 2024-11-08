//UI主菜单管理

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    //场景名称
    [SerializeField] private string sceneName = "MainScene";
    //游戏继续按钮
    [SerializeField] private GameObject continueButton;

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
        SceneManager.LoadScene(sceneName);
    }

    //新游戏
    public void NewGame()
    {
        //删除存储
        SaveManager.instance.DeleteSaveData();
        //加载场景
        SceneManager.LoadScene(sceneName);
    }
    
    //退出游戏
    public void ExitGame()
    {
        Application.Quit();
    }
}
