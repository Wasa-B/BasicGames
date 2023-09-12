using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtility;
using WPooling;
using WDefense;


[System.Serializable]
class GameInfo
{
    public static GameInfo instance;

    public GameObject selectedCharacter;
    public int stageTime = 0;
    public int stageExp = 0;
    public int stageLevel = 1;

    public int stage = -1;

}

public class GameControl : MonoBehaviour
{
    public event System.Action<int> ExpUpdateEvent;
    public event System.Action<int> UpdateTime;

    [SerializeField]
    List<int> expTable;
    [SerializeField]
    GameInfo gameInfo;

    public virtual int TestData { get; protected set; } = 1;

    public Data<int> stageTime;
    public Data<int> exp;

    float _ctime = 0;
    static public GameObject GenerateObject(GameObject gameObject)
    {
        return PoolManager.instance.GetGameObject(gameObject);
    }

    static public void RemveObject(GameObject gameObject)
    {
        PoolManager.instance.ReturnObject(gameObject);
    }



    private void Awake()
    {
        stageTime.OnChange.AddListener((v) => gameInfo.stageTime = v);
        exp.OnChange.AddListener((v) => gameInfo.stageExp = v);

        WDefenseUtility.Generate = GenerateObject;
        WDefenseUtility.Delete = RemveObject;
        WDefenseUtility.blockLayerMask = 1 << 8;


    }

    private void FixedUpdate()
    {
        TimeCycle();
    }

    void TimeCycle()
    {
        _ctime += Time.fixedDeltaTime;
        if (_ctime >= .01f)
        {
            stageTime.value += 1;

            _ctime -= .01f;
        }
    }

    public void AddExp(int exp)
    {
        this.exp.value += exp;
    }

    public void AddWeapon(Weapon weapon)
    {
        CharacterControl.Instance.AddWeapon(weapon);
    }
}
