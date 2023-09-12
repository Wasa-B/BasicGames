using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WasabiGame;
using System.Linq;
using System.Reflection;
using UnityEngine.U2D.Animation;
using System;

namespace CartHero
{


    public partial class PlayerCharacter : ObjectBehaviour,IDamagedObject
    {

        static PlayerCharacter instance;
        public PlayerStatus status;
        public GunStatus weaponAddStatus;

        PlayerWeaponStatus totalStatus = new PlayerWeaponStatus();
        List<PlayerItemBase> items = new List<PlayerItemBase>();

        public GunCommand weapon;
        public AmmoUI ammoUI;
        public ExpUI expUI;

        public SpriteLibraryAsset front;
        public SpriteLibraryAsset back;
        SpriteLibrary spriteLibrary;

        public event Action hitActionHdr;

        protected override void Awake()
        {
            instance = this;
            base.Awake();
            spriteLibrary = GetComponent<SpriteLibrary>();
            totalStatus.AddItems(weaponAddStatus);
            totalStatus.LayerMask = weaponAddStatus.LayerMask;
        }
        public void Initialize()
        {
            ItemSelectUI.selectedItems = items;
            WeaponClone();
            GetComponent<PlayerInput>().input += MoveStart;
            GetComponent<PlayerInput>().inputStay += GetComponent<WeaponControl>().WeaponUse;

            GetComponent<WeaponControl>().SetWeapon(weapon);
            weapon.reloadAction += GetComponent<ReloadUI>().BarUpdate;

            moveCommand.end += MoveEnd;
        }
        private void Update()
        {
            if(weapon.dir.y > 0 && spriteLibrary.spriteLibraryAsset != front)
                spriteLibrary.spriteLibraryAsset = front;
            else if(weapon.dir.y < 0 && spriteLibrary.spriteLibraryAsset != back)
                spriteLibrary.spriteLibraryAsset = back;
        }
        private void Start()
        {
            
        }

        void GetExp(int exp) => status.Exp += exp;

        public void MoveEnd(Vector2 pos)
        {
            Invoke(nameof(MoveStart), .02f);
        }
        public void MoveStart()
        {
            moveCommand.MoveTo(Vector2.zero);
        }

        void WeaponClone()
        {
            weapon = weapon.Clone(this.gameObject) as GunCommand;
            totalStatus.AddItems(weapon.originStatus as GunStatus);
            weapon.SetStatus(totalStatus);
        }
        public static Vector2 GetPosition() => instance.transform.position;

        public void AddItem(PlayerItemBase item)
        {
            items.Add(item);
            totalStatus.AddItems(item.weaponStatus);
        }

        public void Damage(AttackStatus weaponStatuses)
        {
            
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {

            }
        }
    }
}