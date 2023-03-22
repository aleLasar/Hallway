using System;
using System.Linq;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    public OSC osc;
    public float _FarwordSpeed;
    public Material DefaultMaterial;
    public Material ColliderMaterial;
    public Color LightDefaultColor;
    public Color LightColliderColor;

    private Rigidbody rb;
    private float _Recived_x = 0;
    private float _Recived_y = 0;

    public int LeftHand;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        osc.SetAddressHandler("/ian", ReciveData);
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_Recived_x, _Recived_y, 50f);
        transform.Translate(movement * _FarwordSpeed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0.5f, 9.5f), Mathf.Clamp(transform.position.y, 0.5f, 9.5f), transform.position.z);
    }
        
    public void ReciveData(OscMessage msg)
    {
       
        float x_value = (LeftHand)* Scale(msg.GetFloat(0), -16000, 16000, -30, 30);
        float y_value = - Scale(msg.GetFloat(1), -16000, 16000, -30, 30);

        _Recived_x = x_value;
        _Recived_y = y_value;
    }

    private float Scale(float value, int min, int max, int minScale, int maxScale)
    {
        float scaled = minScale + (float)(value - min) / (max - min) * (maxScale - minScale);
        return scaled;
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;

        var splittedName = otherObject.name.Split('_');
        var key = splittedName.Last();
        if (key == name.Split('_').Last())
        {
            if (!Helper.PlayerScores.ContainsKey(key))
                Helper.PlayerScores.Add(key, 1);
            else
                Helper.PlayerScores[key]++;

            otherObject.GetComponent<Renderer>().material = ColliderMaterial;
            otherObject.GetComponentInChildren<Light>().color = LightColliderColor;
            otherObject.GetComponentInChildren<Light>().intensity = 100f;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject otherObject = other.gameObject;

        var splittedName = otherObject.name.Split('_');
        string key = splittedName.Last();
        if (key == name.Split('_').Last())
        {
            OscMessage message = new OscMessage();
            message.address = "/enter";

            message.values.Add(Scale(_Recived_x, -30, 30, 0, 1));
            message.values.Add(Scale(_Recived_y, -30, 30, 0, 1));
            osc.Send(message);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherObject = other.gameObject;

        var splittedName = otherObject.name.Split('_');
        string key = splittedName.Last();
        if (key == name.Split('_').Last())
        {

            otherObject.GetComponent<Renderer>().material = DefaultMaterial;
            otherObject.GetComponentInChildren<Light>().color = LightDefaultColor;
            otherObject.GetComponentInChildren<Light>().intensity = 20f;
        }
    }

    public void EndGame()
    {
        OscMessage message = new OscMessage();
        message.address = "/exit";
        message.values.Add("bang");
        osc.Send(message);
    }
}
