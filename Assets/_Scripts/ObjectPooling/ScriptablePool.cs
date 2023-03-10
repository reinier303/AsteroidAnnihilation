using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "ObjectPool/Pool", order = 999)]
    public class ScriptablePool : ScriptableObject
    {
        public string Tag;
        public GameObject Prefab;
        public int Amount;
        public bool disableOnMissionExit = false;
    }
}