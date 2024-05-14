using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UI_Manager Instance;

    [Header("Score")]
    [SerializeField]
    private int Score;    
    [SerializeField]
    private TextMeshProUGUI Score_Text;

    [Header("Main Status")]
    [SerializeField]
    private UI_Status MainStatus;
    [SerializeField]
    private UI_Status EnemyStatus;

    [Header("Player List")]
    [SerializeField]
    private GameObject UI_Template;
    [SerializeField]
    private Transform UI_Content;
    [SerializeField]
    private List<GameObject> UI_List = new List<GameObject>();

    [Header("Score")]
    [SerializeField]
    private GameObject GameOver_Panel;
    [SerializeField]
    private Button Restart_Button;

    private void Awake()
    {
        Instance = this;
    }
   
    public void SettingListUI()
    {
       Data_Position[] datas = SnakeGameManager.Instance._Player.ToArray();

        if (datas.Length == 0)
        {
            return;
        }
        PlayerStatus player = datas[0]._Object.GetComponent<PlayerStatus>();
        MainStatus.SetStatus(player.GetStatus, player.GetLevel);

        int count = datas.Length;
            for (int i = 1; i < count; i++)
            {
                GameObject _icon;
                PlayerStatus _player;

                if (UI_List.Count >= i)
                {
                    _icon = UI_List[i-1];
                }
                else
                {
                    _icon = Instantiate(UI_Template, UI_Content);
                    UI_List.Add(_icon);
                }
                //_________________________________________________
                if (_icon != null)
                {
                    UI_Status status = _icon.GetComponent<UI_Status>();
                    _player = datas[i]._Object.GetComponent<PlayerStatus>();
                    status.SetStatus(_player.GetStatus, _player.GetLevel);
                    _icon.SetActive(true);
                }

            }

            for (int i = count-1; i < UI_List.Count; i++)
            {
                UI_List[i].gameObject.SetActive(false);
            } 
        
    }
    public void UpdateMainStatus(PlayerStatus player)
    {
        MainStatus.SetStatus(player.GetStatus, player.GetLevel);
    }
    public void UpdateEnemyStatus(MonsterStatus monster)
    {
        EnemyStatus.SetStatus(monster.GetStatus, monster.GetLevel);
    }
    public void UpdateScore()
    {
        Score += 1;
        Score_Text.text = string.Format("Score : {0}",Score);
    }
    public void GameOverOnOff(bool On)
    {
        GameOver_Panel.SetActive(On);
    }
    private void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void Start()
    {
        Restart_Button.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
