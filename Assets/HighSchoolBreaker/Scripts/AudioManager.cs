using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0.5f, 1.5f)]
        public float pitch = 1f;
    }

    [Header("SFX")]
    [SerializeField] private Sound[] sfxSounds;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySFX(string soundName)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == soundName);

        if (sound == null)
        {
            Debug.LogWarning("SFX not found: " + soundName);
            return;
        }

        sfxSource.pitch = sound.pitch;
        sfxSource.PlayOneShot(sound.clip, sound.volume);
    }
}