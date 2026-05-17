using Godot;
using System;

public partial class GameManager : Node2D
{
    private int _gold = 100;
    private int _lives = 5;

    private Label _goldLabel;
    private Label _livesLabel;

    private PackedScene _enemyScene = GD.Load<PackedScene>("res://enemy.tscn");
    private Path2D _enemyPath;

    public override void _Ready()
    {
        _goldLabel = GetNode<Label>("%GoldLabel");
        _livesLabel = GetNode<Label>("%LivesLabel");

        _enemyPath = GetNode<Path2D>("%EnemyPath");

        UpdateUi();

        GD.Print("Game loaded.");
    }

    private void UpdateUi()
    {
        _goldLabel.Text = $"Zlato: {_gold}";
        _livesLabel.Text = $"Životy: {_lives}";
    }

    public void OnSpawnButtonPressed()
    {
        GD.Print("Spawning enemy...");
        var enemyInstance = _enemyScene.Instantiate<Enemy>();

        _enemyPath.AddChild(enemyInstance);
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        UpdateUi();
    }

    public void LoseLife()
    {
        _lives -= 1;
        UpdateUi();

        if (_lives <= 0)
        {
            GD.Print("Game Over!");
        }
    }

    public bool TrySpendGold(int amount)
    {
        if (_gold >= amount)
        {
            _gold -= amount;
            UpdateUi();
            return true;
        }
        return false;
    }
}
