using System.Collections;
using System.Collections.Generic;
using MyCameraEffect;
using UnityEngine;
using Player;

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
        private NewNav newNav;
        private int health = 2;
        public Transform deadGuard;
        public GameObject bloodyPrefab;


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
            if (cool <= 0 && Vector3.Distance(transform.position, player.position) <= attackingRange && notStunned)
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
            SFX.PlayHit();
            

            CameraEffect.Instance.GenerateMeleeImpulse();


            if (hit == 1)
            {
                StartCoroutine(DecreaseHealth());
            }

            if (hit == 2 || (!notStunned && hit == 1) || health <= 0)
            {
                Transform spawnpoint = transform.GetChild(0).GetChild(0);
                var dead = Instantiate(deadGuard,
                    new Vector3(spawnpoint.position.x, 0.5f, spawnpoint.position.z), Quaternion.identity);
                dead.transform.eulerAngles = new Vector3(0, 90, 0);
                if(melee) dead.GetComponent<Knockback>().PlayFeedback(player.GetComponent<PlayerController>().VecDir.normalized);
                ExecutionEffects.Instance.Execution();
                ViewCone.Instance.DeRegister(GetComponent<Collider>());
                Instantiate(bloodyPrefab, new Vector3(transform.position.x, 0.1f, transform.position.z),
                    Quaternion.Euler(new Vector3(90, 0, 0)));
                Destroy(gameObject);
            }
        }

        public Transform ActiveAttackPoint()
        {
            //if (!notStunned) return transform.GetChild(0).transform;
            Vector3 directionToTarget = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if (angle < 22.5)
                return transform.GetChild(3).GetChild(4);
            else if (angle < 67.5)
            {
                if (directionToTarget.x <= 0)
                    return transform.GetChild(3).GetChild(5);
                else
                    return transform.GetChild(3).GetChild(3);
            }
            else if (angle < 112.5)
            {
                if (directionToTarget.x <= 0)
                    return transform.GetChild(3).GetChild(6);
                else
                    return transform.GetChild(3).GetChild(2);
            }
            else if (angle < 157.5)
            {
                if (directionToTarget.x <= 0)
                    return transform.GetChild(3).GetChild(7);
                else
                    return transform.GetChild(3).GetChild(1);
            }
            else
                return transform.GetChild(3).GetChild(0);
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
            SFX.PlayDizzy();
            notStunned = false;
            StartCoroutine(BluntCountDown(.4f));
            newNav.Stunned(stunTime);
            dizzy.enabled = true;
            float direction = DirectionSwitcher();
            anim.SetFloat("direction", direction);
            Debug.Log(anim.GetFloat("direction"));
            anim.SetTrigger("stunned");
            anim.SetBool("wake up", false);
            player.GetComponent<PlayerWeapon>().downedEnemies.Add(this.gameObject);

            yield return new WaitForSeconds(stunTime);

            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            notStunned = true;
            dizzy.enabled = false;
            anim.ResetTrigger("stunned");
            anim.SetBool("wake up", true);
            player.GetComponent<PlayerWeapon>().downedEnemies.Remove(this.gameObject);
        }

        private float DirectionSwitcher()
        {
            if (player.GetComponent<PlayerWeapon>().attkPos == null)
                return 1;
            switch (player.GetComponent<PlayerWeapon>().attkPos.gameObject.name)
            {
                case "West":
                    return 1;
                case "East":
                    return 2;
                case "North":
                    return 3;
                case "South":
                    return 4;
                case "SouthWest":
                    return 5;
                case "SouthEast":
                    return 6;
                case "NorthWest":
                    return 7;
                case "NorthEast":
                    return 8;
                default:
                    return 1;
            }

            return 1;
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