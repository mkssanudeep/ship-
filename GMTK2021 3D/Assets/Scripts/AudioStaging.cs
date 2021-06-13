using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioStaging : MonoBehaviour
{
    static public AudioStaging m_instance;
    public AudioMixer masterMixer;
    public float AudioChageSpeed = 2f;
    public int musicStage = 0;

    private void Awake()
    {
        m_instance = this;
    }

    public void Update()
    {
        musicStage = musicStage < 0 ? 0 : musicStage;
        musicStage = musicStage > 2 ? 2 : musicStage;
        switch (musicStage)
        {
            case 0:
                SetSFXLVL("bgm1Vol", -80);
                SetSFXLVL("bgm2Vol", -80);
                break;
            case 1:
                SetSFXLVL("bgm1Vol", 0);
                SetSFXLVL("bgm2Vol", -80);
                break;
            case 2:
                SetSFXLVL("bgm1Vol", -80);
                SetSFXLVL("bgm2Vol", 0);
                break;
        }
    }

    public void SetSFXLVL(string bgmName, float sfxLvl)
    {
        masterMixer.SetFloat(bgmName, sfxLvl);
    }
}
