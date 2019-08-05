using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Skill {
    
    private Rigidbody2D myRigidBody2D;
    private Vector2 direction;

    private void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();        
    }

    public override void Action()
    {
        base.Action();

        direction = (target != null)? target.CenterPos - transform.position : mousePos -transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {           
        myRigidBody2D.velocity = direction.normalized * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Instantiate(ImpactEffect.gameObject, transform.position, Quaternion.identity);
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
