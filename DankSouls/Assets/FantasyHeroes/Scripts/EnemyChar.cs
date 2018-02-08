using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FantasyHeroes.Scripts
{
    public class EnemyChar : MonoBehaviour
    {
        //EnemyChar Original
        public int life;
        public int damage;
        public float attackSpeed;
        public float attackRange;
        public float walkSpeed;

        private GameObject enemyChar;

        //taken from PlayerController - needs filtering
        private GameObject character;
        private AnimationController animationController;
        private Animator animator;
        private bool moveB = true;
        private Vector3 stopPos;
        private float movement;
        private string direction;
        private string mode;
        private string move;
        private bool jumping = false;
        private bool attacking = false;
        private bool attackAnim = false;
        private float handleRange;

        // Use this for initialization
        void Start()
        {
            enemyChar = gameObject.transform.parent.transform.parent.gameObject;
            stopPos = enemyChar.transform.position;
            character = GameObject.Find("BasicHero");
            animationController = GameObject.Find("PlayerController").GetComponent<AnimationController>();
            handleRange = attackRange + 5;
        }

        // Update is called once per frame
        void Update()
        {
            enemyChar.transform.eulerAngles = new Vector3(0, 0, 0);
            CheckRange();
            if (moveB)
            {
                AproachCharacter();
            }
            else
            {
                enemyChar.transform.position = new Vector3(stopPos.x, enemyChar.transform.position.y);
            }
            CheckDirection();
        }

        void CheckRange()
        {
            float range;
            bool hasNegative = false;
            float cPos = character.transform.position.x;
            float ePos = enemyChar.transform.position.x;
            if(cPos > ePos) { direction = "right"; } else { direction = "left"; }
            if(cPos < 0) { cPos *= -1; hasNegative = !hasNegative; }
            if(ePos < 0) { ePos *= -1; hasNegative = !hasNegative; }
            if (hasNegative) { range = cPos + ePos; hasNegative = false; }
            else
            if(cPos > ePos) { range = cPos - ePos; } else { range = ePos - cPos; }
            if(range < attackRange)
            {
                if (attacking && attackAnim)
                {
                    attackAnim = false;
                    animationController.PlayAnimation("Stand", enemyChar.GetComponent<Character>());
                }
                else
                if (!attacking) {; Attack(); }
            } else { 
                moveB = true;
                Vector3 ctp = enemyChar.transform.position;
                enemyChar.transform.position = new Vector3(ctp.x + movement, ctp.y);
                stopPos = enemyChar.transform.position;
            }
            if (attacking) { attackRange = handleRange; } else { attackRange = handleRange - 5; }
        }

        void Attack()
        {
            moveB = false;
            attacking = true;
            animationController.PlayAnimation("Attack", enemyChar.GetComponent<Character>());
            Invoke("AttackCooldown", attackSpeed);
            Invoke("AAnimCancel", 0.5f);
        }

        void AttackCooldown()
        {
            attacking = false;
        }

        void AAnimCancel()
        {
            attackAnim = true;
        }

        void AproachCharacter()
        {
            mode = "Walk";
            animationController.PlayAnimation("Walk", enemyChar.GetComponent<Character>());
            if (mode == "run")
            {
                movement = movement * 1.5f;
            }
        }

        void CheckDirection()
        {
            movement = walkSpeed;
            switch (direction)
            {
                case "left":
                    movement = movement * -1;
                    if (enemyChar.transform.localScale.x > 0)
                    {
                        FlipChar();
                    }
                    break;
                case "right":
                    if (enemyChar.transform.localScale.x < 0)
                    {
                        FlipChar();
                    }
                    break;
            }
        }

        public void FlipChar()
        {
            var scale = enemyChar.transform.localScale;
            scale.x *= -1;
            enemyChar.transform.localScale = scale;
        }
    }
}
