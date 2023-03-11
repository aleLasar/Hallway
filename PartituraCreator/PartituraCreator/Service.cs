using Newtonsoft.Json;
using PartituraCreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Formatting = Newtonsoft.Json.Formatting;
using Vector3 = PartituraCreator.Model.Vector3;

namespace PartituraCreator;

public class Service
{
    private Score _Score;
    public Service()
    {
        _Score = new()
        {
            Terminals = new()
            {
                InPort = new(),
                OutPort = new()
            },
            Players = new()
        };
    }

    public async Task<bool> PrintAsync(string path)
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(_Score, Formatting.Indented);
            path += "\\Partitura.json";
            await File.WriteAllTextAsync(path, jsonString);
            return true;
        }
        catch(Exception e)
        {
            throw;
        }
    }

    public async Task<string> ReadAsync(string path)
    {
        try
        {
            string readText = await File.ReadAllTextAsync(path);
            var readScore = JsonConvert.DeserializeObject<Score>(readText);
            if (readScore == null)
                return "Unable to deserialize json file";

            _Score = readScore;
            return string.Empty;

        }
        catch(Exception e)
        {
            throw;
        }
    }

    public bool AddIpAddress(string ip)
    {
        _Score.Terminals.IP = ip;
        return true;
    }

    public Guid AddPlayer(bool leftHand, string inPort, string outPort)
    {
        Player player = new()
        {
            Id = Guid.NewGuid(),
            LeftHand = leftHand,
            Obstacles= new()
        };

        if (_Score.Terminals.InPort.Contains(inPort) || _Score.Terminals.OutPort.Contains(outPort))
            throw new Exception("InPort and OutPort are already present");

        _Score.Players.Add(player);

        _Score.Terminals.InPort.Add(inPort);
        _Score.Terminals.OutPort.Add(outPort);

        return player.Id;
    }

    public Player GetPlayer(Guid id)
    {
        return _Score.Players.Where(x => x.Id == id).FirstOrDefault();
    }

    public List<Player> GetPlayers()
    {
        return _Score.Players;
    }

    public bool SetPlayer(Guid id, bool leftHand, string inPort, string outPort)
    {
        Player player = _Score.Players.Where(x => x.Id == id).FirstOrDefault();
        if (player == null)
            throw new Exception("Player not found");

        player.LeftHand = leftHand;

        int index = _Score.Players.FindIndex(x => x == player);
        _Score.Terminals.InPort[index] = inPort;
        _Score.Terminals.OutPort[index] = outPort;

        return true;
    }

    public bool DeletePlayer(Guid id)
    {
        Player player = _Score.Players.Where(x => x.Id == id).FirstOrDefault();
        if (player == null)
            throw new Exception("Player not found");

        _Score.Players.Remove(player);
        return true;
    }

    public Guid AddObstacle(Guid playerId, Vector3 pos, int duration)
    {
        Player player = _Score.Players.Where(x => x.Id == playerId).FirstOrDefault();
        if (player == null)
            throw new Exception("Player not found");

        pos.Limit();

        Obstacle obstacle = new()
        {
            Id = Guid.NewGuid(),
            Position = pos,
            Duration = duration
        };

        if (!player.Obstacles.Any(x => x.Position == pos))
            player.Obstacles.Add(obstacle);
        else
            throw new Exception("Another object with the same position is already exist");

        return obstacle.Id;
    }

    public Obstacle GetObstacle(Guid id)
    {
        return _Score.Players.SelectMany(x => x.Obstacles).Where(x => x.Id == id).FirstOrDefault();
    }

    public bool SetObstacle(Guid cubeId, Vector3 pos, int duration)
    {
        var obstacle = _Score.Players.SelectMany(x => x.Obstacles).Where(x => x.Id == cubeId ).FirstOrDefault();
        if (obstacle == null)
            throw new Exception("Obstacle not found");

        obstacle.Position= pos.Limit();
        obstacle.Duration= duration;

        return true;
    }

    public bool DeleteObstacle(Guid id)
    {
        foreach(var player in _Score.Players)
        {
            var obstacle = player.Obstacles.Where(x => x.Id == id).FirstOrDefault();
            if (obstacle != null)
            {
                player.Obstacles.Remove(obstacle);
                return true;
            }
        }

        throw new Exception("Obstacle not found");
    }


}
