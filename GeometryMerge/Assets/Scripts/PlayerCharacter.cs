using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WasabiGame;

namespace CartHero
{

    public class PlayerCharacter : ObjectBehaviour
    {
        static PlayerCharacter instance;
        public PlayerStatus status;
        public AttackStatus weaponAddStatus;
        public WeaponCommand weapon;
        public AmmoUI ammoUI;
        public ExpUI expUI;

        protected override void Awake()
        {
            instance = this;
            base.Awake();

            
            //weapon.ammoUpdate += ammoUI.UpdateCount;
            //weapon.attackAction += MoveStart;

            //EnemyControl.expUp += GetExp;
            //status.expUpdate += expUI.ExpUpdate;

            //status.Initialize();
        }
        public void Initialize()
        {
            WeaponClone();
            GetComponent<PlayerInput>().input += MoveStart;
            GetComponent<PlayerInput>().inputStay += GetComponent<WeaponControl>().WeaponUse;

            GetComponent<WeaponControl>().SetWeapon(weapon);
            weapon.reloadAction += GetComponent<ReloadUI>().BarUpdate;

            moveCommand.end += MoveEnd;
        }
        private void Update()
        {

        }
        private void Start()
        {
            
        }

        void GetExp(int exp) => status.Exp += exp;

        public void MoveEnd(Vector2 pos)
        {
            Invoke(nameof(MoveStart),.02f);
        }
        public void MoveStart()
        {
            moveCommand.MoveTo(Vector2.zero);
        }
        
        void WeaponClone()
        {
            weapon = weapon.Clone(this.gameObject) as WeaponCommand;
            weapon.weaponStatus.characterStatus = weaponAddStatus;

        }
        public static Vector2 GetPosition()=>instance.transform.position;
    }
}