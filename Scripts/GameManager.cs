using Godot;
using System;

public record WaveConfig(int NumberOfEnemies, int EnemyHealth, float EnemySpeed, int EnemyReward, float SpawnInterval);

public partial class GameManager : Node2D
{
    private int _gold = 85;
    private int _lives = 5;

    private readonly WaveConfig[] _waves =
    [
        new (NumberOfEnemies: 3,  EnemyHealth: 3, EnemySpeed: 100f, EnemyReward: 5, SpawnInterval: 3.0f),
        new (NumberOfEnemies: 5,  EnemyHealth: 5, EnemySpeed: 120f, EnemyReward: 15, SpawnInterval: 2.0f),
        new (NumberOfEnemies: 8, EnemyHealth: 5, EnemySpeed: 200f, EnemyReward: 10, SpawnInterval: 1.5f)
    ];

    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;

    private Label _goldLabel;
    private Label _livesLabel;
    private Label _waveLabel;

    private PackedScene _enemyScene = GD.Load<PackedScene>("res://enemy.tscn");
    private Path2D _enemyPath;

    private Timer _spawnTimer;

    public override void _Ready()
    {
        _goldLabel = GetNode<Label>("%GoldLabel");
        _livesLabel = GetNode<Label>("%LivesLabel");
        _waveLabel = GetNode<Label>("%WaveLabel");
        _enemyPath = GetNode<Path2D>("%EnemyPath");

        _spawnTimer = GetNode<Timer>("%SpawnTimer");
        _spawnTimer.Timeout += OnSpawnTimerTimeout;

        UpdateUi();

        StartPrepPhase();
    }

    private void UpdateUi()
    {
        _goldLabel.Text = $"Zlato: {_gold}";
        _livesLabel.Text = $"Životy: {_lives}";
    }

    private async void StartPrepPhase()
    {
        if (_currentWaveIndex >= _waves.Length)
        {
            _waveLabel.Text = "Konec hry - VÝHRA!";
            return;
        }

        var visualWaveNum = _currentWaveIndex + 1;
        _waveLabel.Text = $"Příprava na vlnu {visualWaveNum}/{_waves.Length} (10s)";

        await ToSignal(GetTree().CreateTimer(10.0), "timeout");

        StartWave();
    }

    private void StartWave()
    {
        var visualWaveNum = _currentWaveIndex + 1;
        _waveLabel.Text = $"Vlna: {visualWaveNum}/{_waves.Length}";

        var currentWave = _waves[_currentWaveIndex];

        _enemiesLeftToSpawn = currentWave.NumberOfEnemies;
        _spawnTimer.WaitTime = currentWave.SpawnInterval;
        _spawnTimer.Start();
    }

    private void OnSpawnTimerTimeout()
    {
        if (_lives <= 0) return;

        if (_enemiesLeftToSpawn > 0)
        {
            var currentWave = _waves[_currentWaveIndex];

            var enemyInstance = _enemyScene.Instantiate<Enemy>();
            
            enemyInstance.Setup(currentWave.EnemyHealth, currentWave.EnemySpeed, currentWave.EnemyReward);
            _enemyPath.AddChild(enemyInstance);

            _enemiesLeftToSpawn--;
        }
        else
        {
            _spawnTimer.Stop();
            _currentWaveIndex++;

            StartPrepPhase();
        }
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
            _waveLabel.Text = "Konec hry - PROHRA!";
            GetTree().Paused = true;
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
