using UnityEngine;

namespace Boss {
    public class BounceTranslate : MonoBehaviour {

        [SerializeField] private Vector3 firstPos;
        [SerializeField] private Vector3 secondPos;
        [SerializeField] private float[] translateSpeed;
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
            MoveTowardsPos(movingToFirstPos ? firstPos : secondPos);
        }

        private void MoveTowardsPos(Vector3 target) {
            var translateDir = (target - trans.position).normalized * (translateSpeed[curDifficulty] * Time.deltaTime);
            trans.Translate(translateDir);

            if (Vector3.Distance(target, trans.position) < 1f) {
                movingToFirstPos = !movingToFirstPos;
            }
        }
    }
}