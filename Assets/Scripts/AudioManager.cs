using System;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Member;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] music;
    public Sound[] sfxSounds;
    public AudioSource musicSource;
    public AudioSource[] sfxSources;
    int sourceIndex = 0;
    public class SoundInstance { public Sound sound; public Vector3 location;  }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlaySFX(string name, Vector2 position, float volume = 1f, float time = 0, bool loop = false)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            var source = sfxSources[sourceIndex];

            source.time = time;
            source.transform.position = position;
            source.PlayOneShot(s.clip, volume);
            source.loop = loop;
               
            if (sourceIndex + 1 >= sfxSources.Length)
            {
                sourceIndex = 0;
            }
            else
            {
                sourceIndex++;
            }
        }
    }

    public void PlayMusic(string name, float volume = .5f, bool loop = false)
    {
        Sound s = Array.Find(music, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.PlayOneShot(s.clip, volume);
            musicSource.loop = loop;
        }
    }
}
