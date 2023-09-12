using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartHero
{

    public class CartHeroManager : MonoBehaviour
    {
        public static CartHeroManager instance;
        public ExpUI expUI;
        public AmmoUI ammoUI;
        public ItemSelectUI itemSelectUI;

        public PlayerCharacter character;
        public EnemyGenerator enemyGenerator;


        [SerializeField]
        List<PlayerItemBase> playerItems;
        [SerializeField]
        PlayerItemBase[] baseItems;
        // Start is called before the first frame update



        public void Awake()
        {
            instance = this;
            character.Initialize();
            character.weapon.ammoUpdate += ammoUI.UpdateCount;
            EnemyControl.expUp += character.status.ExpUp;
            EnemyControl.target = character.gameObject;

            character.status.expUpdate += expUI.ExpUpdate;
            character.status.levelUpdate += itemSelectUI.StartSelect;

            character.status.Initialize();
            ItemSelectUI.Selected += character.AddItem;
            ItemSelectUI.baseItem = baseItems;
            ItemSelectUI.items = playerItems;
        }



        private void FixedUpdate()
        {
            
        }

    }

}