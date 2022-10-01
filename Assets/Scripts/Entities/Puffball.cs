using System.Collections;
using UnityEngine;

namespace Entities
{
    public class Puffball : Enemy
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
        }

        
    
    
    }
}
