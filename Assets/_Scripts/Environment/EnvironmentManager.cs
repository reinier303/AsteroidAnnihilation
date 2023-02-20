using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

namespace AsteroidAnnihilation
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance;

        private ObjectPooler objectPooler;

        public int WreckagePerChunk;
        public int RocksPerChunk;

        private List<Vector2Int> grid; 

        private void Awake()
        {
            Instance = this;
            grid = new List<Vector2Int>();
        }

        private void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }

        public void GenerateEnvironmentChunk(Vector2 position, Vector2 size, Vector2Int gridPos)
        {
            if (grid.Contains(gridPos)) { return; }
            PlaceWreckages(position, size, gridPos);
            //PlaceRocks(position, size, gridPos);
        }

        private void PlaceWreckages(Vector2 position, Vector2 size, Vector2Int gridPos)
        {
            for(int i = 0; i < WreckagePerChunk; i++)
            {
                Vector2 finalPosition = position + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.x / 2, size.x / 2));
                objectPooler.SpawnFromPool("Wreckage", finalPosition, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            }
            grid.Add(gridPos);
        }
    }
}
