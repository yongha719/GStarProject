using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public enum SoundType
{
    SFX,
    BGM,
    CAT,
    END
}

public class SoundManager : Singleton<SoundManager>
{
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    public Dictionary<SoundType, AudioSource> audioSources = new Dictionary<SoundType, AudioSource>();
    public float[] audioVolumes = new float[(int)SoundType.END];

    public Slider audioSlider;
    public Slider sfxSlider;
    public Slider catSlider;

    private Image mySelfImage;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (AudioClip clip in clips)
        {
            audioClips[clip.name] = clip;
        }

        string[] enumNames = System.Enum.GetNames(typeof(SoundType));
        for (int i = 0; i < (int)SoundType.END; i++)
        {
            GameObject AudioSourceObj = new GameObject(enumNames[i]);
            AudioSourceObj.transform.SetParent(transform);
            audioSources[(SoundType)i] = AudioSourceObj.AddComponent<AudioSource>();
            audioVolumes[i] = 0.5f;
        }

        audioSources[SoundType.BGM].loop = true;
    }
    private void Start()
    {
        PlaySoundClip("BGM_01_Fat_Cat", SoundType.BGM);
        mySelfImage = GetComponent<Image>();

        foreach (Button btn in Resources.FindObjectsOfTypeAll<Button>())
        {
            btn.onClick.AddListener(() => UIClickSound());
        }

        sliderValueaApply();
    }
    public AudioClip PlaySoundClip(string clipName, SoundType type, float volume = 0.5f, float pitch = 1)
    {
        AudioClip clip = audioClips[clipName];
        return PlaySoundClip(clip, type, volume, pitch);
    }
    public AudioClip PlaySoundClip(AudioClip clip, SoundType type, float volume = 0.5f, float pitch = 1)
    {
        audioSources[type].pitch = pitch;

        float curVolume = volume * audioVolumes[(int)type];
        if (type == SoundType.BGM)
        {
            audioSources[SoundType.BGM].clip = clip;
            audioSources[SoundType.BGM].volume = curVolume;
            audioSources[SoundType.BGM].Play();
        }
        else
        {
            audioSources[type].PlayOneShot(clip, curVolume);
        }

        return clip;
    }
    private void sliderValueaApply()
    {
        audioSlider.value = audioVolumes[0];
        sfxSlider.value = audioVolumes[1];
        catSlider.value = audioVolumes[2];
    }
    public void AudioSoundSetting(float index)
    {
        audioVolumes[(int)SoundType.BGM] = index;
        audioSources[SoundType.BGM].volume = index;
    }
    public void SFXSoundSetting(float index)
    {
        audioVolumes[(int)SoundType.SFX] = index;
    }
    public void CatSoundSetting(float index)
    {
        audioVolumes[(int)SoundType.CAT] = index;
    }
    public void ScreenOn(bool onOff)
    {
        if (onOff)
        {
            mySelfImage.DOFade(0.5f, 0);
            transform.GetChild(0).transform.DOScale(1, 0.3f);
        }
        else
        {
            transform.GetChild(0).transform.DOScale(onOff ? 1 : 0, 0.3f).OnComplete(() => mySelfImage.DOFade(0f, 0));
        }
    }
    public void UIClickSound() => PlaySoundClip("SFX_Button_Touch", SoundType.SFX);
}
