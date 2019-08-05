using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObstacleScanner : MonoBehaviour {
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle obstacle = collision.GetComponent<Obstacle>();
            if (transform.position.y > collision.transform.position.y)
                obstacle.FadeOut();
            else
                obstacle.FadeIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle obstacle = collision.GetComponent<Obstacle>();
            obstacle.FadeIn();
        }
    }
}
