using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class ParallaxBackground : SerializedMonoBehaviour
    {
        public static ParallaxBackground Instance;

        private EnvironmentManager environmentManager;
        public bool generateEnvironmentChunks = false;

        private Transform player;

        private Vector2 size;

        private Vector2Int parallaxNumber;

        private float zValue;

        public float ParallaxMoveOffsetMultiplier = 3;

        [DictionaryDrawerSettings(KeyLabel ="Keys", ValueLabel ="Values")] 
        [SerializeField] private Dictionary<Transform, Vector2Int> BackgroundElements;

        public bool GetSpritesFromResources;
        public List<Sprite> BackgroundSprites;

        private float parallaxMoveOffset;

        //private List<Vector2> backgroundGrid;

        private void Awake()
        {
            Instance = this;
            BackgroundElements = new Dictionary<Transform, Vector2Int>();

            SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            zValue = transform.GetChild(0).localPosition.z;

            size = spriteRenderer.bounds.size;

            parallaxMoveOffset = size.x / ParallaxMoveOffsetMultiplier;
        }

        private void Start()
        {
            player = GameManager.Instance.RPlayer.transform;
            environmentManager = EnvironmentManager.Instance;
            InitializeBackgrounds();
            SetBackgroundsStart();
            StartCoroutine(CheckParallax());
        }

        public void SetMissionBackgrounds()
        {
            if (GetSpritesFromResources) { BackgroundSprites = MissionManager.Instance.GetCurrentBackgrounds(); }
            SetBackgrounds();
        }

        public void SetHubBackgrounds()
        {
            if (!GetSpritesFromResources) { return; }
            BackgroundSprites.Clear();
            Sprite sprite = (Sprite)Resources.Load<Sprite>("Backgrounds/" + EnumCollections.Backgrounds.BackgroundNebulaBlue.ToString());
            BackgroundSprites.Add(sprite);
            SetBackgrounds();
        }

        private void InitializeBackgrounds()
        {
            SetHubBackgrounds();
            for (int i = 0; i < transform.childCount; i++)
            {
                RandomSprite backgroundRandom = transform.GetChild(i).GetComponent<RandomSprite>();
                if (backgroundRandom != null)
                {
                    BackgroundElements.Add(transform.GetChild(i), new Vector2Int(0, i * 10));
                    backgroundRandom.SetSprites(BackgroundSprites);
                    continue;
                }
            }
        }

        private void SetBackgrounds()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                RandomSprite backgroundRandom = transform.GetChild(i).GetComponent<RandomSprite>();
                if (backgroundRandom != null)
                {
                    backgroundRandom.SetSprites(BackgroundSprites);
                    continue;
                }
            }
        }

        public void SetBackgroundsStart()
        {
            Vector3 position = new Vector3(size.x * (parallaxNumber.x + 1), size.x * parallaxNumber.y, zValue);
            Vector2Int gridPosition = new Vector2Int(parallaxNumber.x + 1, parallaxNumber.y);
            SpawnBackground(position, gridPosition, true, true);
            position = new Vector3(size.x * (parallaxNumber.x - 1), size.x * parallaxNumber.y, zValue);
            gridPosition = new Vector2Int(parallaxNumber.x - 1, parallaxNumber.y);
            SpawnBackground(position, gridPosition, true, false);
            position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y + 1), zValue);
            gridPosition = new Vector2Int(parallaxNumber.x, parallaxNumber.y + 1);
            SpawnBackground(position, gridPosition, false, true);
            position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y - 1), zValue);
            gridPosition = new Vector2Int(parallaxNumber.x, parallaxNumber.y - 1);
            SpawnBackground(position, gridPosition, false, true);
            position = new Vector3(0 ,0, zValue);
            gridPosition = new Vector2Int(0, 0);
            SpawnBackground(position, gridPosition, false, true);

        }

        private IEnumerator CheckParallax()
        {
            parallaxNumber = CalculateParallaxNumber();
            if (player.transform.position.x >= (size.x * (parallaxNumber.x + 1)) - (size.x / 2) - parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * (parallaxNumber.x + 1), size.x * parallaxNumber.y, zValue);
                Vector2Int gridPosition = new Vector2Int(parallaxNumber.x + 1, parallaxNumber.y);
                SpawnBackground(position, gridPosition, true , true);
            }
            if (player.transform.position.x <= (size.x * (parallaxNumber.x - 1)) + (size.x / 2) + parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * (parallaxNumber.x - 1), size.x * parallaxNumber.y, zValue);
                Vector2Int gridPosition = new Vector2Int(parallaxNumber.x - 1, parallaxNumber.y);
                SpawnBackground(position, gridPosition , true, false);
            }

            if (player.transform.position.y >= (size.y * (parallaxNumber.y + 1)) - (size.y / 2) - parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y + 1), zValue);
                Vector2Int gridPosition = new Vector2Int(parallaxNumber.x, parallaxNumber.y + 1);
                SpawnBackground(position, gridPosition, false, true);
            }
            if (player.transform.position.y <= (size.y * (parallaxNumber.y - 1)) + (size.y / 2) + parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y - 1), zValue);
                Vector2Int gridPosition = new Vector2Int(parallaxNumber.x, parallaxNumber.y - 1);
                SpawnBackground(position, gridPosition, false, false);
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(CheckParallax());
        }

        //Spawns 3 for diagonals
        private void SpawnBackground(Vector3 position, Vector2Int gridPosition, bool x, bool positive)
        {
            Vector2Int startGridPos = gridPosition;
            Transform background = null;
            //Three side to side
            for (int i = -1; i < 2; i++)
            {
                //Calculate position on grid
                if (x)
                {
                    gridPosition = startGridPos + new Vector2Int(0, i);
                }
                else
                {
                    gridPosition = startGridPos + new Vector2Int(i, 0);
                }
                if (BackgroundElements.ContainsValue(gridPosition))
                {
                    continue;
                }
                //Get available background and set active
                background = GetAvailableBackground();
                background.gameObject.SetActive(true);

                //Calculate and set position in world
                if(x)
                {
                    background.localPosition = position + new Vector3(0, size.y * i, 0);
                    BackgroundElements[background] = gridPosition;
                }
                else
                {
                    background.localPosition = position + new Vector3(size.x * i, 0, 0);
                    BackgroundElements[background] = gridPosition;
                }
                if (generateEnvironmentChunks) { environmentManager.GenerateEnvironmentChunk(background.localPosition, size, gridPosition); }
            }

            //Extra in front
            int direction = positive ? 1 : -1;

            if (x) { gridPosition = startGridPos + new Vector2Int(direction, 0); } 
            else { gridPosition = startGridPos + new Vector2Int(0, direction); }
            if (BackgroundElements.ContainsValue(gridPosition))
            {
                return;
            }
            background = GetAvailableBackground();
            background.gameObject.SetActive(true);
            if (x)
            {
                background.localPosition = position + new Vector3(size.x * direction, 0, 0);
                BackgroundElements[background] = gridPosition;
            }
            else
            {
                background.localPosition = position + new Vector3(0, size.y * direction, 0);
                BackgroundElements[background] = gridPosition;
            }
            if (generateEnvironmentChunks) { environmentManager.GenerateEnvironmentChunk(background.localPosition, size, gridPosition); }
        }

        private Vector2Int CalculateParallaxNumber()
        {
            int x = 0;
            int y = 0;
            if(player.transform.position.x > 0)
            {
                x = Mathf.FloorToInt((player.transform.position.x + (size.x /2)) / (size.x));
            }
            else
            {
                x = Mathf.CeilToInt((player.transform.position.x - (size.x / 2)) / (size.x));
            }
            if (player.transform.position.y > 0)
            {
                y = Mathf.FloorToInt((player.transform.position.y + (size.y / 2)) / (size.y));
            }
            else
            {
                y = Mathf.CeilToInt((player.transform.position.y - (size.y / 2)) / (size.y));
            }
            return new Vector2Int(x, y);
        }

        private Transform GetAvailableBackground()
        {
            foreach(Transform background in BackgroundElements.Keys)
            {
                if (Vector2.Distance(background.localPosition, player.position) >= size.x * 2f)
                {
                    return background;
                }
            }
            Debug.LogWarning("No available backgrounds found!, returning first child");
            return transform.GetChild(0);
        }
    }
}
