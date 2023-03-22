using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public GameObject Wall_object;
    public int Wall_lenght;
    public GameObject Obstacle_object;
    public GameObject Player_object;
    public GameObject Osc_handler;
    public List<Material> Materials_obstacle;
    public List<Material> Materials_players;
    public Material _ColliderMaterial;
    public List<Color> LightsColor;
    public Color ColliderLightColor;
    public float _PlayerSpeed;

    private List<GameObject> _object_players;

    private Score score;

    private float _ZSpawn = -10;
    private int numberOfWall = 20;
    private List<GameObject> _Borders = new List<GameObject>();
    private GameObject _BorderEnd = null;

    private float _MaxLenght;

    private void Awake()
    {
        score = Read_config.GetScore();

        _MaxLenght = score.Players.SelectMany(x => x.Obstacles).Max(x => x.Position.z) + 5;

        for (int i=0; i < numberOfWall; i++)
        {
            SpawnBorders(0, _ZSpawn);
            SpawnBorders(90, _ZSpawn);
            SpawnBorders(-90, _ZSpawn);
            SpawnBorders(180, _ZSpawn);
            _ZSpawn += Wall_lenght;
        }

        //Border end
        SpawnBorders(-1, _ZSpawn);

        for (int i = 0; i < score.Players.Count; i++)
            SpawnObstacle(score.Players[i].Id, score.Players[i].Obstacles, Materials_obstacle[i], LightsColor[i], 0, _ZSpawn);
       
        var osc_handlers = CreateOSCHandler();
        _object_players = CreatePlayers(osc_handlers);
    }

    private void Update()
    {
        if(_object_players.First().transform.position.z > _ZSpawn - (numberOfWall * Wall_lenght))
        {
            SpawnBorders(0, _ZSpawn);
            SpawnBorders(90, _ZSpawn);
            SpawnBorders(-90, _ZSpawn);
            SpawnBorders(180, _ZSpawn);

            for (int i = 0; i < score.Players.Count; i++)
                SpawnObstacle(score.Players[i].Id, score.Players[i].Obstacles, Materials_obstacle[i], LightsColor[i], _ZSpawn, _ZSpawn+Wall_lenght);

            _ZSpawn += Wall_lenght;
        }

        EndBorderMove();
    }

    public List<GameObject> GetBorders()
    {
        return _Borders;
    }

    public List<GameObject> GetPlayersList()
    {
        return _object_players;
    }

    public float GetFinishGameLenght()
    {
        return _MaxLenght;
    }
 
    private void EndBorderMove()
    {
        if (_BorderEnd != null)
        {
            Vector3 position = new Vector3(_BorderEnd.transform.position.x, _BorderEnd.transform.position.y, _ZSpawn);
            _BorderEnd.transform.position = position;
        }
    }

    private void SpawnBorders(int rotation, float zSpawn)
    {
        GameObject newObjt = Instantiate(Wall_object) as GameObject;

        switch (rotation)
        {
            case -1:
                newObjt.transform.position = new Vector3(5, 5, zSpawn);
                newObjt.transform.Rotate(0, 90, -90);
                break;
            case 0:
                newObjt.transform.position = new Vector3(5, 0, zSpawn + 5);
                break;
            case 90:
                newObjt.transform.position = new Vector3(0, 5, zSpawn + 5);
                newObjt.transform.Rotate(0, 0, rotation);
                break;
            case -90:
                newObjt.transform.position = new Vector3(10, 5, zSpawn + 5);
                newObjt.transform.Rotate(0, 0, rotation);
                break;
            case 180:
                newObjt.transform.position = new Vector3(5, 10.2f, zSpawn + 5);
                newObjt.transform.Rotate(0, 0, rotation);
                break;
        }

        if (rotation != -1)
        {
            newObjt.name = $"border-{Guid.NewGuid()}";
            _Borders.Add(newObjt);
        }
        else
        {
            newObjt.name = $"borderEND";
            _BorderEnd = newObjt;
        }
    }

    private void SpawnObstacle(Guid playerId, List<Obstacle> obstacle, Material material, Color lightColor, float start, float end)
    {
        foreach (Obstacle obs in obstacle.Where(x => x.Position.z >= start && x.Position.z <= end ))
        {
            GameObject newObjt = Instantiate(Obstacle_object) as GameObject;
            newObjt.transform.position = obs.Position;
            newObjt.transform.localScale = new Vector3(1, 1, obs.Duration);

            newObjt.GetComponent<Renderer>().material = material;
            newObjt.GetComponentInChildren<Light>().color = lightColor;

            newObjt.name = $"obstacle_{obs.Id}_{playerId}";
            
            newObjt.AddComponent<DestroyObject>();
        }
    }


    private List<OSC> CreateOSCHandler()
    {
        var returnList = new List<OSC>();
        for (int i = 0; i < score.Terminals.InPort.Count; i++)
        {
            GameObject newObjt = Instantiate(Osc_handler, Vector3.zero, Quaternion.identity) as GameObject;
            var script = newObjt.GetComponent<OSC>();
            script.inPort = Convert.ToInt32(score.Terminals.InPort[i]);
            script.outIP = score.Terminals.IP;

            if(score.Terminals.OutPort.Count != 0)
            {
                if (score.Terminals.OutPort[i] != "")
                    script.outPort = Convert.ToInt32(score.Terminals.OutPort[i]);
            }

            newObjt.name = $"OSCHandlerPlayer_{score.Players[i].Id}";
            returnList.Add(script);
        }
        return returnList;
    }

    private List<GameObject> CreatePlayers(List<OSC> osc_handlers)
    {
        var players_list = new List<GameObject>();
        for (var i = 0; i < score.Players.Count; i++)
        {
            var position = new Vector3(2 * i, 0.5f, 0);
            GameObject newObjt = Instantiate(Player_object, position, Quaternion.identity) as GameObject;
            var player_move = newObjt.GetComponent<Player_move>();

            newObjt.GetComponent<Renderer>().material = Materials_players[i];
            newObjt.GetComponentInChildren<Light>().color = LightsColor[i];

            player_move.DefaultMaterial = Materials_obstacle[i];
            player_move.ColliderMaterial = _ColliderMaterial;
            player_move.LightDefaultColor = LightsColor[i];
            player_move.LightColliderColor = ColliderLightColor;
            player_move.osc = osc_handlers[i];
            player_move._FarwordSpeed = _PlayerSpeed;
            player_move.LeftHand = score.Players[i].LeftHand == true ? 1 : -1;
            newObjt.name = $"Player_{score.Players[i].Id}";
            players_list.Add(newObjt);
        }
        return players_list;
    }

}
