using Godot;
using System;

public partial class Enemy : PathFollow2D
{
    private float _speed = 150f;
    private int _health = 3;
    private int _reward = 10;

    public void Setup(int health, float speed, int reward)
    {
        _health = health;
        _speed = speed;
        _reward = reward;
    }

    public override void _Process(double delta)
    {
        Progress += _speed * (float)delta;

        if (ProgressRatio >= 0.99f)
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
            gameManager.AddGold(_reward);
            QueueFree();
        }
    }
}