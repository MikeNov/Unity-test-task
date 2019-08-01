using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class MainScript : MonoBehaviour
{
    public GameObject sphere;
    public List<float> CoordX = new List<float>();
    public List<float> CoordY = new List<float>();
    public List<float> CoordZ = new List<float>();
    public Vector3[] objPosition;
    public float chaseRange;
    public float sphereSpeed;
    public float camSpeed;
    public Camera cam;
    public Color32 col;
    public TextAsset json;

    private bool mouseClicked = false;
    private bool backMovement = false;
    private int index = 0;
    private int y = 0;
    private int z = 0;
    private int i = 0;
    private float range = 0;
    private float oordinate;
    private string n;

    void Start()
    {
        string jsonString = json.ToString();

        var values = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonString);

        foreach (KeyValuePair<string, List<string>> kvp in values)
        {            
            foreach (string val in kvp.Value)
            {
                if (val.ToString().Contains("."))
                    n = val.Replace(".".ToString(), ",".ToString());

                if (kvp.Key == "x".ToString())
                {
                    if (n != null)
                        oordinate = float.Parse(n);
                    else
                        oordinate = 0;

                    CoordX.Add(oordinate);
                    index++;
                }
                else if (kvp.Key == "y".ToString())
                {
                    if (n != null)
                        oordinate = float.Parse(n);
                    else
                        oordinate = 0;

                    CoordY.Add(oordinate);
                    y++;
                }
                else if (kvp.Key == "z".ToString())
                {
                    if (n != null)
                        oordinate = float.Parse(n);
                    else
                        oordinate = 0;

                    CoordZ.Add(oordinate);
                    z++;
                }

            }
        }

        objPosition = new Vector3[index];

        for (int i = 0; i < index; i++)
        {
            objPosition[i].Set(CoordX[i], CoordY[i], CoordZ[i]);
        }

        sphere.transform.position = objPosition[0];
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < index - 1; i++)
        {
          
            Gizmos.color = col;
            Gizmos.DrawLine(objPosition[i], objPosition[i + 1]);
        }
    }

    public void Update()
    {
        cam.transform.LookAt(sphere.transform);
        range = Vector3.Distance(cam.transform.position, sphere.transform.position);

        if (range >= chaseRange)
        {
            Vector3 dir = sphere.transform.position - cam.transform.position;
            dir = dir.normalized;
            cam.transform.Translate(dir * camSpeed * Time.deltaTime, Space.World);
        }

        if (mouseClicked)
        {
            if (sphere.transform.position != objPosition[i])
            {
                sphere.transform.position = Vector3.MoveTowards(sphere.transform.position, objPosition[i], Time.deltaTime * sphereSpeed);
            }
            else
            {
                if (!backMovement)
                {
                    i = (i < index - 1) ? i + 1 : i;
                    if (sphere.transform.position == objPosition[index - 1])
                    {
                        backMovement = true;
                        mouseClicked = false;
                    }
                }
                else if (backMovement)
                {
                    i = (i > 0) ? i - 1 : 0;
                    if (sphere.transform.position == objPosition[0])
                    {
                        backMovement = false;
                        mouseClicked = false;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!mouseClicked)
                    mouseClicked = true;
            }
        }
    }
}
