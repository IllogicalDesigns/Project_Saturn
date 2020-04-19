using UnityEngine;

namespace Player {
    public class CameraFollow : MonoBehaviour {

        [SerializeField] public Transform FollowTransform;
        [SerializeField] public Vector3 Offset;
        private Transform selfTransform;
    
        // Start is called before the first frame update
        void Start() {
            selfTransform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update() {
            selfTransform.position = FollowTransform.position + Offset;
        }
    }
}
