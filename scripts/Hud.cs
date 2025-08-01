using Godot;
using System;

public partial class Hud : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public void ShowMessage(string text)
    {
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();
        GetNode<Timer>("MessageTimer").Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");

        var messageTimer = GetNode<Timer>("MessageTimer");
        await ToSignal(messageTimer, Timer.SignalName.Timeout);

        var message = GetNode<Label>("Message");
        message.Text = "Dodge the Enemies!";
        message.Show();
        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>("StartButton").Show();
    }

	public override void _Ready()
    {
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
