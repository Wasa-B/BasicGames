using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WasabiGame;

namespace CartHero
{

    public class PlayerItem : ScriptableObject
    {
        AttackStatus weaponStatus;
        PlayerStatus playerStatus;
    }

    public class CartHeroManager : MonoBehaviour
    {
        public ExpUI expUI;
        public AmmoUI ammoUI;

        public PlayerCharacter character;
        public EnemyGenerator enemyGenerator;
        // Start is called before the first frame update



        public void Awake()
        {
            character.Initialize();
            character.weapon.ammoUpdate += ammoUI.UpdateCount;
            EnemyControl.expUp += character.status.ExpUp;


            character.status.expUpdate += expUI.ExpUpdate;
            

            character.status.Initialize();
        }



        private void FixedUpdate()
        {
            
        }

    }

}