using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        sliderValueaApply();
    }
    public AudioClip PlaySoundClip(string clipName, SoundType type, float volume = 0.5f, float pitch = 1)
    {
        AudioClip clip = audioClips[clipName];
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

}
