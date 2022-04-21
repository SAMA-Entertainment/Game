using UnityEngine;

namespace mikunis
{
    public class RollingMikuni : HidingMikuni
    {

        public Transform rotatingBody;
        public Vector3 rotateDir = Vector3.right;
        public float rotSpeedX = 250f;

        protected override void OnAnimation()
        {
            float speed = agent.velocity.magnitude/agent.speed;
            rotatingBody.Rotate(rotateDir, rotSpeedX * speed * Time.deltaTime);
        }
    }
}
