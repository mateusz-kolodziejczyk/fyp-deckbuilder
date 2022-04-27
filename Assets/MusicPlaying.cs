using System.Collections;
using System.Collections.Generic;
using Audio;
using Enums;
using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlaying : MonoBehaviour
{
    // Use a bespoke struct to expose a dictionary like structure to the inspector
    // Inspired by https://forum.unity.com/threads/cant-see-dictionaries-in-inspector.938741/
    [SerializeField] private List<SerializableKeyValuePair<SceneType, AudioClip>> sceneSongList;
    private Dictionary<SceneType, AudioClip> sceneSongs = new();
    private AudioClip song;
    public AudioClip Song
    {
        get => song;
        set => song = value;
    }

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // setup dictionary
        foreach (var sceneSongPair in sceneSongList)
        {
            sceneSongs[sceneSongPair.key] = sceneSongPair.value;
        }

        if (song == null)
        {
            song = sceneSongs[SceneMatcher.IndexToSceneType[SceneManager.GetActiveScene().buildIndex]];
        }
        PlaySong();
    }

    public void PlaySong()
    {
        audioSource.clip = Song;
        audioSource.Play();
    }
}
