using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Health Variables
    private int maxHealth = 70;
    private int currentHealth;
    public HealthBar healthBar;

    // Attack Variables
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask enemyLayer;

    // Animator Variable
    public Animator animator;
    private char currentOrientation = 'r';

    // Player Object
    public GameObject player;
    public EnemySpawn enemySpawnRef;

    // SceneLoader Variables
    public GameObject sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        // Set player's max health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentOrientation == 'l')
            {
                OrientatePlayer('r');
            } else
            {
                OrientatePlayer('l');
            }
        }
    }


    // Orientates player to enemy's location
    void OrientatePlayer(char enemyLoc)
    {
        if (enemyLoc == 'l')
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            transform.position = new Vector3(-0.24f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.position = new Vector3(.24f, transform.position.y, transform.position.z);
        }

        currentOrientation = enemyLoc;
    }


    // Player slashes 
    public void Slash(int cutType)
    {
        // Detect enemies in range of slash
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        char enemyLoc = 'r';
        if (enemies.Length != 0)
        {
            // Find corresponding enemy
            Collider2D corEnemy = enemies[0];
            float corEnemyDist = 100;
            float enemyDist;
            bool hitEnemy = false;
            foreach (Collider2D enemy in enemies)
            {
                // Iterate through all slash patterns the enemy has
                for (int i = 0; i < enemy.GetComponent<Enemy>().slashPatterns.Count; i++)
                {
                    // Checks if cut type corresponds to at least one of Enemy's slash patterns
                    int slashPattern = enemy.GetComponent<Enemy>().slashPatterns[i];
                    int cutRequired = slashPattern;
                    if (cutType == cutRequired)
                    {
                        // Check for the closest enemy to player
                        enemyDist = Vector3.Distance(enemy.gameObject.transform.position, player.transform.position);
                        if (enemyDist < corEnemyDist && enemy.GetComponent<Enemy>().isDead == false)
                        {
                            corEnemy = enemy;
                            corEnemyDist = enemyDist;
                            hitEnemy = true;
                        }
                        break;
                    }
                }
            }

            // Check if an enemy was hit
            if (hitEnemy == true)
            {
                // Damage corresponding enemy
                corEnemy.GetComponent<Enemy>().TakeDamage(20, cutType);
                enemyLoc = corEnemy.GetComponent<Enemy>().location;
            }
            else
            {
                // Reset multiplier
                ScoreSetter.multiplier = 1;
            }
        }
        
        // Play attack animation
        OrientatePlayer(enemyLoc);
        animator.SetTrigger("Slash");
    }


    // Draws Attack Range For SCENE VIEW ONLY
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    /* Player is hit */
    public void TakeDamage(int damage, char enemyLoc)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // Play hurt animation
        if (animator.gameObject.activeSelf)
        {
            OrientatePlayer(enemyLoc);
            animator.SetTrigger("Hit");
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        ScoreSetter.multiplier = 1;
    }

    // Determines whether or not player is hit
    void OnTriggerEnter2D(Collider2D collision)
    {
        int enemyCollisions = collision.gameObject.GetComponent<Enemy>().numOfCollisions;
        bool enemyDead = collision.gameObject.GetComponent<Enemy>().isDead;
        if (collision.gameObject.tag == "Enemy" && enemyCollisions == 2 && !enemyDead)
        {
            char enemyLoc = collision.gameObject.GetComponent<Enemy>().location;
            TakeDamage(20, enemyLoc);
        }
    }
    /* Player is hit */

    // Player dies
    void Die()
    {
        // Show game over screen
        LevelOver.win = false;
        SceneManager.LoadScene("LevelOver");
    }
}