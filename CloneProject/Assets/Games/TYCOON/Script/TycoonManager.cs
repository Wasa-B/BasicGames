using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace TYCOON
{
    [System.Serializable]
    public class PlayData
    {
        public int gold;
        public float rating;
        public List<Item> items;

    }

    [System.Serializable]
    public struct EventBox<T>
    {
        public UnityEvent<T> events;
        public UnityEvent<string> textEvents;

        public void Update(T _value)
        {
            events?.Invoke(_value);
            textEvents?.Invoke(_value.ToString());
        }
    }

    public class TycoonManager : MonoBehaviour
    {

        [SerializeField]
        List<ShopTable> tables = new List<ShopTable>();
        


        PlayData playData = new PlayData();


        internal List<Item> Items {get => playData.items;}
        internal int gold { get=>playData.gold;}

        internal float rating { get => playData.rating;}


        public EventBox<int> goldEventBox;
        public EventBox<float> ratingEventBox;

        private void Awake()
        {
            
        }
        private void Start()
        {

            goldEventBox.Update(gold);
            ratingEventBox.Update(rating);
        }

        public void AddGold(int amount)
        {
            playData.gold += amount;
            playData.rating += .5f;

            goldEventBox.Update(gold);
            ratingEventBox.Update(rating);
        }
    }

}