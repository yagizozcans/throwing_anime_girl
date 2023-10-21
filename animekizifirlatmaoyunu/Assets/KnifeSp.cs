using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KnifeSp : MonoBehaviour
{

    public Rigidbody rb;

    private Vector2 startSwipe;
    private Vector2 endSwipe;

    [SerializeField] float forceMultiplier;
    [SerializeField] float torque;

    [SerializeField]private bool onTrigger;

    public float gravityScaler;
    float gravityScalerSc;

    [SerializeField] TextMeshProUGUI scoreText;
    int score;

    public bool isDead = false;

    public GameObject animeGirlImage;

    public RectTransform animegirlimagespawnpoint;

    public Canvas mainCanvas;

    public GameObject restartButton;

    private void Awake()
    {
        SkinnedMeshRenderer[] meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mesh.receiveShadows = false;
        }

        rb = GetComponent<Rigidbody>();
        Collider[] colList = transform.GetComponentsInChildren<Collider>();
        gravityScalerSc = gravityScaler;
        mainCanvas = FindObjectOfType<Canvas>();
        animegirlimagespawnpoint = GameObject.FindGameObjectWithTag("animegirlimagespawnpoint").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate new position
        rb.AddForce(new Vector3(0, -gravityScalerSc, 0), ForceMode.Acceleration);

        if (!isDead)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Swipe();
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on TouchPhase
                switch (touch.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        startSwipe = Camera.main.ScreenToViewportPoint(touch.position);
                        break;
                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        endSwipe = Camera.main.ScreenToViewportPoint(touch.position);
                        Swipe();
                        break;
                }
            }
        }
    }

    void Swipe()
    {
        if(onTrigger)
        {
            gravityScalerSc = gravityScaler;
            Vector2 swipe = (endSwipe - startSwipe) * forceMultiplier;

            rb.AddForce(swipe, ForceMode.Impulse);
            if(swipe.x != 0)
            {
                rb.AddTorque(0f, 0f, (Mathf.Abs(swipe.x) / -swipe.x) * torque, ForceMode.Impulse);
            }
            GetComponentInChildren<BoxCollider>().enabled = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name != "Death")
        {
            Vector3 newPos = transform.position;
            if (!onTrigger && !isDead)
            {
                gravityScalerSc = 0;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                onTrigger = true;
                GetComponentInChildren<BoxCollider>().enabled = false;

                score += 1;
                int number = Random.Range(0, 100);
                if (number > 1)
                {
                    GameObject animegirl = Instantiate(animeGirlImage, animegirlimagespawnpoint.transform.position, Quaternion.identity, mainCanvas.transform);
                }
                scoreText.text = score.ToString();

                transform.position = newPos;
            }
        }
        else
        {
            Die();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        onTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Die();
    }

    void Die()
    {
        isDead = true;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        restartButton.SetActive(true);
        gravityScalerSc = gravityScaler;
        GetComponentInChildren<BoxCollider>().enabled = true;
    }
}
