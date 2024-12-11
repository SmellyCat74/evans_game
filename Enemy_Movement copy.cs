using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Variables
    public float speed = 3.5f; // Movement speed
    public float chaseRange = 5f; // Distance at which the enemy starts chasing the player

    private Vector3 randomDirection;
    private float changeDirectionTime = 2f; // Time interval to change direction
    private float directionTimer = 0f;

    // Using advanced data type: Enum to represent enemy states
    private enum EnemyState
    {
        Idle,
        Patrol,
        Chase
    }

    private EnemyState currentState = EnemyState.Patrol;

    // Dictionary to track buffs and their effects
    private Dictionary<string, float> buffs = new Dictionary<string, float>();

    // Player reference
    private Transform player;

    void Start()
    {
        // Initialize random direction
        randomDirection = GetRandomDirection();

        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Adding initial buffs
        buffs.Add("Speed", 1.0f);
        buffs.Add("Health", 100.0f);
    }

    void Update()
    {
        // Nested decision structures
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                currentState = EnemyState.Chase;
            }
            else if (currentState == EnemyState.Chase && distanceToPlayer > chaseRange)
            {
                currentState = EnemyState.Patrol;
            }
        }

        switch (currentState) // Using switch statement for state management
        {
            case EnemyState.Idle:
                // Do nothing
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
        }
    }

    // Patrol behavior: Moves in a random direction
    void Patrol()
    {
        directionTimer += Time.deltaTime;

        if (directionTimer >= changeDirectionTime)
        {
            randomDirection = GetRandomDirection();
            directionTimer = 0f;
        }

        transform.Translate(randomDirection * speed * Time.deltaTime);
    }

    // Chase behavior: Moves towards the player
    void ChasePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.Translate(directionToPlayer * speed * buffs["Speed"] * Time.deltaTime);
    }

    // Function to get a random direction
    private Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        return new Vector3(randomX, 0, randomZ).normalized;
    }

    // Function with parameters: Adds a buff to the enemy
    public void AddBuff(string buffName, float buffValue)
    {
        if (buffs.ContainsKey(buffName))
        {
            buffs[buffName] += buffValue;
        }
        else
        {
            buffs.Add(buffName, buffValue);
        }
    }

    // Function with a return value: Get the value of a specific buff
    public float GetBuffValue(string buffName)
    {
        if (buffs.ContainsKey(buffName))
        {
            return buffs[buffName];
        }
        return 0f; // Default value if buff doesn't exist
    }

    // Perform operations on a list
    public List<string> GetBuffNames()
    {
        return new List<string>(buffs.Keys);
    }
}
