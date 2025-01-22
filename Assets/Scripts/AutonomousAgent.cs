using UnityEngine;

public class AutonomousAgent : AiAgent
{
    [Header("Wander")]
    [SerializeField] float displacement;
    [SerializeField] float distance;
    [SerializeField] float radius;

    [Header("Perception")]
    public Perception Seekperception;
    public Perception Fleeperception;


    float angle;

    private void Update()
    {
        //movement.ApplyForce(Vector3.forward * 10);
        transform.position = Utilities.Wrap(transform.position, new Vector3(-10, -10, -10), new Vector3(10, 10, 10));

        //Debug.DrawRay(transform.position, transform.forward * perception.maxDistance, Color.cyan);

        // Seek

        if(Seekperception != null)
        {
            var gameObjects = Seekperception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }
       
        // Flee
        if(Fleeperception != null)
        {
            var gameObjects = Fleeperception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }
           
        if(movement.Acceleration.sqrMagnitude == 0)
        {
            
            angle += Random.Range(-displacement, displacement);

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

            Vector3 point = rotation * (Vector3.forward * radius);

            Vector3 forward = movement.Direction * distance;

            Vector3 force = GetSteeringForce(forward + point);

            movement.ApplyForce(force);
        }
    }

    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Flee(GameObject go)
    {
        Vector3 direction = transform.position - go.transform.position ;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }


    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);

        return force;
    }
}
