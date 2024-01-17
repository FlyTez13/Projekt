using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public AIPath aiPath;

    private Rigidbody rb;

    private NavMeshAgent navMeshAgent;
    private bool isJumping = false;
    public float jumpSpeed;

    public Transform jumpPoint;

    public string nextScene = "Level1";

    public int maxHealth = 100;
    int currentHealth;

    private bool isOnGround;
    public Transform enemyPos;
    public float positionRadius;
    public LayerMask ground;

    private float airTimeCount;
    public float airTime;
    private bool inAir;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        StartCoroutine(EnemyBehaviour());
    }

    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    IEnumerator EnemyBehaviour()
    {
        while (true)
        {
            float distanceToJumpPoint = Vector3.Distance(transform.position, jumpPoint.position);

            if (distanceToJumpPoint < 2f && !isJumping)
            {
                isJumping = true;

                GetComponent<Animator>().SetTrigger("IsJumping");

                navMeshAgent.isStopped = true;

                Vector3 jumpDirection = (jumpPoint.position - transform.position).normalized;
                rb.AddForce(jumpDirection * 10f, ForceMode.Impulse);

                yield return new WaitForSeconds(1.5f);

                navMeshAgent.isStopped = false;

                isJumping = false;
            }
        }
    }

    void Jump()
    {
        Vector3 jumpDirection = new Vector3(0f, 5f, 0f);
        GetComponent<Rigidbody>().AddForce(jumpDirection, ForceMode.Impulse);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
            SwithScene();
        }
    }

    private void SwithScene()
    {
        SceneManager.LoadScene("Level1");
    }

    void Die()
    {
        Debug.Log("Enemy died ");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void DetectGround()
    {
        isOnGround = Physics2D.OverlapCircle(enemyPos.position, positionRadius, ground);

        if (isOnGround == true)
        {
            inAir = true;
            airTimeCount = airTime;
            rb.velocity = Vector2.up * jumpSpeed;
            animator.SetBool("IsJumping", true);
        }
    }
}