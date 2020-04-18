using UnityEngine;

namespace Player {
    public class Pistol : MonoBehaviour {
        string fire = "Fire1";
        bool canFire = true;
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] Transform firePoint;
        [SerializeField] public int dmg = 60;
        [SerializeField] public float speed = 10f;

        public void onFireBind(string _fire) {
            fire = _fire;
        }

        public void onDisablePlayer() {
            canFire = false;
        }

        public void onEnablePlayer() {
            canFire = true;
        }

        // Start is called before the first frame update
        void Start() { }

        void FirePistol() {
            //TODO request from pool
            var newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            newBullet.GetComponent<Bullet>().EnableBullet(1, dmg, speed);
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetButtonDown(fire) && canFire) {
                FirePistol();
            }
        }
    }
}