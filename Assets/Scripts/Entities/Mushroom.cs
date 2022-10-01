using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class Mushroom : Enemy
{

    //Only runs in "Dark Mode"
    protected override void EnemyMovement()
    {
        //Move to linearly
        
        //Get direction of player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        
        //Move to player
        transform.position +=  moveSpeed * Time.deltaTime * directionToPlayer;
    }
    
    //Melee Attack
    protected override void Attack()
    {
        //Player take damage
        Debug.Log(enemyName + " Attack Player!");
    }
}
