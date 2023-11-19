using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float stunTime;
        [SerializeField] private SpriteRenderer dizzy;
        private Material _mat; // require material "2d Sprite Glow"
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        private static readonly int GlowColor = Shader.PropertyToID("_GlowColor");
        private Transform player;
        public float startCool;
        public float attackingRange;
        private float cool;
        private AudioControl SFX;
        public Animator anim;
        public bool notStunned = true;


        private void Awake()
        {
            _mat = sr.material;
            OnNotSelected();
        }

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            cool = startCool;
            SFX = GameObject.Find("AudioController").GetComponent<AudioControl>();
        }

        void Update()
        {
            cool -=Time.deltaTime;
            if(cool <= 0 && Vector3.Distance(transform.position, player.position) <= attackingRange && notStunned)
            {
                anim.SetBool("attacking", true);
                cool = startCool;
            }
            else{
                anim.SetBool("attacking", false);
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


        public void OnHitBlunt()
        {
            SFX.PlayHit();
            StartCoroutine(Blunt());
        }

        private IEnumerator Blunt()
        {
            notStunned = false;
            this.GetComponent<Navigation>().stunned = true;
            dizzy.enabled = true;
            //anim.SetTrigger("stunned");
            yield return new WaitForSeconds(stunTime);
            notStunned = true;
            this.GetComponent<Navigation>().stunned = false;
            dizzy.enabled = false;
            //anim.ResetTrigger("stunned");

        }
    }
}