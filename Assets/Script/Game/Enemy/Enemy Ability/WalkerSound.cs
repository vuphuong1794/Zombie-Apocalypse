using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerSound : MonoBehaviour
{
    public Sound[] sounds;
    public float TimeCount;
    private int indexSound;

    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            // Set volume and pitch separately to avoid assignment issues
            s.volume = 0.1f;
            s.source.volume = s.volume;

            s.pitch = 1f;
            s.source.pitch = s.pitch;
        }

        TimeCount = 0;
        indexSound = Random.Range(0, sounds.Length);

        // Play the initial sound
        sounds[indexSound].source.Play();
    }


    void Update()
    {
        // Only play a random sound when TimeCount exceeds the threshold
        if (TimeCount > 3)
        {
            int active_voice = Random.Range(0, 2);
            if (active_voice==1)
            {
                indexSound = Random.Range(0, sounds.Length);

                // Play the sound only if it's not already playing
                if (!sounds[indexSound].source.isPlaying)
                {
                    sounds[indexSound].source.Play();
                    Debug.Log("Playing sound: " + sounds[indexSound].clip.name);
                }
            }

            // Reset TimeCount after playing a sound
            TimeCount = 0;
        }
        else
        {
            TimeCount += Time.deltaTime;
        }
    }
}
