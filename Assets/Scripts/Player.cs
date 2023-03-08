
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public FuelBar fuelBar;

    private float curFuel = 100;
    private float maxFuel = 100;
    private bool moving = false;
    private bool grappling = false;
    private float timer;

    [SerializeField]
    private LayerMask grappleMask;

    [SerializeField]
    private float degreesPerSec;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float repulsorStrength;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private Planet grappledPlanet;

    [SerializeField]
    private float boostForce;

    [SerializeField]
    private float thrustSpeed;

    [SerializeField]
    private float minBoostX;

    [SerializeField]
    private float minBoostY;

    [SerializeField]
    private float grappleSpeed;

    [SerializeField]
    private float grappleRotateSpeed;

    [SerializeField]
    private float grappleMaxSpeed;

    [SerializeField]
    private float moveSpeedDecay;

    private void Awake()
    {
        //initialize references here

        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        fuelBar.SetMaxFuel(maxFuel);
    }

    void Update()
    {
        HandleButtonInput();
        HandleMovement();
        UpdateFuel();
        if (grappling)
        {
            RotateAroundPlanet();
        }

        //move this somewhere else!
        if (GameManager.Instance.gameStarted == false && Input.GetMouseButtonDown(0))
        {
            Launch();
        }

    }

    private void RotateAroundPlanet()
    {

        Debug.Log("grappling around " + grappledPlanet.name);
        transform.RotateAround(grappledPlanet.transform.position, Vector3.back, grappleRotateSpeed);

    }

    private void UpdateFuel()
    {
        if (moving)
        {
            fuelBar.SetFuel(curFuel);
        }
    }

    private void HandleButtonInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (grappling)
            {
                StopGrapple();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShootRepulsor();
        }
    }

    private void ShootRepulsor()
    {

        //Cast a ray from the mouse position to world space, if anything was clicked on, add negative explosive force

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 diff = transform.position - hit.point;
            rb.AddForce(diff.normalized * repulsorStrength);
        }
    }
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //multiply the direction pressed by the thrust speed set in the inspector to use the thrusters. Checks if there is fuel available before running


        //rotation
        if (!grappling)
        {
            rb.rotation *= Quaternion.AngleAxis(horizontal * Time.deltaTime * rotateSpeed, -Vector3.forward);

        }

        //thrust
        if (vertical > 0)
        {
            timer = 0;
            timer += Time.deltaTime;
            curFuel -= timer;
            moving = true;
            if (!grappling)
            {
                rb.velocity += acceleration * Time.deltaTime * vertical * rb.transform.up;

            }
        }

        //speed cap
        if (grappling)
        {
            if (rb.velocity.magnitude > grappleMaxSpeed)
            {
                rb.velocity = Vector3.Normalize(rb.velocity) * maxSpeed;
            }
        }
        else
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector3.Normalize(rb.velocity) * (rb.velocity.magnitude - moveSpeedDecay * Time.deltaTime);
            }
        }
    }

    void StartGrapple()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, grappleMask))
        {
            grappledPlanet = hit.transform.parent.gameObject.GetComponent<Planet>();
            grappling = true;
        }
    }
    private void StopGrapple()
    {
        grappling = false;
        rb.velocity = grappleMaxSpeed * rb.transform.up;
    }

    private void ApplyBoost()
    {
        rb.velocity += boostForce * rb.transform.up;
    }

    private bool CheckForBoostAngle()
    {
        bool x = false;
        bool y = false;
        bool final = false;
        if (Mathf.Abs(transform.position.x) > Mathf.Abs(grappledPlanet.transform.position.x + minBoostX) || Mathf.Abs(transform.position.x) < Mathf.Abs(grappledPlanet.transform.position.x - minBoostX))
        {
            x = true;
        }
        if (Mathf.Abs(transform.position.y) > Mathf.Abs(grappledPlanet.transform.position.y + minBoostY) || Mathf.Abs(transform.position.y) < Mathf.Abs(grappledPlanet.transform.position.y - minBoostY))
        {
            y = true;
        }
        if (x && y)
        {
            final = true;
        }
        return final;
    }

    public void Launch()
    {

        // add a force to the rb on the y axis (determined by a value set in game manager, and a speed set in game manager)

        GameManager.Instance.gameStarted = true;
        rb.AddForce(GameManager.Instance.launchDir * GameManager.Instance.launchSpeed);
    }
}

