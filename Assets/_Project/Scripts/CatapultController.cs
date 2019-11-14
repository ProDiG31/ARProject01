using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CatapultController : MonoBehaviour
{
    public Transform rockSpawn;
    public GameObject rock;

    [SerializeField]
    private float deltaStrengh = 150f;

    private LineRenderer Lr;
    private float width;
    private float height;
    private float pullStrengh;
    private Animator _catapultAnimator;
    private Vector2 _firstTouchPosition;
    //private int _lineResolution = 30;
    //private int _angle = 20;
    //private float g;
    //private float radianAngle;
    private Camera ARCamera;
    private Vector3 screenCenter;
    private LayerMask ARLayerMask; 

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);
        GUI.Label(new Rect(20, 20, width, height * 0.25f), "str = " + pullStrengh.ToString("f2"));
    }

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        _catapultAnimator = GetComponent<Animator>();
        //Lr = GetComponentInChildren<LineRenderer>();
        Lr = ControllerPlane.IGCanvas.GetComponentInChildren<LineRenderer>();
        //g = Mathf.Abs(Physics.gravity.y);
        Lr.enabled = true;
        ARCamera = Camera.main;
        ARLayerMask = LayerMask.NameToLayer("AR_Sphere");
        screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch Detected");
                _catapultAnimator.SetBool("isTouching", true);
                pullStrengh = 0f;
                _catapultAnimator.SetFloat("PullStrengh", pullStrengh);
                _firstTouchPosition = touch.position;
                //Lr.enabled = true;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Debug.Log("Touch Moving");
                Vector2 pos = touch.position;
                pullStrengh = (_firstTouchPosition.y - pos.y) / height;
                _catapultAnimator.SetFloat("PullStrengh", pullStrengh);
                DrawLine();
                //RenderArc();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("Touch End");
                _catapultAnimator.SetBool("isTouching", false);
            }
        }
    }

    public void ThrowRock()
    {
        Debug.Log("ThrowRock");
        //Lr.enabled = false;
        Vector3 spwanPosition = RaycastARWorld();
        if (spwanPosition != Vector3.zero)
        {
            GameObject rockThrow = Instantiate(rock, spwanPosition, Quaternion.Euler(transform.forward), ControllerPlane.levelCreated.transform);
            rockThrow.GetComponent<Rigidbody>().AddForce(_catapultAnimator.GetFloat("PullStrengh") * deltaStrengh * ARCamera.transform.forward, ForceMode.Impulse);
            _catapultAnimator.SetFloat("PullStrengh", 0f);
        }
    }

    public Vector3 RaycastARWorld()
    {
        RaycastHit hit;
        Ray ray = ARCamera.ScreenPointToRay(screenCenter);

        //point = ARCamera.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, ARCamera.nearClipPlane));

        if (Physics.Raycast(ray, out hit, ARLayerMask))
        {
            //GameObject Prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //Prim.transform.position = hit.point;
            return hit.point;
        }
        return Vector3.zero;
    }

    public void DrawLine()
    {
        Lr.SetPosition(0, Camera.main.transform.position);
        Lr.SetPosition(1, RaycastARWorld());
        Lr.endColor = Color.red;
        //Camera.main.transform.position;
    }

    //void RenderArc()
    //{
    //    Lr.positionCount = _lineResolution + 1;
    //    Vector3 startWorldPosition = RaycastARWorld();
    //    if (!startWorldPosition.Equals(Vector3.zero))
    //    {
    //        Lr.SetPositions(CalculateArcArray(startWorldPosition));
    //    }
    //}

    //Vector3[] CalculateArcArray(Vector3 pointInARWorld)
    //{
    //    Vector3[] Array = new Vector3[_lineResolution + 1];
    //    radianAngle = Mathf.Deg2Rad * _angle;
    //    float maxDist = (pullStrengh * pullStrengh * Mathf.Sin(2 * radianAngle));
    //    for(int i = 0; i < _lineResolution; i++)
    //    {
    //        float t = (float)i / (float)_lineResolution;
    //        Array[i] = CalculateArcPoint(t, maxDist) + pointInARWorld;
    //    }
    //    return Array;
    //}

    //Vector3 CalculateArcPoint(float t, float maxDist)
    //{
    //    float x = t * maxDist;
    //    float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * pullStrengh * pullStrengh * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
    //    return new Vector3(x, y);
    //}
}
