using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Health & Slash Variables
    public int currentHealth;
    public List<int> slashPatterns;
    public List<GameObject> patternObjects;

    // Indicator Variables
    public List<GameObject> indicatorObjects;

    // Enemy Variables
    public int enemyNum;
    public int enemyType;

    // Movement Variables
    private float speed;

    // Combat Variables
    public bool isStunned = false;
    private bool inRange = false;
    public bool isDead = false;
    public int numOfCollisions;

    // Animator Variable
    public Animator animator;
    public char location = 'l';

    // Initialization
    void Awake()
    {
        int levelBPM = 0;
        // Set enemy's speed and location
        if (Level.level == 1)
        {
            levelBPM = 135;
        }
        else if (Level.level == 2)
        {
            levelBPM = 140;
        }
        speed = levelBPM / 60f;

        if (transform.position.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            location = 'r';
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Moves enemy
        if (isDead == false && inRange == false && isStunned == false && Level.level != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
            new Vector3(GameObject.Find("Player").transform.position.x, 
            transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
    }


    /* Enemy attacks player */
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        numOfCollisions++;
        if (collision.gameObject.tag == "Player")
        {
            inRange = true;
            Attack();
        }
    }
    /* Enemy attacks player */


    // Set enemy to no longer be stunned
    public void NotStunned() 
    { 
        isStunned = false;
        if (inRange == true)
        {
            Attack();
        }
    }


    // Enemy takes damage
    public void TakeDamage(int damage, int cutType)
    {
        for (int i = 0; i < slashPatterns.Count; i++)
        {
            // Checks if cut type matches at least one slash pattern
            int slashPattern = slashPatterns[i];
            if (cutType == slashPattern)
            {
                // Enemy loses Health
                currentHealth -= damage;
                animator.SetTrigger("Hurt");
                if (currentHealth <= 0)
                {
                    Die();
                }

                // Enemy loses slash pattern
                RemovePattern(slashPattern);
                isStunned = true;
                break;
            }
        }
    }


    // Enemy loses corresponding slash pattern and indicator
    void RemovePattern(int slashPattern)
    {
        for (int i = 0; i < slashPatterns.Count; i++)
        {
            if (slashPatterns[i] == slashPattern)
            {
                patternObjects[i].GetComponent<Slashes>().DeletePattern();
                patternObjects.RemoveAt(i);
                indicatorObjects[i].GetComponent<Indicators>().BeatHit();
                indicatorObjects.RemoveAt(i);
                break;
            }
        }
        slashPatterns.Remove(slashPattern);
    }


    // Enemy dies
    void Die()
    {
        // Disable enemy
        this.enabled = false;
        isDead = true;

        // Die animation
        animator.SetBool("IsDead", true);
    }


    // Deletes enemy from screen
    void DeleteEnemy()
    {
        for (int i = 0; i < indicatorObjects.Count; i++)
        {
            indicatorObjects[i].GetComponent<Indicators>().DeleteIndicator();
        }
        Destroy(gameObject);
    }
}
