using UnityEngine;
using Weapon;

namespace Player
{
    [RequireComponent(typeof(PlayerWeapon))]
    public class PlayerWeaponEffect : MonoBehaviour
    {
        [Header("Weapon Effects")] 
        [SerializeField]private GameObject objBoomerangWeaponEffect;
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
            }
            else
            {
                switch (_pw.WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eAttackType)
                {
                    case AttackType.Boomerang:
                        if (!objBoomerangWeaponEffect.activeSelf) SetBoomerangWeaponEffect(true);
                        objBoomerangWeaponEffect.transform.position = _pw.WeaponEquipped.transform.position;
                        break;
                    case AttackType.Lob:
                        _pw.WeaponEquipped.GetComponent<LobWeaponBehaviour>().RegisterPlayerWeaponEffect(this);
                        break;
                }
            }
        }

        public void PlayLobEffect(LobWeaponBehaviour behaviour, Vector3 position)
        {
            // Play the effect in the end, a little before the death calculation
            GameObject lobEffect = Instantiate(objLobWeaponEffectPrefab);
            lobEffect.transform.position = position;
            
        }


        private void SetBoomerangWeaponEffect(bool able)
        {
            objBoomerangWeaponEffect.SetActive(able);
        }
    }
}