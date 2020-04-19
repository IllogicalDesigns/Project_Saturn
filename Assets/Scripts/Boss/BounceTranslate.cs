using UnityEngine;

namespace Boss {
    public class BounceTranslate : MonoBehaviour {

        [SerializeField] private Vector3 firstPos;
        [SerializeField] private Vector3 secondPos;
        [SerializeField] private float[] translateSpeed;
        [SerializeField] private float[] stumbleX;
        private Vector3 stumblePos;
        private bool inStumble;
        private bool movingInStumble;
        private bool movingToFirstPos = true;
        private Transform trans;

        private int curDifficulty;
        
        public void ChangeDifficulty(int difficulty) {
            curDifficulty = difficulty;
        }

        private void Start() {
            trans = transform;
        }
        
        private void Update() {
            if (movingInStumble) {
                MoveTowardsPos(stumblePos);
            }else if (inStumble) {
                return;
            }
            else {
                MoveTowardsPos(movingToFirstPos ? firstPos : secondPos);
            }
        }

        private void MoveTowardsPos(Vector3 target) {
            var translateDir = (target - trans.position).normalized * (translateSpeed[curDifficulty] * Time.deltaTime);
            trans.Translate(translateDir);

            if (Vector3.Distance(target, trans.position) > 0.5f) return;
            
            if (movingInStumble) {
                movingInStumble = false;
            }
            else {
                movingToFirstPos = !movingToFirstPos;
            }
        }

        private void EnteredStumble() {
            stumblePos = new Vector3(stumbleX[0], 0.1f, trans.position.z);
            inStumble = true;
            movingInStumble = true;
        }

        private void ExitedStumble() {
            stumblePos = new Vector3(stumbleX[1], 0.1f, trans.position.z);
            inStumble = false;
            movingInStumble = true;
        }
    }
}