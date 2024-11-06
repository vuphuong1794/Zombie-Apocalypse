using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSound : MonoBehaviour
{
    // Start is called before the first frame update
    public Sound[] sounds;
    public float TimeCount;
    int indexSound;
    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume = 1;
            s.source.pitch = s.pitch;

        }
        indexSound = Random.Range(0, sounds.Length);
        sounds[indexSound].source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        sounds[1].source.Play();
        if (TimeCount > 3)
        {
            int active_voice = Random.Range(0, 3);
            if (active_voice == 2)
            {
                indexSound = Random.Range(0, sounds.Length);
                sounds[indexSound].source.Play();
            }
            TimeCount = 0;
        }
        else
        {
            TimeCount += Time.deltaTime;
        }

    }
}
