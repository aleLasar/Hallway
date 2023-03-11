using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Score
{
    public ConfingTerminals Terminals { get; set; }
    public List<Player> Players { get; set; }
}


public class ConfingTerminals
{
    public string IP { get; set; }
    public List<string> InPort { get; set; }
    public List<string> OutPort { get; set; }
}


public class Player
{
    public Guid Id { get; set; }
    public bool LeftHand { get; set; }
    public List<Obstacle> Obstacles { get; set; }
}


public class Obstacle
{
    public Guid Id { get; set; }
    public Vector3 Position { get; set; }
    public int Duration { get; set; }
}



public class Read_config
{
    private readonly Score score;
    private static Read_config config = null;

    private Read_config()
    {
        string path = Helper.PathScore;
        if(string.IsNullOrEmpty(path))
            SceneManager.LoadScene(2);

        string json = File.ReadAllText(path);

        try
        {
            score = JsonConvert.DeserializeObject<Score>(json);
            if (!VerifyScore())
                throw new Exception("Invalid score");
        }
        catch
        {
            SceneManager.LoadScene(2);
        }

    }

    public static Score GetScore()
    {
        if (config == null)
            config = new Read_config();
        return config.score;
    }

    private bool VerifyScore()
    {
        if(score == null) return false;

        if (score.Terminals == null) return false;
        if (string.IsNullOrEmpty(score.Terminals.IP)) return false;
        if(score.Terminals.InPort == null || score.Terminals.InPort.Count == 0) return false;
        if(score.Terminals.OutPort == null || score.Terminals.OutPort.Count == 0) return false;

        if (score.Players == null || score.Players.Count == 0) return false;
        if (score.Players.Count != score.Terminals.InPort.Count || score.Players.Count != score.Terminals.OutPort.Count) return false;

        var duplicatePlayers =  score.Players.GroupBy(x => x.Id).Where(x => x.Count()> 1).ToList();
        if (duplicatePlayers.Any()) return false;

        var duplicateObstacles = score.Players.SelectMany(x => x.Obstacles).GroupBy(x => x.Id).Where(x => x.Count() > 1).ToList();
        if (duplicateObstacles.Any()) return false;

        if (score.Players.SelectMany(x => x.Obstacles).Any())
            Limit();

        return true;
    }

    private void Limit()
    {
        var outOfBoundObstacles = score.Players.SelectMany(x => x.Obstacles)
            .Where(x => x.Position.x > 10 | x.Position.x < 0 | x.Position.y > 10 | x.Position.y < 0)
            .ToList();
        foreach (var obstacle in outOfBoundObstacles)
        {
            if (obstacle.Position.x < 0)
                obstacle.Position = new Vector3(0, obstacle.Position.y, obstacle.Position.z);
            if (obstacle.Position.x > 10)
                obstacle.Position = new Vector3(10, obstacle.Position.y, obstacle.Position.z);

            if (obstacle.Position.y < 0)
                obstacle.Position = new Vector3(obstacle.Position.x, 0, obstacle.Position.z);
            if (obstacle.Position.y > 10)
                obstacle.Position = new Vector3(obstacle.Position.x, 10, obstacle.Position.z);
        }
    }
}
