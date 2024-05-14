using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class SnakeGameManager : MonoBehaviour
{
    public static SnakeGameManager Instance;
    [Header("Map Setting")]
    [SerializeField]
    private GameObject plane;

    public List<Data_Position> _Positions = new List<Data_Position>();
    public List<Data_Position> _Block = new List<Data_Position>();
    public List<Data_Position> _Monster = new List<Data_Position>();
    public List<Data_Position> _Scroll = new List<Data_Position>();
    public List<Data_Position> _Player = new List<Data_Position>();

    //private List<Data_Position> _Used = new List<Data_Position>();
    [SerializeField]
    private Transform _Map_Position;
    [SerializeField]
    private Transform _Map_Block;
    [SerializeField]
    private Transform _Map_Monster;
    [SerializeField]
    private Transform _Map_Scroll;
    [SerializeField]
    private Transform Avatar;
    [SerializeField]
    private bool firstsetting = true;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    IEnumerator Start()
    {
        firstsetting = true;
        UI_Manager.Instance.GameOverOnOff(false);
        GenerateMap();
        yield return new WaitForSeconds(0.5f);
        GenerateBlock();
        yield return new WaitForSeconds(0.5f);
        GenerateMonster();
        yield return new WaitForSeconds(0.5f);
        GenerateScoll();
        yield return new WaitForSeconds(0.5f);        
        PlayerController.Instance.SetPosition(_Positions[0]);
        RandomPlayer(_Positions[0]);
        firstsetting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstsetting &&_Monster.Count<=0) GenerateMonster();

        if (!firstsetting && (PlayerController.Instance.GetGameOver ||_Player.Count <= 0))
        {
            UI_Manager.Instance.GameOverOnOff(true);

        }
    }
    private void GenerateMap()
    {
        for (int x = 0; x < ListPlayerData.Instance.x_Size; x++)
        {
            for (int y = 0; y < ListPlayerData.Instance.y_Size; y++)
            {
                GameObject _plane = Instantiate(plane,new Vector3(x ,0, y ),Quaternion.identity);
                _plane.transform.SetParent(_Map_Position);
                _plane.transform.rotation = Quaternion.Euler(SetRotation(Direction.Down));
                _Positions.Add(new Data_Position(_plane,x,y,Direction.Down));
            }
        }
    }
    private void GenerateBlock()
    {
        int num = Random.Range(10, 20);
        for (int i = 0; i < num; i++)
        {
            int random = Random.Range(0, ListPlayerData.Instance._Block.Count);
            int amount = Random.Range(1, _Positions.Count);
            if (!CheckBlock(_Positions[num]))
            {
                GameObject _block = Instantiate(ListPlayerData.Instance._Block[random], _Positions[amount]._Object.transform.position, Quaternion.identity);
                _block.transform.SetParent(_Map_Block);
                _block.transform.rotation =Quaternion.Euler(SetRotation(_Positions[amount].direction));
                _Block.Add(new Data_Position(_block, _Positions[amount].X, _Positions[amount].Y, _Positions[amount].direction));
                //_Used.Add(new Data_Position(_block, _Positions[amount].X, _Positions[amount].Y, _Positions[amount].direction));
            }
            else
            {
                i--;
            }
        }
        
    }
    private void GenerateMonster()
    {
        int num = Random.Range(ListPlayerData.Instance.min_monster, ListPlayerData.Instance.max_monster);
        for (int i = 0; i < num; i++)
        {
            int amount = Random.Range(1, _Positions.Count);
            if (!CheckBlock(_Positions[amount]) && !CheckPlayerPos(_Positions[amount])&&!CheckMonster(_Positions[amount]))
            {
                int random_Data = Random.Range(0, ListPlayerData.Instance._PlayerDatas.Count);
                int random_Avatar = Random.Range(0, ListPlayerData.Instance._Monster.Count);

                GameObject @object = Instantiate(ListPlayerData.Instance._Monster[random_Avatar]);

                Status_Data status = new Status_Data(ListPlayerData.Instance._MonsterDatas[random_Data].HP, ListPlayerData.Instance._MonsterDatas[random_Data].Max_HP, ListPlayerData.Instance._MonsterDatas[random_Data].Attack);
                @object.GetComponent<MainStatus>().SetStatus(status);
                @object.GetComponent<MainStatus>().SetPosition(_Positions[amount]);

                @object.transform.SetParent(_Map_Monster);
                @object.transform.position = _Positions[amount]._Object.transform.position;
                @object.transform.rotation = Quaternion.Euler(SetRotation(_Positions[amount].direction));

                _Monster.Add(new Data_Position(@object, _Positions[amount].X, _Positions[amount].Y, _Positions[amount].direction));
                //_Used.Add(new Data_Position(@object, _Positions[amount].X, _Positions[amount].Y, _Positions[amount].direction));
            }
            else
            {
                i--;
            }
        }

    }
    public void GenerateScoll()
    {
       
        for (int i = 0; i < 1; i++)
        { 
            int num = Random.Range(1, _Positions.Count);
            if (CheckBlock(_Positions[num]) || CheckPlayerPos(_Positions[num]) || CheckMonster(_Positions[num]))
            {
                i--;
                return;
            }
            GameObject @object;
            if (_Scroll.Count <= 0)
            {
                @object = Instantiate(ListPlayerData.Instance._SummonScroll);  
                @object.transform.SetParent(_Map_Scroll);
                @object.transform.position = _Positions[num]._Object.transform.position;
                @object.transform.rotation = Quaternion.Euler(SetRotation(_Positions[num].direction));

                _Scroll.Add(new Data_Position(@object, _Positions[num].X, _Positions[num].Y, _Positions[num].direction));
            }
            else
            {
                @object = _Scroll[i]._Object;
                @object.transform.SetParent(_Map_Scroll);
                @object.transform.position = _Positions[num]._Object.transform.position;
                @object.transform.rotation = Quaternion.Euler(SetRotation(_Positions[num].direction));
                _Scroll[0].X = _Positions[num].X;
                _Scroll[0].Y = _Positions[num].Y;
            }
        }
        /*{
            amount = Random.Range(1, _Positions.Count);
        }*/

        /*int num = Random.Range(5, 10);
        for (int i = 0; i < num; i++)
        {
            int amount = Random.Range(1, _Positions.Count);
            if (Checkposition(_Positions[amount], _Used.ToArray()))
            {

                int random_Data = Random.Range(0, ListPlayerData.Instance._Datas.Count);

                GameObject @object = Instantiate(ListPlayerData.Instance._SummonScroll);
                //@object.GetComponent<MainStatus>().SetPlayerStatus(ListPlayerData.Instance._Datas[random_Data]);
                @object.transform.SetParent(_Map_Scroll);
                @object.transform.position = _Positions[amount]._Object.transform.position;
                @object.transform.rotation = Quaternion.Euler(SetRotation(_Positions[amount].direction));

                _Scroll.Add(new Data_Position(@object, _Positions[amount].X, _Positions[amount].Y, _Positions[amount].direction));
                _Used.Add(new Data_Position(@object, _Positions[amount].X, _Positions[amount].Y, _Positions[amount].direction));
            }
            else
            {
                i--;
            }
        }*/

    }


    public void RandomPlayer(Data_Position _After)
    {
        int random_Data = Random.Range(0, ListPlayerData.Instance._PlayerDatas.Count);
        int random_Avatar = Random.Range(0, ListPlayerData.Instance._Avatar.Count);

        GameObject @object = Instantiate(ListPlayerData.Instance._Avatar[random_Avatar]);

        Status_Data status = new Status_Data(ListPlayerData.Instance._PlayerDatas[random_Data].HP, ListPlayerData.Instance._PlayerDatas[random_Data].Max_HP, ListPlayerData.Instance._PlayerDatas[random_Data].Attack);
        @object.GetComponent<PlayerStatus>().SetStatus(status);
        @object.GetComponent<MainStatus>().SetPosition(new Data_Position(@object, _After.X, _After.Y, Direction.Up));

        @object.transform.SetParent(Avatar);
        @object.transform.position = SnakeGameManager.Instance.GetPosition(_After)._Object.transform.position;
        @object.transform.rotation = SnakeGameManager.Instance.GetPosition(new Data_Position(@object, _After.X, _After.Y, Direction.Up))._Object.transform.rotation;
        SnakeGameManager.Instance._Player.Add(new Data_Position(@object, _After.X, _After.Y, Direction.Up));

        UpdateAllLevelPlayer(); 
        UI_Manager.Instance.SettingListUI();
    }
    public void RemovePlayer(Data_Position player)
    {
        GameObject @object = player._Object;
        SnakeGameManager.Instance._Player.Remove(player);
        GameObject.Destroy(@object);
        UpdateAllLevelPlayer();
        UI_Manager.Instance.SettingListUI();
    }
    public bool CheckPlayerPos(Data_Position player)
    {
        return _Player.Find(X => X.X == player.X && X.Y == player.Y) != null ? true : false; ;
    }
    public void UpdateAvatarMove(Data_Position _Before, Data_Position _After)
    {

        Data_Position _Temp = new Data_Position();
        _Temp.X = _Before.X;
        _Temp.Y = _Before.Y;
        _Temp.direction =_Before.direction;

        for (int i = 0; i < _Player.Count; i++)
        {
            if (i == 0)
            {
                _Temp.X = _Player[i].X;
                _Temp.Y = _Player[i].Y;
                _Temp.direction = _Player[i].direction;

                _Player[i].X = _After.X;
                _Player[i].Y = _After.Y;
                _Player[i].direction = _After.direction;
            }
            else
            {
                int temp_X = _Player[i].X;
                int temp_Y = _Player[i].Y;
                Direction direction = _Player[i].direction;

                _Player[i].X = _Temp.X;
                _Player[i].Y = _Temp.Y;
                _Player[i].direction = _Temp.direction;

                _Temp.X = temp_X;
                _Temp.Y = temp_Y;
                _Temp.direction = direction;
            }
            _Player[i]._Object.transform.position = GetPosition(_Player[i])._Object.transform.position;
            _Player[i]._Object.transform.rotation = Quaternion.Euler(SetRotation(_Player[i].direction));

        }
    }
    public void Switch_Q()
    {
        GameObject _Temp = _Player[0]._Object;
        for (int i = 0; i < _Player.Count; i++)
        {
            if (i + 1 < _Player.Count)
            {
                ;

                _Player[i]._Object = _Player[i + 1]._Object;

            }
            else
            {
                _Player[i]._Object = _Temp;
            }

            _Player[i]._Object.transform.position = SnakeGameManager.Instance.GetPosition(_Player[i])._Object.transform.position;
            _Player[i]._Object.transform.rotation = Quaternion.Euler(SnakeGameManager.Instance.SetRotation(_Player[i].direction));

        }
        UI_Manager.Instance.SettingListUI();
    }
    public void Switch_E()
    {
        GameObject _Temp = _Player[_Player.Count - 1]._Object;
        for (int i = _Player.Count - 1; i >= 0; i--)
        { 
            if (i - 1 >= 0)
            {

                _Player[i]._Object = _Player[i - 1]._Object;
                Debug.LogError("Number : " + i);

            }
            else
            {
                _Player[i]._Object = _Temp;
                Debug.LogError("Number : Temp");
            }

            _Player[i]._Object.transform.position = SnakeGameManager.Instance.GetPosition(_Player[i])._Object.transform.position;
            _Player[i]._Object.transform.rotation = Quaternion.Euler(SnakeGameManager.Instance.SetRotation(_Player[i].direction));

        }
        UI_Manager.Instance.SettingListUI();
    }


    public bool CheckBlock(Data_Position _Alpha)
    {
        return _Block.Find(X=>X.X == _Alpha.X&& X.Y == _Alpha.Y) != null ? true:false;
    }
    public bool CheckMonster(Data_Position _Alpha)
    {
        return _Monster.Find(X => X.X == _Alpha.X && X.Y == _Alpha.Y) != null ? true : false; ;
    }
    public void RemoveMonster(Data_Position _Alpha)
    {
        GameObject @object = _Alpha._Object;
        _Monster.Remove(_Alpha);
        GameObject.Destroy(@object);
        UpdateAllLevelMonster();
    }
    public Data_Position GetMonster(Data_Position _Alpha)
    {
        return _Monster.Find(X => X.X == _Alpha.X && X.Y == _Alpha.Y); 
    }
    public bool CheckScroll(Data_Position _Alpha)
    {
        return _Scroll.Find(X => X.X == _Alpha.X && X.Y == _Alpha.Y) != null ? true : false; ;
    }


    public int GetX_Size { get { return ListPlayerData.Instance.x_Size; } }
    public int GetY_Size { get { return ListPlayerData.Instance.y_Size; } }
    public Data_Position GetPosition(Data_Position _EX)
    {
        Data_Position _Positions = this._Positions.Find(X => X.X == _EX.X && X.Y == _EX.Y);
        return _Positions;
    }

    public Vector3 SetRotation(Direction _direction)
    {
        Vector3 quaternion = Vector3.forward;
        switch (_direction)
        {
            case Direction.Up: quaternion = new Vector3(0,0,0); break;
            case Direction.Right: quaternion = new Vector3(0, 90, 0); break;
            case Direction.Down: quaternion = new Vector3(0, 180, 0); break;
            case Direction.Left: quaternion = new Vector3(0, 270, 0); break;
            
        }
        return quaternion;
    }

    public int _LevelMonster()
    {
        int LV = 1;
        if (_Monster.Count <= ListPlayerData.Instance.max_monster && _Monster.Count > ListPlayerData.Instance.min_monster) LV = 2;
        else LV = 3;
        return LV;
                
    }
    public int _LevelPlayer()
    {
        int LV = 1;
        if (_Player.Count >= 5 && _Player.Count < 10) LV = 2;
        if (_Player.Count > 10) LV = 3;
        return LV;
    }
    private void UpdateAllLevelPlayer()
    {
        foreach (var item in _Player) 
        {
            PlayerStatus player = item._Object.GetComponent<PlayerStatus>();
            player.SetLevel = _LevelPlayer();
            player.UpdateStatusLevel();
        }
    }
    private void UpdateAllLevelMonster()
    {
        foreach (var item in _Monster)
        {
            MonsterStatus monster = item._Object.GetComponent<MonsterStatus>();
            monster.SetLevel= _LevelMonster();
            monster.UpdateStatusLevel();
        }
    }
}
