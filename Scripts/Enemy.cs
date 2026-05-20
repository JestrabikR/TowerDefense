using Godot;
using System;

public partial class Enemy : PathFollow2D
{
    private float _speed = 150f;
    private int _health = 3;
    private int _reward = 10;

    private ProgressBar _healthBar;

    public void Setup(int health, float speed, int reward)
    {
        _health = health;
        _speed = speed;
        _reward = reward;

        _healthBar = GetNode<ProgressBar>("HealthBar");

        _healthBar.MaxValue = health;
        _healthBar.Value = health;
    }

    public override void _Process(double delta)
    {
        Progress += _speed * (float)delta;

        if (ProgressRatio >= 0.99f)
        {
            var gameManager = (GameManager)GetTree().CurrentScene;
            gameManager.LoseLife();
            gameManager.EnemyRemoved();

            QueueFree(); // Remove Enemy from the scene
        }
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;

        if (_healthBar != null)
        {
            _healthBar.Value = _health;
        }

        if (_health <= 0)
        {
            var gameManager = (GameManager)GetTree().CurrentScene;
            gameManager.AddGold(_reward);
            gameManager.EnemyRemoved();
            QueueFree();
        }
    }
}