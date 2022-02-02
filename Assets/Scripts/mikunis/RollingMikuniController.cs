using UnityEngine;

namespace mikunis
{
    public class RollingMikuniController : Mikuni
    {

        public Transform rotatingBody;
        public float rotSpeedX = 250f;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            float speed = agent.velocity.magnitude/agent.speed;
            rotatingBody.Rotate(Vector3.right, rotSpeedX * speed * Time.deltaTime);
        }
    }
}
