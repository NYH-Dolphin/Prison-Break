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

        //private Navigation nav;
        private NewNav newNav;
        private int health = 2;
        public Transform deadGuard;


        private void Awake()
        {
            _mat = sr.material;
        }

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            cool = startCool;
            SFX = GameObject.Find("AudioController").GetComponent<AudioControl>();
            newNav = this.GetComponent<NewNav>();
        }

        void Update()
        {
            cool -= Time.deltaTime;
            if (cool <= 0 && Vector3.Distance(transform.position, player.position) <= attackingRange && notStunned &&
                newNav.chasing)
            {
                if (!newNav.unconscious)
                    anim.SetBool("attacking", true);
                cool = startCool;
            }

            else
            {
                // Debug.Log(Vector3.Distance(transform.position, player.position));
                if (!newNav.unconscious) anim.SetBool("attacking", false);

                else
                {
                    if (!newNav.unconscious) anim.SetBool("attacking", false);
                }
            }

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


        public void OnHit(int hit, bool melee)
        {
            Debug.Log("enemy health:" + health);
            SFX.PlayHit();
            if (hit == 1)
                StartCoroutine(DecreaseHealth());


            if (hit == 2 || (!notStunned && hit == 1) || health <= 0)
            {
                var dead = Instantiate(deadGuard,
                    new Vector3(this.transform.position.x, 0.5f, this.transform.position.z), Quaternion.identity);
                dead.transform.eulerAngles = new Vector3(0, 90, 0);
                if (melee)
                {
                    Knockback kb = dead.GetComponent<Knockback>();
                    PlayerController Pc = GameObject.Find("[Player]").GetComponent<PlayerController>();
                    kb.PlayFeedback(Pc.VecDir.normalized);
                }

                Destroy(gameObject);
            }
        }

        public Transform ActiveAttackPoint()
        {
            Vector3 directionToTarget = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if(angle < 22.5)
                return transform.GetChild(4).GetChild(4);
            else if (angle < 67.5)
            {
                if(directionToTarget.x <= 0)
                    return transform.GetChild(4).GetChild(5);
                else
                    return transform.GetChild(4).GetChild(3);
            }
            else if (angle < 112.5)
            {
                if(directionToTarget.x <= 0)
                    return transform.GetChild(4).GetChild(6);
                else
                    return transform.GetChild(4).GetChild(2);
            }
            else if (angle < 157.5)
            {
                if(directionToTarget.x <= 0)
                    return transform.GetChild(4).GetChild(7);
                else
                    return transform.GetChild(4).GetChild(1);
            }
            else
                return transform.GetChild(4).GetChild(0);
        }
        


        public void OnHitBlunt()
        {
            SFX.PlayHit();
            StartCoroutine(Blunt());
        }


        public bool bExecution => !notStunned && _bExecutable;

        private bool _bExecutable;

        private IEnumerator Blunt()
        {
            notStunned = false;
            StartCoroutine(BluntCountDown(0.8f));
            newNav.Stunned(stunTime);
            //dizzy.enabled = true;
            anim.SetTrigger("stunned");
            anim.SetBool("wake up", false);
            yield return new WaitForSeconds(stunTime);
            notStunned = true;
            dizzy.enabled = false;
            anim.ResetTrigger("stunned");
            anim.SetBool("wake up", true);
        }

        private IEnumerator BluntCountDown(float time)
        {
            _bExecutable = false;
            yield return new WaitForSeconds(time);
            _bExecutable = true;
        }

        private IEnumerator DecreaseHealth()
        {
            yield return new WaitForSeconds(0.2f);
            health--;
        }
    }
}