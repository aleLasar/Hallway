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

    private float minZoom = 20f;
    private float maxZoom = 50f;
    private float zoomLimiter = 12f;

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
        float maxLenght = GetGreatestDistance();
        float clamp = maxLenght / zoomLimiter;

        float zoom = Mathf.Lerp(minZoom, maxZoom, clamp);

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoom, Time.deltaTime*1.5f);
    }

    private void Move()
    {
        Vector3 center = GetCenterPoint();
        if (center.y > 6)
            center = new Vector3(center.x, 7.0f, center.z);
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

        float distance = Mathf.Sqrt(Mathf.Pow(bounds.size.x, 2) + Mathf.Pow(bounds.size.y, 2));

        return distance;
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

