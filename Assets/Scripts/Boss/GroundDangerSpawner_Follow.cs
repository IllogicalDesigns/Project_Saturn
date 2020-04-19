using UnityEngine;

namespace Boss {
    public class GroundDangerSpawner_Follow : GroundDangerSpawner {
        
        protected override void DoSpawn() {
            base.DoSpawn(Player.transform.position);
        }

    }
}