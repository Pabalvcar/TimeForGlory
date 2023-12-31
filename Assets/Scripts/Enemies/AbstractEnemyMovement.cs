using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public abstract class AbstractEnemyMovement : MonoBehaviour
{

    public float movementSpeed;
    public float detectionRadius;

    protected AIDestinationSetter aiDestinationSetter;
    protected Transform playerTransform;
    protected Animator animator;
    protected AIPath aiPath;
    protected Rigidbody2D rigidBody;
    private bool currentlyMoving;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = gameObject.GetComponent<Animator>();
        aiDestinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
    }

    protected virtual void FixedUpdate()
    {
        if (!(aiPath.desiredVelocity.x >= 0.01))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance <= detectionRadius)
        {
            aiPath.enabled = true;
            aiDestinationSetter.target = playerTransform;
            animator.SetFloat("isMoving", 1);
        } else
        {
            aiPath.enabled = false;
            aiDestinationSetter.target = null;
            animator.SetFloat("isMoving", 0);
            if (!currentlyMoving)
            {
                StartCoroutine(WalkRandomDirection());
            }
        }

    }

    private Vector3Int GetDirectionvector()
    {
        List<Vector2Int> listPositions = new List<Vector2Int>(CorridorBasedDungeonGenerator.Instance.tiles);

        Vector2Int currentPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        int distance = Random.Range(1, 6);

        Vector2Int direction = Direction2D.GetRandomCardinalDirection();

        Vector2Int movementVector = direction * distance;

        bool isWalkable = true;

        for (int i = 0; i <= distance + 1; i++)
        {
            currentPosition = currentPosition + direction;

            if (listPositions.Contains(currentPosition))
            {
                isWalkable = isWalkable && true;
            }
            else
            {
                isWalkable = false;
            }
        }

        if (isWalkable == true)
        {
            return new Vector3Int(movementVector.x, movementVector.y, 0);
        } else
        {
            return Vector3Int.zero;
        }
        
    }

    private IEnumerator WalkRandomDirection()
    {
        currentlyMoving = true;

        Vector3 movementVector = GetDirectionvector();

        Vector3 newPosition = transform.position + movementVector;

        while (transform.position.x != newPosition.x || transform.position.y != newPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, movementSpeed * Time.deltaTime);
            animator.SetFloat("isMoving", 1);
            yield return new WaitForSeconds(0.01f);
        }

        animator.SetFloat("isMoving", 0);

        yield return new WaitForSeconds(5f);

        currentlyMoving = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(BattleManager.Instance.StartBattle(gameObject));
        }
    }
}
