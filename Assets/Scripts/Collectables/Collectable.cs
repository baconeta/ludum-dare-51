using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    // Parent class.
    public class Collectable : MonoBehaviour
    {
        
        public int value;

        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        
        // Start is called before the first frame update
        void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                OnCollectablePickup();
            }
        }

        public void OnCollectablePickup()
        {
            //Do effect
            
            //Consume
            Destroy(gameObject);
        }
    }
}
