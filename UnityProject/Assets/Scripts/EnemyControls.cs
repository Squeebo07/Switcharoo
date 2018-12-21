using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : MonoBehaviour {
    public float moveSpeed; // the rotation of the enemy
    public float distance;      // the distance of enemy's sight
    Rigidbody2D enemyRigidbody; // enemy's rigidbody
    
    public GameManager getScript;

    public LineRenderer lineOfSight;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       // transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -transform.right, distance);
        if (hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            lineOfSight.SetPosition(1, hitInfo.point);
            if (hitInfo.collider.CompareTag("Player"))
            {
                getScript.EnemyPlayerDetected();
            }
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + -transform.right * distance, Color.green);
            lineOfSight.SetPosition(1, transform.position + -transform.right * distance);
        }

        lineOfSight.SetPosition(0, transform.position);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            getScript.EnemyPlayerDetected();
    }
}
