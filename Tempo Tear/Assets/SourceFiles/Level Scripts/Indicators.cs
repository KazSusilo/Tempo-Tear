using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    private float speed = .33f;

    // Initialization
    private void Start()
    {
        // Set indicators to black if on level 1
        if (Level.level == 1)
        {
            this.GetComponent<SpriteRenderer>().material.color = new Color(0, 0, 0);
            speed = .33f;
        }
        else if (Level.level == 2)
        {
            speed = .34f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move indicator until directly above diamond
        if (transform.position.x != 0 && Level.level != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(GameObject.Find("Indicator").transform.position.x,
            transform.position.y, transform.position.z), speed * Time.deltaTime);
        } 
    }


    // Beat was hit successfully
    public void BeatHit()
    {
        // Increase score & multipler depending on how on beat they are
        if (transform.position.x <= .033f && transform.position.x >= -.033f)
        {
            ScoreSetter.score += 300 * ScoreSetter.multiplier;
            ScoreSetter.multiplier++;
        }
        else if (transform.position.x <= .066f && transform.position.x >= -.066f)
        {
            ScoreSetter.score += 100 * ScoreSetter.multiplier;
            ScoreSetter.multiplier++;
        }
        else if (transform.position.x <= .099f && transform.position.x >= -.099f)
        {
            ScoreSetter.score += 50 * ScoreSetter.multiplier;
            ScoreSetter.multiplier++;
        }
        else
        {
            ScoreSetter.score += 10;
            ScoreSetter.multiplier = 1;
        }
        DeleteIndicator();
    }


    // Deletes indicator from screen
    public void DeleteIndicator()
    {
        Destroy(gameObject);
    }
}
