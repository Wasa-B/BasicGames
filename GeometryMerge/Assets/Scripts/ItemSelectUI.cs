using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WasabiGame;

namespace CartHero
{


    public class ItemSelectUI : MonoBehaviour
    {

        internal static List<PlayerItemBase> items = new List<PlayerItemBase>();
        internal static PlayerItemBase[] baseItem;
        internal static List<PlayerItemBase> selectedItems;

        public static event System.Action<PlayerItemBase> Selected;

        PlayerItemBase[] generatedItem = new PlayerItemBase[3];

        public GameObject pannel;
        public Image[] itemImageUI;


        public void StartSelect(int level = 1)
        {
            if (level < 1) return;
            Debug.Log($"Level {level}");
            GenerateItem();
            pannel.SetActive(true);
            Time.timeScale = 0;
        }

        public void GenerateItem()
        {
            FillBaseItems();
        }

        void FillBaseItems()
        {

            for (int i = 0; i < generatedItem.Length; i++)
            {
                if(generatedItem[i] == null)
                {
                    generatedItem[i] = baseItem[i];
                }
            }
            
        }

        public void SelectItem(int index)
        {
            Selected?.Invoke(generatedItem[index]);
            pannel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}