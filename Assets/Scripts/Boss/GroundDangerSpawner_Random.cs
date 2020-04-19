using UnityEngine;

namespace Boss {
    public class GroundDangerSpawner_Random : GroundDangerSpawner {

        [SerializeField] private Bounds spawnBounds;
        
        protected override void DoSpawn() {
            var min = spawnBounds.min;
            var max = spawnBounds.max;
            var spawnPos = new Vector3(Random.Range(min.x, max.x), 0f, Random.Range(min.z, max.z));
            base.DoSpawn(spawnPos);
        }

    }
}