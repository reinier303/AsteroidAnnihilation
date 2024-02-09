using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Audio", order = 999)]
    public class ScriptableAudio : ScriptableObject
    {
        public List<AudioClip> Clips;
        public Vector2 VolumeRange = Vector2.one;
        public Vector2 PitchRange = Vector2.one;
        public string Tag;
    }
}