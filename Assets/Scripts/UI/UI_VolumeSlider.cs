//控制UI音量
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    
    //参数
    public string parametr;
    
    //声音混合器
    [SerializeField] private AudioMixer audioMixer;
    //乘数
    [SerializeField] private float multiplier;

    //控制滑块值
    public void SliderValue(float _value)
    {
        audioMixer.SetFloat(parametr, Mathf.Log10(_value) * multiplier);
    }
}
