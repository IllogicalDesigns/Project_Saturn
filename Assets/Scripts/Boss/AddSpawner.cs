using System.Collections.Generic;
using UnityEngine;

namespace Boss {
    public class AddSpawner : MonoBehaviour {

        [SerializeField] private List<Vector3> spawnPositions;
        [SerializeField] private List<GameObject> addPrefabs;
        [SerializeField] private int maxAdds;

        private List<GameObject> spawnedAdds = new List<GameObject>();
        private GameObject player;
        private bool spawning;
        public bool IsSpawning => spawning;
        
        public void SpawnAdds() {
            spawning = true;
            spawnedAdds.RemoveAll(add => add == null);
        }

        private void Start() {
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (!spawning) return;

            // Spawn adds at each position
            foreach (var position in spawnPositions) {
                if (spawnedAdds.Count >= maxAdds) break;
                
                var prefab = addPrefabs[Random.Range(0, addPrefabs.Count)];

                var spawn = Instantiate(prefab);
                spawn.transform.position = position;
                spawnedAdds.Add(spawn);                
            }

            spawning = false;
        }

    }
}