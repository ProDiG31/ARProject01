using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CatapultController : MonoBehaviour
{
    public Transform rockSpawn;
    public GameObject rock;
    public GameObject targetIndicator; 

    [SerializeField]
    private float deltaStrengh = 150f;

    private float width;
    private float height;
    private float _pullStrengh;
    private Animator _catapultAnimator;
    private Vector2 _firstTouchPosition;

    private Camera ARCamera;
    public LayerMask ARBridgeMask;
    public LayerMask ARPropsMask;
    //private string LayerARSphere = "AR_Bridge";
    //private string LayerLevelProps = "AR_World";

    private ARRaycastManager arRayCastManager;
    private List<ARRaycastHit> hitsResults = new List<ARRaycastHit>();
    private bool isPlaneDetected;

    private AudioSource _audioSource;

    public List<AudioClip> _AudioClips;

    //private string hitted = "";
    private GameObject RayCastedItem;

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);
        GUI.Label(new Rect(20, 20, width, height * 0.25f), "str =   " + _pullStrengh.ToString("f2"));
        GUI.Label(new Rect(20, 45, width, height * 0.25f), "Trgt =  " + targetIndicator.activeSelf);
        //GUI.Label(new Rect(20, 100, width, height * 0.25f), "hit =  " + hitted);

        if(RayCastedItem != null)
        {
            if (RayCastedItem.GetComponent<DestructibleProps>())
            {
                DestructibleProps item = RayCastedItem.GetComponent<DestructibleProps>();
                GUI.Label(new Rect(20, 100, width, height * 0.25f), "Ray =  " + RayCastedItem.name + " - " + item._lifePoint + "/" + item.LifePointMax);
            } else
            {
                GUI.Label(new Rect(20, 100, width, height * 0.25f), "Ray =  " + RayCastedItem.name);
            }
        }

    }

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        _catapultAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        ARCamera = Camera.main;
        
        //ARBridgeMask = 1 << LayerMask.GetMask(LayerARSphere);
        //ARPropsMask = 1 << LayerMask.GetMask(LayerLevelProps);
    }

    void Start()
    {
        arRayCastManager = FindObjectOfType<ARRaycastManager>();
        isPlaneDetected = false;
        //targetIndicator = Instantiate(targetIndicator);
        //targetIndicator.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayCastResult();
        UpdateTarget();
        HandleTouch();
    }

    public Vector3 GetScreenCenter()
    {
        //Camera.
        return Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
    }

    public void HandleTouch()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _catapultAnimator.SetBool("isTouching", true);
                _pullStrengh = 0f;
                _catapultAnimator.SetFloat("PullStrengh", _pullStrengh);
                _firstTouchPosition = touch.position;
                PlaySound(0);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                _pullStrengh = (_firstTouchPosition.y - pos.y) / height;
                _catapultAnimator.SetFloat("PullStrengh", _pullStrengh);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _catapultAnimator.SetBool("isTouching", false);
            }
        }
    }

    private void UpdateRayCastResult()
    {
        hitsResults = new List<ARRaycastHit>();
        arRayCastManager.Raycast(GetScreenCenter(), hitsResults, TrackableType.Planes);
        isPlaneDetected = hitsResults.Count > 0;
    }

    private void UpdateTarget()
    {
        bool targetSet = false;

        if (ControllerPlane.isLevelCreated)
        {
            Ray ray = ARCamera.ScreenPointToRay(GetScreenCenter());
            //RaycastHit[] hitProps = Physics.RaycastAll(ray, ARPropsMask).OrderBy(hit => hit.distance).ToArray();
            //if (hitProps.Length > 0)
            //{
            //    hitted = "1 " + hitProps[0].transform.name;
            //    targetIndicator.SetActive(true);
            //    targetIndicator.transform.position = hitProps[0].point;
            //    targetIndicator.transform.rotation = Quaternion.Euler(hitProps[0].normal * 90);
            //    targetSet = true;
            //}

            if (Physics.Raycast(ray, out RaycastHit hitProps, ARPropsMask))
            {
                //hitted = "1 " + hitProps.transform.name;
                if (hitProps.collider.gameObject.layer == ARPropsMask)
                {
                    RayCastedItem = hitProps.transform.gameObject;
                    targetIndicator.SetActive(true);
                    targetIndicator.transform.position = hitProps.point;
                    targetIndicator.transform.rotation = Quaternion.Euler(hitProps.normal * 90);
                    targetSet = true;
                }
            }
        }

        if (isPlaneDetected && !targetSet)
        {
            targetIndicator.SetActive(true);
            Pose detectedPlanePose = hitsResults[0].pose;
            Vector3 ArCamForward = ARCamera.transform.forward;
            Vector3 ArCamBearing = new Vector3(ArCamForward.x, 0, ArCamForward.z).normalized;
            detectedPlanePose.rotation = Quaternion.LookRotation(ArCamBearing);
            targetIndicator.transform.SetPositionAndRotation(detectedPlanePose.position, detectedPlanePose.rotation);
            //hitted = "2 - AR plan";
            targetSet = true;
        }

        if (!targetSet) {
            targetIndicator.SetActive(false);
        }
    }

    public void ThrowRock()
    {
        Ray ray = ARCamera.ScreenPointToRay(GetScreenCenter());
        if (Physics.Raycast(ray, out RaycastHit hitSphere, ARBridgeMask))
        {
            PlaySound(1);
            //hitted = hitSphere.transform.name;

            Vector3 SpawnPosition = hitSphere.point; // + (- ARCamera.transform.forward);
            GameObject rockThrow = Instantiate(rock, SpawnPosition, Quaternion.Euler(ARCamera.transform.forward), ControllerPlane.levelCreated.transform);
            rockThrow.GetComponent<Rigidbody>().AddForce(_catapultAnimator.GetFloat("PullStrengh") * deltaStrengh * ARCamera.transform.forward, ForceMode.Impulse);
            _catapultAnimator.SetFloat("PullStrengh", 0f);
        }
    }

    private void PlaySound(int id)
    {
        if (_audioSource.isPlaying) _audioSource.Stop();
        _audioSource.clip = _AudioClips[id];
        _audioSource.Play();
    }

    //public void DrawLine()
    //{
    //    if(Lr == null)
    //    {
    //        Lr = ControllerPlane.IGCanvas.GetComponentInChildren<LineRenderer>();
    //    }
    //    Lr.enabled = false;
    //    Lr.SetPosition(0, GetScreenCenter());
    //    Lr.SetPosition(1, RaycastGeneratedWorld().Value.transform.position);
    //    Lr.endColor = Color.red;
    //}
    //void RenderArc()
    //{
    //    Lr.positionCount = _lineResolution + 1;
    //    Vector3 startWorldPosition = GetScreenCenter();
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
    //    for (int i = 0; i < _lineResolution; i++)
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
