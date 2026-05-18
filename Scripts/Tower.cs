using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Node2D
{
    private readonly List<Enemy> _enemiesInRange = [];
    private int _damage = 1;

    public override void _Ready()
    {
        var rangeArea = GetNode<Area2D>("RangeArea");
        var shootTimer = GetNode<Timer>("ShootTimer");

        rangeArea.AreaEntered += OnAreaEntered;
        rangeArea.AreaExited += OnAreaExited;
        shootTimer.Timeout += OnShootTimerTimeout;
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

        if (_enemiesInRange.Count > 0)
        {
            // Aim at the first enemy in range
            var target = _enemiesInRange[0];

            target.TakeDamage(_damage);

            GD.Print("Shooting at enemy!");
        }
    }
}
