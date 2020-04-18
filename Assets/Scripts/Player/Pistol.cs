using System.Runtime.InteropServices;
using UnityEngine;

namespace Player {
    public class Pistol : MonoBehaviour {
        string fire = "Fire1";
        bool canFire = true;
        [SerializeField] Transform firePoint;
        [SerializeField] public int dmg = 60;
        [SerializeField] public float ReloadTime = 0.1f;
        [SerializeField] public float DirectionArcRange = 1f;

        [SerializeField] public GameObject BulletPrefab;
        [SerializeField] public float speed = 1f;

        [SerializeField] public GameObject BulletTrailPrefab;
        [SerializeField] AudioSource gunShot;

        private float lastFireTime;
        private float arcRangeRad;
        private CamEffects camEffects;

        public float kickback = 6f;

        Rigidbody rigid;

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
        void Start() {
            arcRangeRad = Mathf.Deg2Rad * DirectionArcRange;
            rigid = GetComponent<Rigidbody>();
            camEffects = FindObjectOfType<CamEffects>();
        }

        void FirePistol() {
            var rand = 2 * Random.value * arcRangeRad - arcRangeRad; // +/- angle from forward
            var forward = transform.forward;
            var fireDir = forward +
                          transform.TransformDirection(new Vector3(Mathf.Sin(rand), 0, Mathf.Cos(rand))).normalized;

            var bullet = Instantiate(BulletPrefab);
            bullet.transform.forward = fireDir;
            bullet.transform.eulerAngles = new Vector3(0f, bullet.transform.eulerAngles.y, 0f);
            bullet.transform.position = firePoint.position;
            
            var bulletComp = bullet.GetComponent<Bullet>();
            bulletComp.SetSpeed(speed);
            bulletComp.SetDmg(dmg);
            bulletComp.SetOwner(1);
            
            lastFireTime = Time.time;

            camEffects.Shake(0.05f, 0.5f);
            rigid.AddForce(-forward * kickback, ForceMode.Impulse);
            rigid.AddForce(-transform.forward * kickback, ForceMode.Impulse);

            gunShot.Play();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetButtonDown(fire) && canFire && Time.time > lastFireTime + ReloadTime) {
                FirePistol();
            }
        }
    }
}