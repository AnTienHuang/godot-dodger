using Godot;
using System;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export]
    public int Speed { get; set; } = 400;
    public Vector2 ScreenSize;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        Hide(); // Player is only shown when user clicks ready
    }

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero; // player's movement vector

        if (Input.IsActionPressed("MoveUp"))
        {
            velocity.Y -= 1;
        }

        if (Input.IsActionPressed("MoveDown"))
        {
            velocity.Y += 1;
        }

        if (Input.IsActionPressed("MoveRight"))
        {
            velocity.X += 1;
        }

        if (Input.IsActionPressed("MoveLeft"))
        {
            velocity.X -= 1;
        }

        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");


        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite2D.Play();
        }
        else
        {
            animatedSprite2D.Stop();
        }

        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );
        if (velocity.X != 0)
        {
            animatedSprite2D.Animation = "Walk";
            animatedSprite2D.FlipV = false;
            animatedSprite2D.FlipH = velocity.X < 0;
        }
        else if (velocity.Y != 0)
        {
            animatedSprite2D.Animation = "Up";
            animatedSprite2D.FlipV = velocity.Y > 0;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        Hide(); // Hide player after being hit
        EmitSignal(SignalName.Hit);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true); // Disable the physics for player after current physics calculation is done.
    }
}
