using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioHelper : MonoBehaviour
{
    [SerializeField] private Toggle toggleSound;
    [SerializeField] private Toggle toggleMusic;
    private bool isInitMusic, isInitSound;

    private void Awake()
    {
        UpdateStatusMusic();
        UpdateStatusSound();
    }
    public void UpdateStatusMusic()
    {
        bool statusMusic = AudioManager.Instance.UpdateStatusMusic();
        if (toggleMusic.isOn == statusMusic)
        {
            isInitMusic = true;
            toggleMusic.isOn = !statusMusic;
        }
    }

    public void UpdateStatusSound()
    {
        bool statusSFX = AudioManager.Instance.UpdateStatusSFX();
        if (toggleSound.isOn == statusSFX)
        {
            isInitSound = true;
            toggleSound.isOn = !statusSFX;
        }
    }

    public void ToggleMusic()
    {
        if (isInitMusic)
        {
            isInitMusic = false;
        }
        else
        {
            AudioManager.Instance.ToggleMusic();
        }
    }

    public void ToggleSFX()
    {
        if (isInitSound)
        {
            isInitSound = false;
        }
        else
        {
            AudioManager.Instance.ToggleSFX();
        }
    }
}
