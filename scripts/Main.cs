using System.Diagnostics;
using Godot;

public partial class Main : Node
{
    [Export]
    public PackedScene EnemyScene { get; set; }

    private int _score;

    public void GameOver()
    {
        GetNode<Timer>("EnemyTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
    }

    public void NewGame()
    {
        _score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();
    }

    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("EnemyTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    private void OnScoreTimerTimeout()
    {
        _score++;
    }

    private void OnEnemyTimerTimeout()
    {
        Enemy enemy = EnemyScene.Instantiate<Enemy>();

        var enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");
        enemySpawnLocation.ProgressRatio = GD.Randf();
        enemy.Position = enemySpawnLocation.Position;

        // Set the enemy to move toward player
        Player player = GetNode<Player>("Player");
        Vector2 direction = (player.GlobalPosition - enemySpawnLocation.Position).Normalized();
        float randomAngle = (float)GD.RandRange(-30.0f, 30.0f) * Mathf.Pi / 180.0f;
        direction = direction.Rotated(randomAngle);
        enemy.Rotate(direction.Angle());


        // Choose the velocity
        var velocity = new Vector2((float)GD.RandRange(350.0, 550.0), 0);
        enemy.LinearVelocity = velocity.Rotated(direction.Angle());

        AddChild(enemy);
    }
    public override void _Ready()
    {
        // NewGame();
    }
}