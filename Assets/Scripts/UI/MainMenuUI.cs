using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Image soundsDisabledSprite;
    GameManager gameManager;
    SoundManager soundManager;
    void Awake()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        if (soundManager != null) {
            soundsDisabledSprite.enabled = !SoundManager.Instance.soundOn;
        }
    }

    public void SetSinglePlayer(bool singlePlayer)
    {
        if (gameManager == null) gameManager = GameManager.Instance;
        gameManager.SetSinglePlayer(singlePlayer);
    }

    public void ToggleSound()
    {
        SoundManager.Instance.ToggleSound();
        soundsDisabledSprite.enabled = !SoundManager.Instance.soundOn;
    }
}
