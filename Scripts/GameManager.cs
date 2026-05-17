using Godot;
using System;

using Godot;
using System;

public partial class GameManager : Node2D
{
    private int _gold = 100;
    private int _lives = 5;

    private Label _goldLabel;
    private Label _livesLabel;

    public override void _Ready()
    {
        _goldLabel = GetNode<Label>("%GoldLabel");
        _livesLabel = GetNode<Label>("%LivesLabel");

        UpdateUi();

        GD.Print("Game loaded.");
    }

    private void UpdateUi()
    {
        _goldLabel.Text = $"Zlato: {_gold}";
        _livesLabel.Text = $"Životy: {_lives}";
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
}
