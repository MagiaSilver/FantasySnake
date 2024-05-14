using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data 
{
  
}
public enum Direction {Up,Left,Right,Down };

[System.Serializable]
public class Data_Position
{
    public GameObject _Object;
    public int X;
    public int Y;
    public Direction direction;
    public Data_Position(GameObject _Object, int X, int Y, Direction direction)
    {
        this._Object = _Object;
        this.X = X;
        this.Y = Y;
        this.direction = direction;
    }
    public Data_Position()
    {
        this._Object = null;
        this.X = 0;
        this.Y = 0;
        this.direction = Direction.Down;
    }
}

/*[System.Serializable]
public class Data_Object
{
    public GameObject _Object;
    public int X;
    public int Y;
    public Direction direction;
    public Data_Position(GameObject _Object, int X, int Y, Direction direction)
    {
        this._Object = _Object;
        this.X = X;
        this.Y = Y;
        this.direction = direction;
    }
    public Data_Position()
    {
        this._Object = null;
        this.X = 0;
        this.Y = 0;
        this.direction = Direction.Down;
    }
}*/



[System.Serializable]
public class Status_Data
{
    public float HP;
    public float Max_HP;
    public float Attack;

    public Status_Data(float HP, float Max_HP, float Attack) { this.HP = HP; this.Max_HP= Max_HP; this.Attack = Attack; }
    public Status_Data() { this.HP = 100; this.Max_HP = HP; this.Attack = 50; }
    //public GameObject Avatar;
}
