using Godot;
using System;

public partial class Projectile : Node2D
{
    private Enemy _target;
    private int _damage;
    private float _speed = 300f;

    public void Setup(Enemy target, int damage)
    {
        _target = target;
        _damage = damage;
    }

    public override void _Process(double delta)
    {
        if (!GodotObject.IsInstanceValid(_target))
        {
            QueueFree();
            return;
        }

        float distanceToTarget = GlobalPosition.DistanceTo(_target.GlobalPosition);

        if (distanceToTarget < 10f)
        {
            _target.TakeDamage(_damage);
            QueueFree();
        }
        else
        {
            float moveAmount = _speed * (float)delta;
            GlobalPosition = GlobalPosition.MoveToward(_target.GlobalPosition, moveAmount);
        }
    }
}
