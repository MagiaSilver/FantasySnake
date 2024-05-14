using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainStatus : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    protected Data_Position Position;
    [SerializeField]
    protected Status_Data BaseStatus;
    [SerializeField]
    protected Status_Data UseStatus;
    [SerializeField]
    protected int Level = 1;

    [Header("Aniamtion")]
    public CharacterAnimate animate;

    public void SetStatus(Status_Data Status)
    {
        this.BaseStatus = Status;
        this.UseStatus =new Status_Data(Status.HP, Status.Max_HP, Status.Attack);
    }
    public void SetPosition(Data_Position Position)
    {
        this.Position = Position;
    }
    public Status_Data GetStatus{ get { return UseStatus; } }
    public int SetLevel { set {Level = value; } }
    public int GetLevel { get { return Level; } }
    public void TakeDamage(Status_Data Status,int Level) 
    {
        this.UseStatus.HP -= Status.Attack;
    }
    // Start is called before the first frame update

    public void UpdateStatusLevel()
    {
        float percent = (this.UseStatus.HP * 100) / this.UseStatus.Max_HP;
 
        this.UseStatus.Max_HP = this.BaseStatus.HP + ((this.BaseStatus.HP * (Level - 1)) / 2);
        this.UseStatus.HP = (percent * this.UseStatus.Max_HP) / 100;
        this.UseStatus.Attack = this.BaseStatus.Attack + ((this.BaseStatus.Attack * (Level - 1)) / 2);
    }
    private void Awake()
    {
        animate = GetComponent<CharacterAnimate>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
