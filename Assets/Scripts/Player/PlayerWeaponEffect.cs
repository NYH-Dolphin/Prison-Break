using UnityEngine;
using Weapon;

namespace Player
{
    [RequireComponent(typeof(PlayerWeapon))]
    public class PlayerWeaponEffect : MonoBehaviour
    {
        [Header("Weapon Effects")] 
        [SerializeField]private GameObject objBoomerangWeaponEffect;
        [SerializeField] private GameObject objLobRange;
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
                objBoomerangWeaponEffect.SetActive(false);
                DisableLobPosition();
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
        
        public void DrawLobPosition(Vector3 position)
        {
            objLobRange.SetActive(true);
            Vector3 pos = position;
            pos.y += 0.1f;
            objLobRange.transform.position = pos;
        }

        private void SetBoomerangWeaponEffect(bool able)
        {
            objBoomerangWeaponEffect.SetActive(able);
        }
    }
}