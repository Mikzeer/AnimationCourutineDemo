using UnityEngine;
using System.Collections.Generic;

public class GameSoundManager : GenericSingleton<GameSoundManager>
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
}