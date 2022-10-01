using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    // Parent class.
    public abstract class Collectable : MonoBehaviour
    {

        public float value;
        
        public Sprite sprite;
        protected SpriteRenderer _sr;
        protected Rigidbody2D _rb;
        protected Player _player;
        
        protected abstract void OnCollectablePickup();
        
        // Start is called before the first frame update
        void Start()
        {
            _player = FindObjectOfType<Player>();
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

        

    }
}
