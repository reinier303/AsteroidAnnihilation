using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Audio", order = 999)]
    public class ScriptableAudio : ScriptableObject
    {
        public AK.Wwise.Event Sound;
        public string Tag;
    }
}