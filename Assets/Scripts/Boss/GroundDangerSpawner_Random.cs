using UnityEngine;

namespace Boss {
    public class GroundDangerSpawner_Random : GroundDangerSpawner {

        [SerializeField] private bool useSphereSpawner;
        [SerializeField] private Bounds spawnBounds;
        [SerializeField] private Vector3 sphereCenter;
        [SerializeField] private float sphereRadius;
        
        protected override void DoSpawn() {
            Vector3 spawnPos;
            if (useSphereSpawner) {
                var point = Random.insideUnitCircle * sphereRadius;
                spawnPos = new Vector3(point.x, 0, point.y) + sphereCenter;
            }
            else {
                var min = spawnBounds.min;
                var max = spawnBounds.max;
                spawnPos = new Vector3(Random.Range(min.x, max.x), 0f, Random.Range(min.z, max.z));
            }

            base.DoSpawn(spawnPos);
        }

    }
}