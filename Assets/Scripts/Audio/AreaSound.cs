//区域音效

using System;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            AudioManager.instance.PlaySFX(areaSoundIndex,null);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            AudioManager.instance.StopSFXWithTime(areaSoundIndex);
        }
    }
}
