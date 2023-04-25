using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPlay="FirstPlay";
    private static readonly string BackgroundPref="BackgroundPref";
    private static readonly string SoundEffectsPref="SoundEffectsPref";

    private int FirstPlayInt;
    public Slider backgroundSlider, soundEffectsSlider;
    private float backgroundFloat,soundEffectsFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    void Start()
    {
        FirstPlayInt=PlayerPrefs.GetInt(FirstPlay);

        if(FirstPlayInt==0)
        {
            backgroundFloat = .125f;
            soundEffectsFloat = .75f;
            if (backgroundSlider != null && soundEffectsSlider != null)
            {
                backgroundSlider.value = backgroundFloat;
                soundEffectsSlider.value = soundEffectsFloat;
            }
            PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay,-1);
        }

        else if (backgroundSlider != null && soundEffectsSlider != null)
        {
            
           backgroundFloat= PlayerPrefs.GetFloat(BackgroundPref);
           backgroundSlider.value=backgroundFloat;
           soundEffectsFloat=PlayerPrefs.GetFloat(SoundEffectsPref);
           soundEffectsSlider.value=soundEffectsFloat;

        }
    }

    public void SaveSoundSettings()
    {
        if (backgroundSlider != null && soundEffectsSlider != null)
        {
            PlayerPrefs.SetFloat(BackgroundPref, backgroundSlider.value);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
        }
    }

    void OnApplicationFocus(bool inFocus)
    {
        if(!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        backgroundAudio.volume=backgroundSlider.value;

        for(int i=0;i<soundEffectsAudio.Length;++i)
        {
            soundEffectsAudio[i].volume=soundEffectsSlider.value;
        }
    }

    
}
