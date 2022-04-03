using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;


public class AudioManager : Singleton<AudioManager>
{
    static List<AudioSource> currentlyPlayingSounds = new List<AudioSource>();
    Sound[] sounds;
    void Awake()
    {
        // Get all sounds from the Resources/Sounds folder
        sounds = Resources.LoadAll<Sound>("Sounds");
        //Debug.Log("Found " + sounds.Length + " sounds");
        foreach (Sound s in sounds)
        {
            s.source = MakeAudioSource(s).GetComponent<AudioSource>();
        }
    }
    GameObject MakeAudioSource(Sound sound, Vector3 position = default(Vector3))
    {
        GameObject go = new GameObject("AudioSource (" + sound.name + ")");
        go.transform.SetParent(transform);
        go.transform.position = position;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = sound.loop;
        source.mute = sound.mute;

        source.spatialBlend = sound.spatialize;
        source.minDistance = sound.distanceMin;
        source.maxDistance = sound.distanceMax;

        source.outputAudioMixerGroup = sound.group.group;
        return go;
    }

    IEnumerator RunAfterDone(Sound sound, Action action)
    {
        yield return new WaitForSeconds(sound.clip.length);
        action();
    }

    /* IEnumerator DestroyAfterPlay(GameObject go)
    {
        yield return new WaitForSeconds(go.GetComponent<AudioSource>().clip.length);
        Destroy(go);
    } */

    public AudioSource Play(Sound sound, GameObject playOn = null)
    {
        GameObject newGo = MakeAudioSource(sound, playOn == null ? Vector3.zero : playOn.transform.position);

        AudioSource source = newGo.GetComponent<AudioSource>();
        source.Play();

        if(!sound.loop)
            StartCoroutine(RunAfterDone(sound, () => Destroy(newGo)));

        currentlyPlayingSounds.Add(source);

        StartCoroutine(RunAfterDone(sound, () => currentlyPlayingSounds.Remove(source)));
        
        return source;
    }


    public AudioSource Play(string name, GameObject playOn = null)
    {
        Sound sound = FindSoundByName(name);
        if (sound == null)
        {
            return null;
        }
        return Play(sound, playOn);
    }
    
    private Sound FindSoundByName(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }

    public void StopAllSounds(bool remove = false)
    {
        foreach (AudioSource source in currentlyPlayingSounds)
        {
            source.Stop();
        }
        if (remove)
        {
            currentlyPlayingSounds.Clear();
        }
    }
}
