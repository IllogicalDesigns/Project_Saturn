using UnityEngine;

namespace Player {
    public class Pistol : MonoBehaviour {
        string fire = "Fire1";
        bool canFire = true;
        [SerializeField] Transform firePoint;
        [SerializeField] public int dmg = 60;
        [SerializeField] public float ReloadTime = 0.1f;
        [SerializeField] public float DirectionArcRange = 1f;
        [SerializeField] public GameObject BulletTrailPrefab;
        private float lastFireTime;
        private float arcRangeRad;

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
        }

        void FirePistol() {
            var rand = 2 * Random.value * arcRangeRad - arcRangeRad; // +/- angle from forward
            if (!Physics.Raycast(firePoint.position,
                (transform.forward + transform.TransformDirection(new Vector3(Mathf.Sin(rand), 0, Mathf.Cos(rand)))
                    .normalized), out var hitInfo)) return;

            if (hitInfo.transform.gameObject.CompareTag("Enemy")) {
                hitInfo.transform.gameObject.SendMessage("ApplyDmg", dmg);
            }

            var trail = Instantiate(BulletTrailPrefab);
            trail.GetComponent<LineRenderer>().SetPositions(new[]{firePoint.transform.position, hitInfo.point});
            
            lastFireTime = Time.time;
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetButton(fire) && canFire && Time.time > lastFireTime + ReloadTime) {
                FirePistol();
            }
        }
    }
}