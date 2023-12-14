using UnityEngine;
using Weapon;

namespace Player
{
    [RequireComponent(typeof(PlayerWeapon))]
    public class PlayerWeaponEffect : MonoBehaviour
    {
        [Header("Weapon Effects")] [SerializeField]
        private GameObject objBoomerangWeaponEffect;

        [SerializeField] private GameObject objLobRange;
        [SerializeField] private GameObject objDirHint;
        [SerializeField] private GameObject objDirHintAngle;
        [SerializeField] private GameObject objLobWeaponEffectPrefab;


        private PlayerWeapon _pw;
        private GameObject _weaponEquipped;

        private void Start()
        {
            _pw = GetComponent<PlayerWeapon>();
        }


        private void Update()
        {
            if (_pw.WeaponEquipped == null)
            {
                objDirHint.SetActive(false);
                objLobRange.SetActive(false);
                objBoomerangWeaponEffect.SetActive(false);
            }
            else
            {
                switch (_pw.WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eAttackType)
                {
                    case AttackType.Boomerang:
                        if (!objBoomerangWeaponEffect.activeSelf) SetBoomerangWeaponEffect(true);
                        objBoomerangWeaponEffect.transform.position = _pw.WeaponEquipped.transform.position;
                        break;
                }
            }
        }


        public void OnCancelAllEffect()
        {
            objDirHint.SetActive(false);
            objLobRange.SetActive(false);
            objDirHintAngle.SetActive(false);
            objBoomerangWeaponEffect.SetActive(false);
        }

        public void PlayLobEffect(Vector3 position)
        {
            // Play the effect in the end, a little before the death calculation
            GameObject lobEffect = Instantiate(objLobWeaponEffectPrefab);
            lobEffect.transform.position = position;
        }

        public void DisableLobPosition()
        {
            objLobRange.SetActive(false);
        }

        public void DisableLobSprite()
        {
            objLobRange.transform.GetChild(0).gameObject.SetActive(false);
        }

        public void DrawLobPosition(Vector3 position)
        {
            objLobRange.SetActive(true);
            objLobRange.transform.GetChild(0).gameObject.SetActive(true);
            Vector3 pos = position;
            pos.y += 0.1f;
            objLobRange.transform.position = pos;
        }


        public void DrawDirHint(Vector3 dir)
        {
            objDirHint.SetActive(true);
            objDirHintAngle.SetActive(true);
            Vector3 pos = transform.position + dir * 3.5f;
            pos.y += 0.1f;

            // rotate the angle object based on the intersection angle
            float angle = Vector3.Angle(Vector3.forward, dir);
            if (dir.x > 0)
            {
                angle = -angle;
            }

            objDirHintAngle.transform.rotation = Quaternion.Euler(new Vector3(90, 0, angle));
            objDirHintAngle.transform.position = pos;
        }

        public void DisableDirHint()
        {
            objDirHint.SetActive(false);
        }

        private void SetBoomerangWeaponEffect(bool able)
        {
            objBoomerangWeaponEffect.SetActive(able);
        }
    }
}