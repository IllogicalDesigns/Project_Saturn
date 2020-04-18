using UnityEngine;

namespace Player {
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        int dmg = 25;
        int owner = -1;  //-1 == enemy, 1-4 is players
        bool bulletEnabled = true;

        [SerializeField] public Collider collider;
        [SerializeField] public MeshRenderer renderer;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player") && owner == -1)
            {
                other.gameObject.SendMessage("ApplyDmg", dmg);
                DisableBulletBloody();
            }
            else if (other.gameObject.CompareTag("Enemy") && owner != -1)
            {
                other.gameObject.SendMessage("ApplyDmg", dmg);
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
            transform.position = Vector3.zero;
            bulletEnabled = false;
            collider.enabled = false;
            renderer.enabled = false;
            Destroy(gameObject);
            //TODO put in pool
        }

        void DisableBulletBloody()
        {
            //TODO Bloody effects
            DisableBullet();
        }

        void DisableBulletDefaultImpact()
        {
            //TODO impact effects
            DisableBullet();
        }


        public void EnableBullet(int _owner, int _dmg, float _speed)
        {
            dmg = _dmg;
            speed = _speed;
            owner = _owner;
            bulletEnabled = true;
            collider.enabled = true;
            renderer.enabled = true;
        }
    }
}
