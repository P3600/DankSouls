using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FantasyHeroes.Scripts {

    public class PlayerController : MonoBehaviour {

        [Header("Player Information")]
        public string playerName;
        [Header("Stats")]
        public int swordSkill = 1;
        public int fitness = 1;
        public int mageTr = 1;
        public int lootTr = 1;
        public int gold;
        public int weaponDamage;
        public string weaponType;
        public int spellDamage;
        public int spellSpeed;
        public int hp;
        public int mp;
        public int armor;
        public int swordSPrice;
        public int armorPrice;
        public int mageTrPrice;
        public int lootTrPrice;
        [Header("Movement")]
        public int walkSpeed;
        public int jumpHeight;
        public int rollLength;

        private GameObject character;
        private AnimationController animationController;
        private Animator animator;
        private bool moveB = false;
        private Vector3 stopPos;
        private float movement;
        private string direction;
        private string mode;
        private string move;
        private bool jumping = false;
        private bool attacking = false;

        void Start() {
            character = GameObject.Find("BasicHero");
            animationController = GameObject.Find("PlayerController").GetComponent<AnimationController>();
            animator = GameObject.Find("Animation").GetComponent<Animator>();
            stopPos = character.transform.position;
            CheckWeapon();
            if(weaponType == "Melee1H" || weaponType == "Melee2H")
            {
                //DOES NOT WORK TO DO TODO
                GameObject.Find("MeleeWeapon").AddComponent<WeaponScript>();
            }
        }

        void Update()
        {
            character.transform.eulerAngles = new Vector3(0, 0, 0);
            if (moveB)
            {
                Vector3 ctp = character.transform.position;
                character.transform.position = new Vector3(ctp.x + movement, ctp.y);
            }
            else
            {
                character.transform.position = new Vector3(stopPos.x, character.transform.position.y);
            }
            if (character.GetComponent<Rigidbody2D>().velocity.y == 0 && jumping)
            {
                jumping = false;
                //animator.speed = 1;
                //animationController.PlayAnimation("Stand", character.GetComponent<Character>());
            }
            if(jumping == false && character.GetComponent<Rigidbody2D>().velocity.y == 0 && !attacking && !moveB)
            {
                animationController.Reset(character.GetComponent<Character>());
            }
        }

        public void Attack()
        {
            if (!attacking)
            {
                attacking = true;
                animationController.PlayAnimation("Attack", character.GetComponent<Character>());
                Invoke("StopAttack", 1);
            }
        }

        public void StopAttack()
        {
            CancelInvoke("StopAttack");
            animationController.Reset(character.GetComponent<Character>());
            attacking = false;
        }

        public void Jump()
        {
            if(!jumping)
            {
                StopAttack();
                character.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 650, 0);
                animationController.PlayAnimation("Jump", character.GetComponent<Character>());
                jumping = true;
                //Invoke("PauseAnimation", 0.49f);
            }
        }

        /*public void PauseAnimation()
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                print("Freeze");
                animator.speed = 0;
            }
        }*/

        public void MoveChar(string dm)
        {
            string[] chars = dm.Split(';');
            direction = chars[0];
            mode = chars[1];
            movement = walkSpeed;
            switch (direction)
            {
                case "left":
                    movement = movement * -1;
                    if(character.transform.localScale.x > 0)
                    {
                        FlipChar();
                    }
                    if (!jumping) { animationController.PlayAnimation("Run", character.GetComponent<Character>()); }
                    break;
                case "right":
                    if (character.transform.localScale.x < 0)
                    {
                        FlipChar();
                    }
                    if (!jumping) { animationController.PlayAnimation("Run", character.GetComponent<Character>()); }
                    break;
            }
            if (mode == "run")
            {
                movement = movement * 1.5f;
            }
            moveB = true;
        }

        public void FlipChar()
        {
            var scale = character.transform.localScale;
            scale.x *= -1;
            character.transform.localScale = scale;
        }

        public void StopMoving()
        {
            stopPos = character.transform.position;
            moveB = false;
            animationController.Reset(character.GetComponent<Character>());
        }

        public void CheckWeapon()
        {
            switch (character.GetComponent<Character>().WeaponType)
            {
                case WeaponType.Melee1H:
                    weaponType = "Melee1H";
                    break;
                case WeaponType.Melee2H:
                    weaponType = "Melee2H";
                    break;
                case WeaponType.MeleeTween:
                    weaponType = "MeleeTween";
                    break;
                case WeaponType.Bow:
                    weaponType = "MeleeBow";
                    break;
            }
        }

        public void IncreaseStat(string name)
        {
            if (name == "swordSkill")
            {
                swordSkill++;
            }
            if (name == "armor")
            {
                armor++;
            }
            if (name == "mageTr")
            {
                mageTr++;
            }
            if (name == "lootTr")
            {
                lootTr++;
            }
        }
    }
}
