using UnityEngine;

namespace Player {
    public class Movement : MonoBehaviour {
        string moveVert = "Vertical", moveHor = "Horizontal", dodge = "Jump";
        string lookVert = "lookVertical", lookHor = "lookHorizontal";
        [SerializeField] float speed = 6f, dodgeForce = 6f;
        [SerializeField] bool canMove = true;
        [SerializeField] bool gamePad = false;
        Vector3 movePos, lookPos;
        Rigidbody rbody;
        Camera cam;

        [SerializeField] InGameCanvas gameCanvas;
        [SerializeField] float timeBetweenDodges = 0.5f;
        float dodgeTimer = 0f;
        bool dodging = false;
        
        public float MovementFactor = 1f;

        [SerializeField] GameObject Crosshair;
        [SerializeField] private float CrosshairAltitude;

        [SerializeField] Animator animator;

        // Start is called before the first frame update
        void Start() {
            rbody = gameObject.GetComponent<Rigidbody>();
            cam = FindObjectOfType<Camera>();

            gameCanvas.SetupDodgeSlider(timeBetweenDodges, dodgeTimer);
        }

        public void onDisablePlayer() {
            canMove = false;
        }

        public void onEnablePlayer() {
            canMove = true;
        }

        public void BindControls(string _vertical, string _horizontal, string _dodge) {
            moveVert = _vertical;
            moveHor = _horizontal;
            dodge = _dodge;
        }

        // Update is called once per frame
        void Update() {
            if (!canMove)
                return;

            movePos.z = Input.GetAxisRaw(moveVert);
            movePos.x = Input.GetAxisRaw(moveHor);

            if (gamePad) {
                lookPos.x = Input.GetAxisRaw(lookVert);
                lookPos.z = Input.GetAxisRaw(lookHor);
            }

            if (Input.GetButtonDown(dodge) && dodgeTimer >= timeBetweenDodges) {
                Dodge();
            }
            else if (dodgeTimer <= timeBetweenDodges) {
                dodgeTimer += Time.deltaTime;
                gameCanvas.UpdateDodgeSlider(dodgeTimer);
            }

            if(animator == null){
                return;
            }

            if (movePos.magnitude > 0)
                animator.SetBool("Walking", true);
            else
                animator.SetBool("Walking", false);

            Debug.Log(rbody.velocity.magnitude);
            if(dodging && dodgeTimer > 0.1 && rbody.velocity.magnitude < 5) {
                dodging = false;
                animator.SetBool("Dodging", false);
            }

        }

        void Turning() {
            if (!gamePad) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

                float enter;

                if (groundPlane.Raycast(ray, out enter)) {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    transform.LookAt(new Vector3(hitPoint.x, transform.position.y, hitPoint.z), Vector3.up);
                    Crosshair.transform.position = new Vector3(hitPoint.x, CrosshairAltitude, hitPoint.z);
                }
            }
            else {
                var position = transform.position;
                transform.LookAt(position + lookPos, Vector3.up);
                Crosshair.transform.position = position + lookPos;
            }
        }


        void Dodge() {
            dodging = true;
            animator.SetBool("Dodging", true);
            rbody.AddForce(movePos.normalized * (dodgeForce * MovementFactor), ForceMode.Impulse);
            dodgeTimer = 0;
        }

        private void FixedUpdate() {
            if (!canMove)
                return;

            rbody.MovePosition(rbody.position + movePos.normalized * (speed * MovementFactor * Time.fixedDeltaTime));
            Turning();
        }
    }
}