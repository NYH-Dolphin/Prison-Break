﻿using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        private Material _mat; // require material "2d Sprite Glow"
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        private static readonly int GlowColor = Shader.PropertyToID("_GlowColor");
        private Transform player;
        public float startCool;
        private float cool;
        private AudioControl SFX;
        public Animator anim;
        public Collider hitbox;


        private void Awake()
        {
            _mat = sr.material;
            OnNotSelected();
        }

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            cool = startCool;
            hitbox.enabled = false;
            SFX = GameObject.Find("AudioController").GetComponent<AudioControl>();
        }

        void Update()
        {
            cool -=Time.deltaTime;
            if(cool <= 0 && Vector3.Distance(transform.position, player.position) <= 5f)
            {
                anim.SetBool("attacking", true);
                hitbox.enabled = true;
                cool = startCool;
            }
            else{
                anim.SetBool("attacking", false);
            }

            if(AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName("GuardStill"))
            {
                hitbox.enabled = false;
            }
        }

        bool AnimatorIsPlaying(){
            return anim.GetCurrentAnimatorStateInfo(0).length > anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        /// <summary>
        /// Weapon is selected and ready for grabbing
        /// </summary>
        public void OnSelected()
        {
            _mat.SetColor(GlowColor, Color.red);
            _mat.SetFloat(OutlineWidth, 0.1f);
        }

        public void OnNotSelected()
        {
            _mat.SetFloat(OutlineWidth, 0);
        }
        
        
        // TODO Current directly make enemy dead after being hit
        public void OnHit()
        {
            SFX.PlayHit();
            Destroy(gameObject);
        }
    }
}