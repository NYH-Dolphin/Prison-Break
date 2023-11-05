﻿using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class ThrowableWeaponBehaviour : WeaponBehaviour
    {
        public override void OnAttack(Transform startTransform, Transform targetTransform)
        {
            ThrowBehaviour(startTransform, targetTransform);
        }
        
        private void ThrowBehaviour(Transform startTransform, Transform targetTransform)
        {
            iTween.Init(gameObject);
            Vector3[] path = new Vector3[3];
            Vector3 startPosition = startTransform.position;
            Vector3 targetPosition = targetTransform.position;
            Vector3 midPosition = (startPosition + targetPosition) / 2.0f;
            midPosition.y += 1f;
            path[0] = startPosition;
            path[1] = midPosition;
            path[2] = targetPosition;
            Hashtable args = new Hashtable();
            args.Add("position", targetPosition);
            args.Add("path", path);
            args.Add("time", 0.5f);
            args.Add("easetype", iTween.EaseType.easeOutQuart);
            iTween.MoveTo(gameObject, args);
            StartCoroutine(DestroyCountDown(0.6f)); // Start this countdown in case weapon doesn't hit the enemy
        }
        
        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}