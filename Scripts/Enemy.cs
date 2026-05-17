using Godot;
using System;

public partial class Enemy : PathFollow2D
{
    private float _speed = 150f;
    private int _health = 3;

    public override void _Process(double delta)
    {
        Progress += _speed * (float)delta;

        if (ProgressRatio >= 1.0f)
        {
            var gameManager = (GameManager)GetTree().CurrentScene;
            gameManager.LoseLife();

            QueueFree(); // Remove Enemy from the scene
        }
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            var gameManager = (GameManager)GetTree().CurrentScene;
            gameManager.AddGold(20);
            QueueFree();
        }
    }
}