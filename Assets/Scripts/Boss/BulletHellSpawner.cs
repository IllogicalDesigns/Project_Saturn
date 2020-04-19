using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boss {
    public class BulletHellSpawner : MonoBehaviour {

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float spawnInterval;
        [SerializeField] private float spawnSpreadAngle;
        [SerializeField] private Vector3 fireDir;
        [SerializeField] private int totalBulletsToSpawn;

        [SerializeField] private float speed;
        [SerializeField] private int damage;

        private float intervalTracker;
        private int projectilesToSpawn;

        public bool IsSpawning => projectilesToSpawn > 0;

        public void StartSpawning() {
            projectilesToSpawn = totalBulletsToSpawn;
        }

        private void Update() {
            if (!IsSpawning) return;
            
            if (intervalTracker > 1e-4) {
                intervalTracker -= Time.deltaTime;
            }
            else {
                DoSpawn();
            }
        }

        private void DoSpawn() {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.forward = fireDir;
            bullet.transform.eulerAngles = new Vector3(0f,
                bullet.transform.eulerAngles.y + Random.Range(-spawnSpreadAngle, spawnSpreadAngle), 0f);
            bullet.transform.position = firePoint.position;
            
            var bulletComp = bullet.GetComponent<Bullet>();
            bulletComp.SetSpeed(speed);
            bulletComp.SetDmg(damage);
            bulletComp.SetOwner(-1);
            
            intervalTracker = spawnInterval;
            projectilesToSpawn--;
        }

    }
}