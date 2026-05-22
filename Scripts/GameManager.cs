using Godot;
using System;

public record WaveConfig(int NumberOfEnemies, int EnemyHealth, float EnemySpeed, int EnemyReward, float SpawnInterval, string EnemyScenePath);

public partial class GameManager : Node2D
{
    private int _gold = 85;
    private int _lives = 5;

    private readonly WaveConfig[] _waves =
    [
        new (NumberOfEnemies: 4,  EnemyHealth: 3, EnemySpeed: 100f, EnemyReward: 5, SpawnInterval: 2.5f, EnemyScenePath: "res://enemy.tscn"),
        new (NumberOfEnemies: 6,  EnemyHealth: 5, EnemySpeed: 120f, EnemyReward: 15, SpawnInterval: 2.0f, EnemyScenePath: "res://enemyWave2.tscn"),
        new (NumberOfEnemies: 8, EnemyHealth: 3, EnemySpeed: 200f, EnemyReward: 10, SpawnInterval: 1.5f, EnemyScenePath: "res://enemyWave3.tscn")
    ];

    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;
    private int _activeEnemiesOnField;
    private bool _allWavesSpawned;
    private bool _waveSpawningFinished;

    private Label _goldLabel;
    private Label _livesLabel;
    private Label _waveLabel;

    private PackedScene _enemyScene;
    private Path2D _enemyPath;

    private Timer _spawnTimer;
    private Timer _countdownTimer;
    private int _secondsRemaining;

    private Button _playAgainButton;

    public override void _Ready()
    {
        _goldLabel = GetNode<Label>("%GoldLabel");
        _livesLabel = GetNode<Label>("%LivesLabel");
        _waveLabel = GetNode<Label>("%WaveLabel");
        _enemyPath = GetNode<Path2D>("%EnemyPath");

        _spawnTimer = GetNode<Timer>("%SpawnTimer");
        _spawnTimer.Timeout += OnSpawnTimerTimeout;

        _countdownTimer = new Timer();
        AddChild(_countdownTimer);
        _countdownTimer.WaitTime = 1.0;
        _countdownTimer.Timeout += OnCountdownTimerTimeout;

        _playAgainButton = GetNode<Button>("%PlayAgainButton");
        _playAgainButton.Pressed += OnPlayAgainButtonPressed;
        _playAgainButton.Visible = false;

        UpdateUi();

        StartPrepPhase();
    }

    private void UpdateUi()
    {
        _goldLabel.Text = $"Zlato: {_gold}";
        _livesLabel.Text = $"Životy: {_lives}";
    }
    private void OnPlayAgainButtonPressed()
    {
        GetTree().Paused = false;

        GetTree().ReloadCurrentScene();
    }

    private void StartPrepPhase()
    {
        _secondsRemaining = 7;
        _countdownTimer.Start();
        UpdateCountdownDisplay();
    }

    private void UpdateCountdownDisplay()
    {
        var visualWaveNum = _currentWaveIndex + 1;
        _waveLabel.Text = $"Příprava na vlnu {visualWaveNum}/{_waves.Length} ({_secondsRemaining}s)";
    }

    private void OnCountdownTimerTimeout()
    {
        _secondsRemaining--;

        if (_secondsRemaining >= 0)
        {
            UpdateCountdownDisplay();
        }

        if (_secondsRemaining < 0)
        {
            _countdownTimer.Stop();
            StartWave();
        }
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

            var enemyScene = GD.Load<PackedScene>(currentWave.EnemyScenePath);

            var enemyInstance = enemyScene.Instantiate<Enemy>();
            
            enemyInstance.Setup(currentWave.EnemyHealth, currentWave.EnemySpeed, currentWave.EnemyReward);
            _enemyPath.AddChild(enemyInstance);

            _activeEnemiesOnField++;
            _enemiesLeftToSpawn--;
        }
        else
        {
            _spawnTimer.Stop();
            _currentWaveIndex++;

            if (_currentWaveIndex >= _waves.Length)
            {
                _allWavesSpawned = true;
                CheckWinCondition();
            }
            else
            {
                _waveSpawningFinished = true;
                
                if (_activeEnemiesOnField == 0)
                {
                    _waveSpawningFinished = false;
                    StartPrepPhase();
                }
            }
        }
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        UpdateUi();
    }

    public void EnemyRemoved()
    {
        _activeEnemiesOnField--;

        if (_waveSpawningFinished && _activeEnemiesOnField == 0 && _currentWaveIndex < _waves.Length)
        {
            _waveSpawningFinished = false;
            StartPrepPhase();
        }

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (_allWavesSpawned && _activeEnemiesOnField <= 0 && _lives > 0)
        {
            _waveLabel.Text = "Konec hry - VÝHRA!";
            _playAgainButton.Visible = true;
        }
    }

    public void LoseLife()
    {
        _lives -= 1;
        UpdateUi();

        if (_lives <= 0)
        {
            _waveLabel.Text = "Konec hry - PROHRA!";
            GetTree().Paused = true;
            _playAgainButton.Visible = true;
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

    public override void _ExitTree()
    {
        _countdownTimer?.QueueFree();
    }
}
