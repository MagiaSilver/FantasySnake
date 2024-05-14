using System.Collections;
using UnityEngine;
using UnityEngine.Experimental;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement Data")]
    public Data_Position _After;
    public Data_Position _Before;
    public Data_Position _Temp;

    [Header("Player Data")]
    [SerializeField]
    private bool CanPlay = true;
    [SerializeField]
    private bool GameOver = false;

    public bool GetGameOver { get { return GameOver; } }

    [Header("Enemy Data")]
    [SerializeField]
    private Data_Position player;
    [SerializeField]
    private Data_Position enemy;

    private void Awake()
    {
        Instance = this;
    }
    public void SetPosition(Data_Position position)
    {
        _After._Object.transform.position = position._Object.transform.position;
        _After.X = position.X;
        _After.Y = position.Y;
        _After.direction = position.direction;
    }
    private void Movement()
    {
        if (SnakeGameManager.Instance._Player.Count != 0 && SnakeGameManager.Instance._Player[0] != null)
        {
            #region Movement
            if (Input.GetKeyDown(KeyCode.W))
            {
                _Temp.X = _Before.X;
                _Temp.Y = _Before.Y;
                _Temp.direction = _Before.direction;

                _Before.X = _After.X;
                _Before.Y = _After.Y;
                _Before.direction = _After.direction;

                _After.Y += 1;
                _After.direction = Direction.Up;
                CheckPosition();
                Debug.LogError("KeyCode.W");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _Temp.X = _Before.X;
                _Temp.Y = _Before.Y;
                _Temp.direction = _Before.direction;

                _Before.X = _After.X;
                _Before.Y = _After.Y;
                _Before.direction = _After.direction;

                _After.X -= 1;
                _After.direction = Direction.Left;
                CheckPosition();
                Debug.LogError("KeyCode.A");
        }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _Temp.X = _Before.X;
                _Temp.Y = _Before.Y;
                _Temp.direction = _Before.direction;

                _Before.X = _After.X;
                _Before.Y = _After.Y;
                _Before.direction = _After.direction;

                _After.Y -= 1;
                _After.direction = Direction.Down;
                CheckPosition();
                Debug.LogError("KeyCode.S");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _Temp.X = _Before.X;
                _Temp.Y = _Before.Y;
                _Temp.direction = _Before.direction;

                _Before.X = _After.X;
                _Before.Y = _After.Y;
                _Before.direction = _After.direction;

                _After.X += 1;
                _After.direction = Direction.Right;
                CheckPosition();
                Debug.LogError("KeyCode.D");
            }
            #endregion
        }
    }

    private void CheckPosition() 
    {
        #region Check X Y Position

        if (_After.X < 0) { _After.X = 0; }
        if (_After.Y < 0) { _After.Y = 0; }
        if (_After.X > SnakeGameManager.Instance.GetX_Size - 1) { _After.X = SnakeGameManager.Instance.GetX_Size - 1; }
        if (_After.Y > SnakeGameManager.Instance.GetY_Size - 1) { _After.Y = SnakeGameManager.Instance.GetY_Size - 1; }


        if ((_After.X == _Before.X && _After.Y == _Before.Y)||(_After.X == _Temp.X && _After.Y == _Temp.Y))
        {
            _After.X = _Before.X;
            _After.Y = _Before.Y;
            _After.direction = _Before.direction;

            _Before.X = _Temp.X;
            _Before.Y = _Temp.Y;
            _Before.direction = _Temp.direction;
            Debug.LogError("Can't Walk");
            return;
        }
        
        else
        {
            if (SnakeGameManager.Instance.CheckBlock(_After))
            {
                //_After._Object.transform.position = SnakeGameManager.Instance.GetPosition(_After)._Object.transform.position;
                _After.X = _Before.X;
                _After.Y = _Before.Y;
                _After.direction = _Before.direction;

                _Before.X = _Temp.X;
                _Before.Y = _Temp.Y;
                _Before.direction = _Temp.direction;
                SnakeGameManager.Instance.RemovePlayer(SnakeGameManager.Instance._Player[0]);
            }
            else if (SnakeGameManager.Instance.CheckMonster(_After))
            {
                player = SnakeGameManager.Instance._Player[0];
                enemy = SnakeGameManager.Instance.GetMonster(_After);

                player._Object.gameObject.transform.rotation = Quaternion.Euler(SnakeGameManager.Instance.SetRotation(_After.direction));
                switch (_After.direction)
                {
                    case Direction.Left: enemy._Object.transform.rotation = Quaternion.Euler(SnakeGameManager.Instance.SetRotation(Direction.Right)); break;
                    case Direction.Right: enemy._Object.transform.rotation = Quaternion.Euler(SnakeGameManager.Instance.SetRotation(Direction.Left)); break;
                    case Direction.Up: enemy._Object.transform.rotation = Quaternion.Euler(SnakeGameManager.Instance.SetRotation(Direction.Down)); break;
                    case Direction.Down: enemy._Object.transform.rotation = Quaternion.Euler(SnakeGameManager.Instance.SetRotation(Direction.Up)); break;
                }

                _After.X = _Before.X;
                _After.Y = _Before.Y;
                _After.direction = _Before.direction;

                _Before.X = _Temp.X;
                _Before.Y = _Temp.Y;
                _Before.direction = _Temp.direction;

                StartCoroutine(Attack_TurnBased());

                return;
            }
            else if (SnakeGameManager.Instance.CheckScroll(_After))
            {
                //_After._Object.transform.position = SnakeGameManager.Instance.GetPosition(_After)._Object.transform.position;
                SnakeGameManager.Instance.RandomPlayer(_After);
                SnakeGameManager.Instance.GenerateScoll();
            }
            else if (SnakeGameManager.Instance._Player.Count>1 && SnakeGameManager.Instance.CheckPlayerPos(_After))
            {
                CanPlay = false;
                GameOver = true;
            }
            else
            {
                //_After._Object.transform.position = SnakeGameManager.Instance.GetPosition(_After)._Object.transform.position;

            }
   

            _After._Object.transform.position = SnakeGameManager.Instance.GetPosition(_After)._Object.transform.position;
            SnakeGameManager.Instance.UpdateAvatarMove(_Before,_After);
        

        }
        #endregion


    }

    private IEnumerator Attack_TurnBased()
    {
        CanPlay = false;
        PlayerStatus player = this.player._Object.GetComponent<PlayerStatus>();
        MonsterStatus enemy = this.enemy._Object.GetComponent<MonsterStatus>();

        UI_Manager.Instance.UpdateMainStatus(player);
        UI_Manager.Instance.UpdateEnemyStatus(enemy);
        yield return new WaitForSeconds(1f);

        player.animate.Attack();        
        enemy.animate.Hit();
        enemy.TakeDamage(player.GetStatus, player.GetLevel);

        UI_Manager.Instance.UpdateEnemyStatus(enemy);
        yield return new WaitForSeconds(1f);

        if (enemy.GetStatus.HP <= 0)
        {
            player.animate.Victory();
            enemy.animate.Dead();
            yield return new WaitForSeconds(1f);
            SnakeGameManager.Instance.RemoveMonster(this.enemy);
            UI_Manager.Instance.UpdateScore();
        }
        else
        {
            enemy.animate.Attack();
            player.animate.Hit();
            player.TakeDamage(enemy.GetStatus, enemy.GetLevel);
            UI_Manager.Instance.UpdateMainStatus(player);
            yield return new WaitForSeconds(1f);

            if (player.GetStatus.HP <= 0)
            {

                player.animate.Dead();
                enemy.animate.Victory();
                yield return new WaitForSeconds(2f);
                SnakeGameManager.Instance.RemovePlayer(SnakeGameManager.Instance._Player[0]);
                SnakeGameManager.Instance.UpdateAvatarMove(_Before,_After);
                UI_Manager.Instance.SettingListUI();

                if (SnakeGameManager.Instance._Player.Count <= 0)
                {
                    GameOver = true;
                }
            }
        }

        this.enemy = null;
        CanPlay = true;
    }
    void Update()
    {
        if (CanPlay)
        {
            #region Develop code
            if (Input.GetKeyDown(KeyCode.P))
            {
                SnakeGameManager.Instance.RandomPlayer(_After);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SnakeGameManager.Instance.Switch_Q();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                SnakeGameManager.Instance.Switch_E();
            }
            #endregion
            Movement();
        }
      
    }

    
}
