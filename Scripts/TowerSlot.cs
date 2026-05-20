using Godot;
using System;

public partial class TowerSlot : Button
{
    private PackedScene _towerScene = GD.Load<PackedScene>("res://tower.tscn");
    private int _towerPrice = 50;

    public override void _Ready()
    {
        Text = $"$ {_towerPrice}";

        Pressed += OnSlotPressed;
    }

    private void OnSlotPressed()
    {
        var gameManager = (GameManager)GetTree().CurrentScene;

        if (gameManager.TrySpendGold(_towerPrice))
        {
            var towerInstance = _towerScene.Instantiate<Node2D>();

            towerInstance.Position = Position + new Vector2(Size.X / 2, Size.Y / 2);

            gameManager.AddChild(towerInstance);

            QueueFree();
        }
        else
        {
            GD.Print("Not enough gold!");
        }
    }
}
