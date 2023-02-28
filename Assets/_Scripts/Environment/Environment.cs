using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private Vector2Int pieceAmountRange;
        private bool initialized;
        [SerializeField] private List<GameObject> environmentPieces;
        [SerializeField] private List<GameObject> activePieces;
        [SerializeField] private Vector2 RandomPositionRange;

        private Transform player;

        protected void EnableRandomPieces()
        {
            int pieceAmount = Random.Range(pieceAmountRange.x, pieceAmountRange.y);
            List<int> possibleNumbers = new List<int>();
            List<int> randomNumbers = new List<int>();
            //Select random enemies from children
            for (int i = 0; i < environmentPieces.Count; i++)
            {
                possibleNumbers.Add(i);
            }
            for (int i = 0; i < pieceAmount; i++)
            {
                int index = Random.Range(0, possibleNumbers.Count);
                randomNumbers.Add(possibleNumbers[index]);
                possibleNumbers.RemoveAt(index);
            }
            //Debug.Log(randomNumbers.Count);

            for (int i = 0; i < pieceAmount; i++)
            {
                GameObject child = environmentPieces[randomNumbers[i]];
                activePieces.Add(child);
                child.SetActive(true);
            }
        }

        private void OnEnable()
        {
            if(!initialized)
            {
                initialized = true;
                player = Player.Instance.transform;
            }
            else
            {
                foreach (GameObject piece in environmentPieces)
                {
                    piece.transform.localPosition = new Vector3(0, 0, 0) + new Vector3(Random.Range(RandomPositionRange.x, RandomPositionRange.y), Random.Range(RandomPositionRange.x, RandomPositionRange.y), 0);
                }
                EnableRandomPieces();
                StartCoroutine(CheckDistanceToPlayer());
            }
        }

        private IEnumerator CheckDistanceToPlayer()
        {
            if(Vector2.Distance(transform.position, player.position) > 100f)
            {
                ToggleChildren(false);
            }
            else
            {
                ToggleChildren(true);
            }
            yield return new WaitForSeconds(2f);
            StartCoroutine(CheckDistanceToPlayer());
        }

        private void ToggleChildren(bool enable)
        {
            foreach (GameObject piece in activePieces)
            {
                piece.SetActive(enable);
            }
        }

        public void RemoveActivePiece(GameObject piece)
        {
            activePieces.Remove(piece);
            if(activePieces.Count < 1) 
            {
                StartCoroutine(DisableAfterTime());
            }
        }

        private IEnumerator DisableAfterTime()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }
    }
}
