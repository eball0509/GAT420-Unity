using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] public float maxSpeed = 0;
    [SerializeField] public float minSpeed = 0;
    [SerializeField] public float maxForce = 0;

    public Vector3 Velocity { get; set; }
    public Vector3 Acceleration { get; set; }
    public Vector3 Direction { get { return Velocity.normalized; } }

    public abstract void ApplyForce(Vector3 force);
}
