using Godot;
using System;
using System.IO;
using System.Linq;

public partial class GameManager : Node
{
	
    public static GameManager Instance;

    [Export] private PackedScene barrierScene;
    [Export] private float gapBetweenBarriers = 550f;
    [Export] private Vector2 spawnHeightRange = new Vector2(150f, 550f);
    [Export] private Bird bird;
    [Export] private Node currentScene;
    [Export] private RichTextLabel currentScoreLabel;
    [Export] private RichTextLabel highScoreLabel;

    private float lastBirdX = 0f;
    private float birdXTally = 0f;
    private int score = 0;
    private int highScore = 0;
    private string path;
	
    public override void _Ready()
    {
        Instance = this;

        string data;
        path = Path.Join(ProjectSettings.GlobalizePath("user://"), "HighScore.txt");

        if (!File.Exists(path)) return;

        try
        {
            data = File.ReadAllText(path);

            int.TryParse(data, out highScore);
            highScoreLabel.Text = highScore.ToString();
        }
        catch (System.Exception)
        {
        }
    }

    public override void _Process(double delta)
    {
        birdXTally += bird.Position.X - lastBirdX;

        lastBirdX = bird.Position.X;

        if (birdXTally >= gapBetweenBarriers)
        {
            birdXTally = 0f;
            Node2D barrier = barrierScene.Instantiate<Node2D>();
            AddChild(barrier);
			
            float yPos = (float)GD.RandRange(spawnHeightRange.X, spawnHeightRange.Y);
            barrier.Position = new Vector2(bird.Position.X + gapBetweenBarriers, yPos);
        }
    }

    public void Reload()
    {
        if (score > highScore)
        {
            File.WriteAllText(path, score.ToString());
        }
        
        currentScene.GetTree().ReloadCurrentScene();
    }

    public void AddPoint()
    {
        score++;
        currentScoreLabel.Text = score.ToString();
        bird.PointScoredSfx();
    }
    
}