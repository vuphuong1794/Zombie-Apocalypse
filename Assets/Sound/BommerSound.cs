using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class BommerSound : MonoBehaviour
{
    // Start is called before the first frame update
    public Sound[] sounds;
    public float TimeCount;
    private int indexSound;
    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume = 1;
            s.source.pitch = s.pitch;

        }
        TimeCount = 0;
        indexSound = Random.Range(0, sounds.Length);
        sounds[indexSound].source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Only play a random sound when TimeCount exceeds the threshold
        if (TimeCount > 3)
        {
            int active_voice = Random.Range(0, 2);
            if (active_voice == 1)
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
