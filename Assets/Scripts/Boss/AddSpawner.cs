using System.Collections.Generic;
using UnityEngine;

namespace Boss {
    public class AddSpawner : MonoBehaviour {

        [SerializeField] private List<Vector3> spawnPositions;
        [SerializeField] private List<GameObject> addPrefabs;

        private GameObject player;
        private bool spawning;
        public bool IsSpawning => spawning;
        
        public void SpawnAdds() {
            spawning = true;
        }

        private void Start() {
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (!spawning) return;

            // Spawn adds at each position
            foreach (var position in spawnPositions) {
                var prefab = addPrefabs[Random.Range(0, addPrefabs.Count)];

                var spawn = Instantiate(prefab);
                spawn.transform.position = position;
                
            }

            spawning = false;
        }

    }
}