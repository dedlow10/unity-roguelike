using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Fighter
{
    [SerializeField] float sightRange = 5f;
    [SerializeField] EnemyWeapon weapon;

    private AIDestinationSetter aIDestinationSetter;
    private GameObject player;
    private Vector3 startPosition;
    public float visionDistance = 100f;
    private bool isChasing = false;
    private AIPath aiPath;
    private float spawnDelay = 1f;
    private float timeSinceSpawn = 0;
    protected override void Awake()
    {
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        base.Awake();
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer().gameObject;
        startPosition = transform.position;
        aiPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn < spawnDelay) return;
        if (GameManager.instance.IsGamePaused()) return;
        if (CanSeePlayer())
        {
            aIDestinationSetter.target = player.transform;
            isChasing = true;
            if (weapon.CanFire() && CanHitPlayer()) {
                weapon.Fire();
            }
        }
        else
        {
            if (isChasing)
            {
                aIDestinationSetter.target = null;
                aiPath.destination = startPosition;
            }

            if (aiPath.reachedDestination)
            {
                aiPath.isStopped = true;
            }
        }
    }
    
    private bool CanSeePlayer()
    {
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        bool inSightRange = distance < sightRange;
        if (!inSightRange) return false;


        Vector2 direction = player.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, visionDistance, LayerMask.GetMask("Default", "Obstacle"));
            
        if (hit.collider?.gameObject?.tag == "Player")
        {
            return true;
        }
      
        return false;

        //var hit = Physics2D.Linecast(transform.position, transform.forward * 20);
        //return hit.collider.tag == "Player";
        //float distance = Vector2.Distance(transform.position, player.transform.position);
        //return distance < sightRange;
    }

    private bool CanHitPlayer()
    {
        var hit = Physics2D.Linecast(transform.position, GameManager.instance.GetPlayer().transform.position, LayerMask.GetMask("Obstacle", "Object "));
        return hit.collider == null;
    }
}
