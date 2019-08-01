using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A struct that contains information about audio.
/// Used for segmenting clips from a large audio clip
/// </summary>
[System.Serializable]
public class AudioObject {
    public string name;
    public AudioClip clip;
    public float startTime;
    public float endTime;
    public bool playWholeClip;
}

public class AudioHandler : BaseObject
{
    //List of all clips in game
    public List<AudioObject> clips; 
    //List of animal clips with segments
    public List<AudioClip> animalClips;
    private List<AudioSource> audioSources;
    private float masterVolume = 1f;
    public float GetVolume() { return masterVolume; }

    public bool mute;
    public override void ObjectInitialize(GameManager gameManager)
    {
        base.ObjectInitialize(gameManager);
        Initialization();
       
    }
    private void Initialization()
    {
        audioSources = new List<AudioSource>();
        AudioSource[] getAudioSources =  FindObjectsOfType<AudioSource>();
        foreach(AudioSource a in getAudioSources)
        {
            audioSources.Add(a);
        }

        animalClips = new List<AudioClip>();
        foreach(AudioObject a in clips)
        {
            animalClips.Add(CreateNewAudioClip(a));
        }

      
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        HandleVolume();
    }
    /// <summary>
    /// Method to handle volume output using arrow keys
    /// </summary>
    private void HandleVolume()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (masterVolume > 0)
                masterVolume -= Time.deltaTime;

            if (masterVolume < 0)
                masterVolume = 0;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (masterVolume < 1)
                masterVolume += Time.deltaTime;

            if (masterVolume > 1)
                masterVolume = 1;
        }

        foreach(AudioSource a in audioSources)
        {
            a.volume = masterVolume;
        }

        if (mute)
        {
            foreach (AudioSource a in audioSources)
            {
                a.volume = 0f;
            }
        }

    }
    /// <summary>
    /// Grab clip from audio list
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetClip(string name)
    {
        foreach(AudioClip a in animalClips)
        {
            if (a.name == name)
            {
                return a;
            }
        }
        return null;
    }
    /// <summary>
    /// Creates a segment of an audio clip
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    private AudioClip CreateNewAudioClip(AudioObject audioObject)
    {
        int frequency = audioObject.clip.frequency;
        float timeLength = audioObject.endTime - audioObject.startTime;
        int samplesLength = (int)(frequency * timeLength);
        if (audioObject.playWholeClip)
        {
            audioObject.clip.name = audioObject.name;
            return audioObject.clip;
        }

        AudioClip newClip = AudioClip.Create(audioObject.name, samplesLength, 1, frequency, false);
        
        //temporary buffer for samples
        float[] data = new float[samplesLength];

        audioObject.clip.GetData(data, (int)(frequency * timeLength));

        //Transfser the data to the new clip
        newClip.SetData(data, 0);

        return newClip;
    }
}
