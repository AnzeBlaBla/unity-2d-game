using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : Singleton<SettingsManager>
{
    public GameObject container;
    public float volume;
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    void Awake()
    {
        volume = LoadVolume();
        volumeSlider.value = volume;

        Hide();
    }

    void Start()
    {
        audioMixer.SetFloat("MasterVolume", SoundToAudioManagerRange(volume));
    }

    /* public void Show()
    {
        container.SetActive(true);
        GameManager.Instance.PauseGame();
    }
 */
    public void Hide()
    {
        container.SetActive(false);
        GameObjectRegistry.Instance.player.GetComponent<PlayerMovement>().enabled = true;
        //GameObjectRegistry.Instance.player.GetComponent<PlayerLook>().enabled = true;
        GameObjectRegistry.Instance.player.GetComponent<PlayerShooting>().enabled = true;
    }

    public void OnVolumeChanged()
    {
        volume = volumeSlider.value;
        SaveVolume();
        //Debug.Log("Settzings master volume to " + SoundToAudioManagerRange(volume));
        audioMixer.SetFloat("MasterVolume", SoundToAudioManagerRange(volume));
    }

    void SaveVolume()
    {
        //Debug.Log("Saving volume: " + volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    float LoadVolume()
    {
        float loadedVolume = PlayerPrefs.GetFloat("volume", 1);
        //Debug.Log("Loaded volume: " + loadedVolume);
        return loadedVolume;
    }

    float SoundToAudioManagerRange(float value)
    {
        return value - 80f > 0 ? 0 : value - 80f;
    }


}
