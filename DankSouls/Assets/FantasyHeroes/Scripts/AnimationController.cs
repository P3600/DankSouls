using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Play animation from character editor
    /// </summary>
    public class AnimationController : MonoBehaviour
    {
        [Header("UI")]
        public Text ClipName;

        /// <summary>
        /// Called automatically on app start
        /// </summary>
        public void Start()
        {
            Reset(GameObject.Find("BasicHero").GetComponent<Character>());
            Reset(GameObject.Find("BasicEnemy").GetComponent<Character>());
        }

        /// <summary>
        /// Reset animation on start and weapon type change
        /// </summary>
        public void Reset(Character go)
        {
            PlayAnimation("Stand", go);
        }

        /// <summary>
        /// Change animation and play it
        /// </summary>
        /// <param name="direction">Pass 1 or -1 value to play forward / reverse</param>
        /// Stand; Alert; Attack; Attack Lunge; Cast; Walk; Run; Jump; Hit; Die; 
        
        public void PlayAnimation(string clipName, Character go, float timer = 0)
        {
            clipName = ResolveAnimatiobClip(clipName, go);
            
            go.Animator.SetTrigger("LoopAll");
            go.Animator.Play(clipName);
        }

        private string ResolveAnimatiobClip(string clipName, Character go)
        {
            switch (clipName)
            {
                case "Alert":
                    switch (go.WeaponType)
                    {
                        case WeaponType.Melee1H:
                        case WeaponType.MeleeTween:
                        case WeaponType.Bow: return "Alert1H";
                        case WeaponType.Melee2H: return "Alert2H";
                        default: throw new NotImplementedException();
                    }
                case "Attack":
                    switch (go.WeaponType)
                    {
                        case WeaponType.Melee1H: return "Attack1H";
                        case WeaponType.Melee2H: return "Attack2H";
                        case WeaponType.MeleeTween: return "AttackTween";
                        case WeaponType.Bow: return "Shot";
                        default: throw new NotImplementedException();
                    }
                case "AttackLunge":
                    switch (go.WeaponType)
                    {
                        case WeaponType.Melee1H:
                        case WeaponType.Melee2H:
                        case WeaponType.MeleeTween:
                        case WeaponType.Bow: return "AttackLunge1H";
                        default: throw new NotImplementedException();
                    }
                case "Cast":
                    switch (go.WeaponType)
                    {
                        case WeaponType.Melee1H:
                        case WeaponType.Melee2H:
                        case WeaponType.MeleeTween:
                        case WeaponType.Bow: return "Cast1H";
                        default: throw new NotImplementedException();
                    }
                default: return clipName;
            }
        }
    }
}