using UnityEngine;

namespace Player {
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        int dmg = 25;
        public int owner = -1;  //-1 == enemy, 1-4 is players
        bool bulletEnabled = true;
        float knock = 10f;

        // Start is called before the first frame update
        void Start() {
        
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player") && owner == 1 ||
                other.gameObject.CompareTag("Enemy") && owner == -1 || 
                other.isTrigger) return;
            
            if(other.gameObject.CompareTag("Player") && owner == -1)
            {
                other.gameObject.SendMessage("ApplyDamage", dmg);
                other.SendMessage("ApplyKnockback", new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, knock), SendMessageOptions.DontRequireReceiver);
                DisableBulletBloody();
            }
            else if (other.gameObject.CompareTag("Enemy") && owner != -1)
            {
                other.gameObject.SendMessage("ApplyDamage", dmg);
                other.SendMessage("ApplyKnockback", new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, knock), SendMessageOptions.DontRequireReceiver);
                DisableBulletBloody();
            }
            else
            {
                DisableBulletDefaultImpact();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(bulletEnabled)
                transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        }

        void DisableBullet()
        {
            
            //transform.position = Vector3.zero;
            //bulletEnabled = false;
            //collider.enabled = false;
            //renderer.enabled = false;
            Destroy(gameObject);
            //TODO put in pool
        }

        void DisableBulletBloody()
        {
            if (CamEffects.Instance != null) CamEffects.Instance.FreezeFrame();
            if (Blood.Instance != null) Blood.Instance.EmitBlood(transform);
            DisableBullet();
        }

        void DisableBulletDefaultImpact() {
            if (Blood.Instance != null) Blood.Instance.EmitWallImpact(transform);
            DisableBullet();
        }

        public void SetOwner(int _owner) {
            owner = _owner;
        }

        public void SetDmg(int _dmg) {
            dmg = _dmg;
        }

        public void SetSpeed(float _speed) {
            speed = _speed;
        }
    }
}
