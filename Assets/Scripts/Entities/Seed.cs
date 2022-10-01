using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;

public class Seed : Enemy
{
    public Projectile projectile;
    public float projectileSpeed;
    

    //Only runs in "Dark Mode"
    protected override void EnemyMovement()
    {
        //Move to linearly
        
        //Get direction of player
        Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
        
        //Move to player
        transform.position +=  moveSpeed * Time.deltaTime * directionToPlayer;
        
    }
    
    //Range Attack
    protected override void Attack()
    {
        //Player take damage
        Debug.Log(enemyName + " Attack Player!");
        Projectile newProjectile = Instantiate(projectile);
        newProjectile.ShootTarget(_player.transform.position, gameObject, projectileSpeed, attackDamage);

    }
}
