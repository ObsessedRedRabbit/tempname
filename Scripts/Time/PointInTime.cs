using UnityEngine;

public class PointInTime
{
    public Vector3 position;
    public int health;

    public PointInTime (Vector3 _position, int _health)
    { 
        position = _position;
        health = _health;
    }
}
