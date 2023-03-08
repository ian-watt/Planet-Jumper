using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool gameStarted = false;
    UnityEvent gameStart;
    public static GameManager Instance;
    public Player player;
    public Vector3 launchDir;
    public float launchSpeed;
    public float gravity = 9.81f;
    public Seed seed;
    public float maxPlanetRenderDistance;

    public Transform starfield;

    private Camera mainCam;

    public int minDistance;

    public List<Planet> currentPlanets;
    public int numPlanets = 10;

    [SerializeField]
    Vector3 bounds;


    public GameObject planetPrefab;
    [SerializeField]
    public float cameraZoomOutDist;
    public bool camZoomed = true;

    public float cameraZoomedSize;
    public float cameraZoomSpeed;
    public float cameraUnzoomedSize;
    float startingSize;
    Vector3 starFieldStartingSize;

    public Planet currentlyFocused;

    float timer = 0;
    private bool zooming = false;
    public float minPlanetSeperationDistance;

    private void Awake()
    {
        Instance = this;
        camZoomed = true;
        mainCam = Camera.main;
    }

    void Start()
    {

        {
            if (gameStart == null)
            {
                gameStart = new UnityEvent();
            }
            gameStart.AddListener(StartGame);
        }
    }

    void Update()
    {

        int check = 0;

        for (int x = 0; x < currentPlanets.Count; x++)
        {
            if (Vector3.Distance(player.transform.position, currentPlanets[x].transform.position) <= cameraZoomOutDist)
            {
                check++;
            }
            if(Vector3.Distance(player.transform.position, currentPlanets[x].transform.position) <= cameraZoomOutDist && camZoomed)
            {
                zooming = false;
                ZoomOutCamera();
            }
            if(Vector3.Distance(player.transform.position, currentPlanets[x].transform.position) <= cameraZoomOutDist && !camZoomed && mainCam.orthographicSize <= cameraUnzoomedSize - 1)
            {
                zooming = false;
                ZoomOutCamera();
            }
        }

        if (check == 0)
        {
            if (!camZoomed)
            {
                zooming = false;
                ZoomInCamera();
            }
        }
        else if (check == 0 && camZoomed && mainCam.orthographicSize >= cameraZoomedSize + 1)
        {
            if (camZoomed)
            {
                zooming = false;
                ZoomOutCamera();
            }
        }

        
        

        if (currentPlanets.Count < numPlanets && gameStarted)
        {
            CreatePlanets(numPlanets - currentPlanets.Count);
        }

        if (!gameStarted)
        {
            gameStart.Invoke();
        }
    }


    [ContextMenu("Zoom in")]
    private void ZoomInCamera()
    {

        if (!zooming)
        {
            timer = 0;
            timer += Time.deltaTime;
            startingSize = mainCam.orthographicSize;
            

        }
        Debug.Log("tried to  in");

        mainCam.orthographicSize = Mathf.Lerp(startingSize, cameraZoomedSize, Mathf.Clamp(timer / cameraZoomSpeed, 0, 1));

        if (mainCam.orthographicSize <= cameraZoomedSize + 1)
        {
            zooming = true;
            camZoomed = true;

        }
    }

    [ContextMenu("Zoom out")]
    private void ZoomOutCamera()
    {
        if (!zooming)
        {

            timer = 0;
            timer += Time.deltaTime;
            startingSize = mainCam.orthographicSize;

        }
        mainCam.orthographicSize = Mathf.Lerp(startingSize, cameraUnzoomedSize, Mathf.Clamp(timer / cameraZoomSpeed, 0, 1));

        if (cameraUnzoomedSize - mainCam.orthographicSize <= 1)
        {
            zooming = true;
            camZoomed = false;

        }

    }

    void StartGame()
    {
        CreatePlanets(numPlanets);
        player.Launch();
        gameStarted = true;

    }

    void CreatePlanets(int planetsToCreate)
    {
        for (int i = 1; i <= planetsToCreate; i++)
        {
            GameObject p = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
            p.name = "Planet " + i;
            Planet planet = p.AddComponent<Planet>();
            planet.seed = seed;
            currentPlanets.Add(planet);
            Radar.Instance.CreateRadarObject(planet);

        }
    }

    public void MovePlanet(Planet planet)
    {
        Vector3 spawnPos = new Vector3();

        bool isValidPos = false;
        while (!isValidPos)
        {
            spawnPos = new Vector3(player.transform.position.x + Random.Range(-bounds.x, bounds.x), player.transform.position.y + Random.Range(0, bounds.y));

            if (Vector3.Distance(player.transform.position, spawnPos) >= minDistance && CheckPlanetDistance(spawnPos))
            {
                isValidPos = true;
            }
        }

        planet.transform.position = spawnPos;
    }

    private bool CheckPlanetDistance(Vector3 spawnPos)
    {
        bool check = true;
        for(int i = 0; i < currentPlanets.Count; i++)
        {
            if(Vector3.Distance(spawnPos, currentPlanets[i].transform.position) <= minPlanetSeperationDistance)
            {
                check = false;
                break;
            }
        }
        return check;
    }
    

}

