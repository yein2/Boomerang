using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    LineRenderer lr;
    Vector3 cube1Pos, cube2Pos;
 
    public float beziersize = 0;
    
    [SerializeField]
    private Vector3 point_0;
    [SerializeField]
    private Vector3 point_1;
    [SerializeField]
    private Vector3 point_2;
    [SerializeField]
    private Vector3 point_3;

    [SerializeField]
    private int interval;

    private Vector3[] bezierPoints = null;

    private void Start()
    {
        // lr = GetComponent<LineRenderer>();
        // lr.startWidth = .05f;
        // lr.endWidth = .05f;
 
        // cube1Pos = gameObject.GetComponent<Transform>().position;

        bezierPoints = GetBezierCurve(interval);
    }
 
    void Update()
    {
       
        // Debug.Log(MathCS.Bezier(this.transform.position,this.transform.position + new Vector3(0,0,300),this.transform.position + new Vector3(200, 0, 0), this.transform.position, beziersize));
       
        // lr.SetPosition(0, cube1Pos);
        // lr.SetPosition(1, GameObject.Find("Cube2").GetComponent<Transform>().position);

        for (int i = 0; i < bezierPoints.Length - 1; i++){
            Debug.DrawLine(transform.rotation * bezierPoints[i], transform.rotation * bezierPoints[i + 1], Color.red);
        }
    }

    Vector3[] GetBezierCurve(int size){
        Vector3[] _bezierCurvePoints = new Vector3[size];
        float interval = 1f / size;
        float value = 0f;

        for (int i = 0; i < size; i++){
            _bezierCurvePoints[i] = MathCS.Bezier(point_0 + transform.position, point_1 + transform.position, point_2 + transform.position, point_3 + transform.position, value);
            value += interval;
        }

        return _bezierCurvePoints;
    }
}
