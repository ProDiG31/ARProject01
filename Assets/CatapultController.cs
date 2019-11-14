using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour
{
    //private Vector3 position;
    private float width;
    private float height;
    private float pullStrengh;
    private Animator _catapultAnimator;
    private Vector2 _firstTouchPosition;

    public Transform rockSpawn;

    public GameObject rock;

    [SerializeField]
    private float deltaStrengh = 150f;
    //[Range(0f,1f)]
    //[SerializeField]
    //private float strenghThreshold = .2f;

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "str = " + pullStrengh.ToString("f2"));
    }

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        _catapultAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Debug.Log("Touch Moving");
                Vector2 pos = touch.position;
                //Mathf.Clamp
                pullStrengh = (_firstTouchPosition.y - pos.y) / height;
                _catapultAnimator.SetFloat("PullStrengh", pullStrengh);
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
        GameObject rockThrow = Instantiate(rock, rockSpawn.position,Quaternion.Euler(transform.forward));
        rockThrow.GetComponent<Rigidbody>().AddForce(_catapultAnimator.GetFloat("PullStrengh") * deltaStrengh * rockThrow.transform.forward, ForceMode.Impulse);
        _catapultAnimator.SetFloat("PullStrengh", 0f);
    }
}
