using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public GameController gameController;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float minZoom = 10f;
    public float maxZoom = 30f;
    public float zoomLimiter = 50f;

    private List<GameObject> players;
    private Vector3 velocity;
    private Camera camera;

    private bool _StartProcidure = true;
    private int _StartAngle = 90;
    private int _EndAngle = 0;
    Quaternion _StartRot, _EndRot;

    private bool _EndGame = false;

    private void Start()
    {
        players = gameController.GetPlayersList();
        camera = GetComponent<Camera>();
        _StartRot = Quaternion.Euler(_StartAngle,0,0);
        _EndRot = Quaternion.Euler(_EndAngle,0,0);
        transform.rotation = _StartRot;
    }

    private void LateUpdate()
    {
        if (transform.position.z > gameController.GetFinishGameLenght())
        {
            CloseGame();
            if(!_EndGame)
            {
                var p = players.First().GetComponent<Player_move>();
                p.EndGame();
                _EndGame= true;
            }
        }            
        else
        {
            if (_StartProcidure == false)
                Zoom();
            else
                StartProcedure();
        }

        Move();
        RemoveBorder();
    }

    private void StartProcedure()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _EndRot, 1f * Time.deltaTime);
        
        if (transform.rotation == _EndRot)
            _StartProcidure = false;
    }

    private void Zoom()
    {
        float zoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter );
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoom, Time.deltaTime);
    }

    private void Move()
    {
        Vector3 center = GetCenterPoint();
        center += offset;
        transform.position = Vector3.SmoothDamp(transform.position, center, ref velocity, smoothTime);

    }

    private void RemoveBorder()
    {
        var borders = gameController.GetBorders();

        var toDelete = borders.Where(x => transform.position.z > x.transform.position.z + 7).ToList();

        foreach (var border in toDelete)
        {
            borders.Remove(border);
            Destroy(border.gameObject);
        }
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        foreach (var player in players)
            bounds.Encapsulate(player.transform.position);

        return bounds.size.x;
    }
    private Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
            return players[0].transform.position;

        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        foreach (var player in players)
            bounds.Encapsulate(player.transform.position);

        return bounds.center;
    }

    private void CloseGame()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _StartRot, 1f * Time.deltaTime);

        if(transform.rotation == _StartRot)
            SceneManager.LoadScene(3);
    }
}

