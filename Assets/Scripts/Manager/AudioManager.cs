//音频管理器

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    //单例模式
    public static AudioManager instance;

    //可以播放声音的最小距离   
    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    //是否需要播放bgm
    public bool playBgm;
    //播放第几段bgm
    public int bgmIndex;

    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                bgm[bgmIndex].Play();
            }
        }
    }

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

    //开始特效声音
    public void PlaySFX(int _sfxIndex,Transform _source) 
    {
        if (sfx[_sfxIndex].isPlaying)
        {
            return;
        }

        if (_source != null  && Vector2.Distance(PlayerManager.instance.Player.transform.position,_source.position) > sfxMinimumDistance)
        {
            return;
        }
        
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    //随机播放BGM
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    //停止特效声 
    public void StopSFX(int _sfxIndex)
    {
        sfx[_sfxIndex].Stop();
    }

    //开始BGM
    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGM();
        
        bgm[bgmIndex].Stop();
    }
    
    //停止所有的BGM
    public void StopAllBGM()
    {
        for (int i = 0; i<bgm.Length ; i++)
        {
            bgm[i].Stop();
        }
    }
    
}
