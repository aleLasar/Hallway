
using UnityEngine;


public class DimmerLight : MonoBehaviour
{
    private Light lightToDim = null;


    private void Awake()
    {
        lightToDim = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightToDim.intensity = Mathf.PingPong(Time.time*7, 40);
    }

}
