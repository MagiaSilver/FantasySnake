using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "ListPlayerData", menuName = "GameData/ListPlayerData", order = 1)]
public class ListPlayerData : ScriptableObject
{
    #region singleton
    private static ListPlayerData instance;
    public static ListPlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load("ScriptableObject/ListPlayerData") as ListPlayerData;
            }
            return instance;

        }
    }
    #endregion
    //public int MaxLevel;
    [Header("Map_Size")]
    [SerializeField]
    public int x_Size; 
    public int y_Size;
    [Header("Data")]
    public List<Status_Data> _PlayerDatas = new List<Status_Data>();
    public List<Status_Data> _MonsterDatas = new List<Status_Data>();
    [Header("Object")]
    public List<GameObject> _Avatar = new List<GameObject>();
    public List<GameObject> _Block = new List<GameObject>();
    public List<GameObject> _Monster = new List<GameObject>();
    public GameObject _SummonScroll;

    [Header("Monster Amount")]
    public int min_monster;
    public int max_monster;

}
