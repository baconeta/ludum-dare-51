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
            var moveValue = moveSpeed * Time.deltaTime * directionToPlayer;
            transform.position += moveValue;
            NotifyAnimator(moveValue);
        }

        //Melee Attack
        protected override void Attack()
        {
            //Player take damage
            player.GetPlayerCombat().DamagePlayer(attackDamage);
        }

        
    
    
    }
}
