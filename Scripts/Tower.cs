using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Node2D
{
    private readonly List<Enemy> _enemiesInRange = [];
    private int _damage = 1;

    private float _rangeRadius = 150f;

    public override void _Ready()
    {
        var rangeArea = GetNode<Area2D>("RangeArea");
        var shootTimer = GetNode<Timer>("ShootTimer");

        var collisionShape = rangeArea.GetNode<CollisionShape2D>("CollisionShape2D");

        if (collisionShape.Shape is CircleShape2D circleShape)
        {
            circleShape.Radius = _rangeRadius;
        }

        rangeArea.AreaEntered += OnAreaEntered;
        rangeArea.AreaExited += OnAreaExited;
        shootTimer.Timeout += OnShootTimerTimeout;

        QueueRedraw();
    }

    private void OnAreaEntered(Area2D area)
    {
        // Hitbox is a child of Enemy => get parent which is Enemy
        if (area.GetParent() is Enemy enemy)
        {
            _enemiesInRange.Add(enemy);
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area.GetParent() is Enemy enemy)
        {
            _enemiesInRange.Remove(enemy);
        }
    }

    private void OnShootTimerTimeout()
    {
        _enemiesInRange.RemoveAll(enemy => !GodotObject.IsInstanceValid(enemy));
        _enemiesInRange.RemoveAll(enemy => GlobalPosition.DistanceTo(enemy.GlobalPosition) > _rangeRadius);

        if (_enemiesInRange.Count > 0)
        {
            // Aim at the first enemy in range
            var target = _enemiesInRange[0];

            target.TakeDamage(_damage);

            GD.Print("Shooting at enemy!");
        }
    }

    public override void _Draw()
    {
        var circleColor = new Color(0.2f, 0.5f, 1.0f, 0.15f);

        DrawCircle(Vector2.Zero, _rangeRadius, circleColor);

        var lineColor = new Color(0.2f, 0.5f, 1.0f, 0.4f);
        DrawArc(Vector2.Zero, _rangeRadius, 0, Mathf.Tau, 64, lineColor, 1.5f);
    }
}
