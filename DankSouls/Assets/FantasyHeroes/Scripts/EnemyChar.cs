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

        // Use this for initialization
        void Start()
        {
            print(gameObject.transform.parent.transform.parent.gameObject);
            enemyChar = gameObject.transform.parent.transform.parent.gameObject;
            stopPos = enemyChar.transform.position;
            character = GameObject.Find("BasicHero");
            animationController = GameObject.Find("PlayerController").GetComponent<AnimationController>();
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
        }

        void CheckRange()
        {
            float range;
            float cPos = character.transform.position.x;
            float ePos = enemyChar.transform.position.x;
            if(cPos > ePos) { direction = "right"; } else { direction = "left"; }
            if(cPos < 0) { cPos *= -1; }
            if(ePos < 0) { ePos *= -1; }
            if(cPos > ePos) { range = cPos - ePos; } else { range = ePos - cPos; }
            if(range < attackRange)
            {
                Attack();
            } else {
                moveB = true;
                Vector3 ctp = enemyChar.transform.position;
                enemyChar.transform.position = new Vector3(ctp.x + movement, ctp.y);
                stopPos = enemyChar.transform.position;
            }
        }

        void Attack()
        {
            moveB = false;

        }

        void AproachCharacter()
        {
            mode = "Walk";
            movement = walkSpeed;
            switch (direction)
            {
                case "left":
                    movement = movement * -1;
                    if (enemyChar.transform.localScale.x > 0)
                    {
                        FlipChar();
                    }
                    animationController.PlayAnimation("Run", enemyChar.GetComponent<Character>());
                    break;
                case "right":
                    if (enemyChar.transform.localScale.x < 0)
                    {
                        FlipChar();
                    }
                    animationController.PlayAnimation("Run", enemyChar.GetComponent<Character>());
                    break;
            }
            if (mode == "run")
            {
                movement = movement * 1.5f;
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
