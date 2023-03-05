using System.Collections.Generic;
using UnityEngine;
namespace SweetRoad
{
    [CreateAssetMenu(fileName = "CandyData", menuName = "SweetRoad/CandyData")]
    public class CandyData : ScriptableObject
    {
        [System.Serializable]
        public struct Status
        {
            public CandyType type;
            public Sprite sprite;
        }
        [System.Serializable]
        public struct SpecialStatus
        {
            public SpecialType type;
            public Sprite sprite;
        }
        public List<Status> candyStatus;
        public List<SpecialStatus> spCandyStatus;

        public Dictionary<CandyType, Sprite> GetDictionary()
        {
            Dictionary<CandyType, Sprite> dic = new Dictionary<CandyType, Sprite>();
            foreach (var c in candyStatus)
            {
                dic.Add(c.type, c.sprite);
            }
            return dic;
        }

        public Dictionary<SpecialType, Sprite> GetSPDictionary()
        {
            Dictionary<SpecialType, Sprite> dic = new Dictionary<SpecialType, Sprite>();
            foreach (var c in spCandyStatus)
            {
                dic.Add(c.type, c.sprite);
            }
            return dic;
        }
    }
}