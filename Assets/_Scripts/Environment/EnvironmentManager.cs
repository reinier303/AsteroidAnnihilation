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

        public float WreckagePerChunk;
        public float RocksPerChunk;

        private List<Vector2Int> wreckageGrid;
        private List<Vector2Int> rockGrid;

        private List<GameObject> activeObjects;

        private void Awake()
        {
            Instance = this;
            wreckageGrid = new List<Vector2Int>();
            rockGrid = new List<Vector2Int>();
            activeObjects = new List<GameObject>();
        }

        private void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }

        public void GenerateEnvironmentChunk(Vector2 position, Vector2 size, Vector2Int gridPos)
        {
            if (wreckageGrid.Contains(gridPos)) { return; }
            PlaceWreckages(position, size, gridPos);
            PlaceRocks(position, size, gridPos);
        }

        private void PlaceWreckages(Vector2 position, Vector2 size, Vector2Int gridPos)
        {
            int wreckageFloored = Mathf.FloorToInt(RocksPerChunk);
            float remainder = RocksPerChunk / 1;

            for (int i = 0; i < wreckageFloored; i++)
            {
                Vector2 finalPosition = position + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.x / 2, size.x / 2));
                activeObjects.Add(objectPooler.SpawnFromPool("Wreckage", finalPosition, Quaternion.Euler(0, 0, Random.Range(0, 360f))));
            }

            if (Random.Range(0f, 1f) < remainder)
            {
                Vector2 finalPosition = position + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.x / 2, size.x / 2));
                activeObjects.Add(objectPooler.SpawnFromPool("Wreckage", finalPosition, Quaternion.Euler(0, 0, Random.Range(0, 360f))));
            }
            wreckageGrid.Add(gridPos);
        }

        private void PlaceRocks(Vector2 position, Vector2 size, Vector2Int gridPos)
        {
            int rocksFloored = Mathf.FloorToInt(RocksPerChunk);
            float remainder = RocksPerChunk / 1;

            for (int i = 0; i < rocksFloored; i++)
            {
                Vector2 finalPosition = position + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.x / 2, size.x / 2));
                activeObjects.Add(objectPooler.SpawnFromPool("Rocks", finalPosition, Quaternion.Euler(0, 0, Random.Range(0, 360f))));
            }
            
            if(Random.Range(0f, 1f) < remainder)
            {
                Vector2 finalPosition = position + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.x / 2, size.x / 2));
                activeObjects.Add(objectPooler.SpawnFromPool("Rocks", finalPosition, Quaternion.Euler(0, 0, Random.Range(0, 360f))));
            }
            rockGrid.Add(gridPos);
        }

        public void ResetEnvironment()
        {
            foreach(GameObject gObj in activeObjects)
            {
                gObj.SetActive(false);
            }
            wreckageGrid.Clear();
            rockGrid.Clear();
        }
    }
}
