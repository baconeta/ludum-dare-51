using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{

    //Only runs in "Dark Mode"
    protected override void EnemyMovement()
    {
        //Move to linearly
        
        //Get direction of player
        Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
        
        //Move to player
        transform.position +=  moveSpeed * Time.deltaTime * directionToPlayer;
    }
}
