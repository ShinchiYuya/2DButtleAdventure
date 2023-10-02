using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject target;
    [SerializeField] float disPlayer;
    public NavMeshAgent agent;
    public float distance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find(target.name);
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        Debug.Log(distance);

        if (distance < disPlayer)
        {
            agent.destination = target.transform.position;
        }
    }
}
