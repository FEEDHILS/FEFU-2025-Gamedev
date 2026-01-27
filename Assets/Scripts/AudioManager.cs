using System.Linq;
using UnityEngine;


[System.Serializable]
public class AudioData
{
    public string Name;
    public AudioClip audio;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioData[] Sounds;
    [SerializeField] float PitchMin, PitchMax; // Randomize pitch.
    public static AudioManager instance;
    // [SerializeField] AudioSource src;
    void Awake() => instance = this;


    public void Play(string id)
    {
        AudioData sound = Sounds.FirstOrDefault(x => x.Name == id);

        if (sound == null)
        {
            Debug.LogWarning($"Couldn't find audio: {id}");
            return;
        }

        AudioSource src = gameObject.AddComponent<AudioSource>();
        src.clip = sound.audio;
        src.pitch = Random.Range(PitchMin, PitchMax);
        src.Play();
        Destroy(src, sound.audio.length);
    }
}
