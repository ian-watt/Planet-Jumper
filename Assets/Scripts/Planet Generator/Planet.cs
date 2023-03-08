using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 80;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    public Seed seed;
    public ShapeSettings seedSettings;

    public Material mat;
    public Shader shad;

    public GameObject borderObject;


    private void Awake()
    {
        shad = Shader.Find("Planet");
        mat = new Material(shad);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) > GameManager.Instance.maxPlanetRenderDistance)
        {
            GameManager.Instance.MovePlanet(this);
            GenerateColours(seed.GenerateRandomColourSettings());
        }
    }

    [ContextMenu("Randomize Planet")]
    private void GenerateRandomPlanet()
    {
        seedSettings = seed.GetSettings();
        GeneratePlanet(seed.GetSettings(), seed.GenerateRandomColourSettings());
    }

    private void Start()
    {

        GeneratePlanet(seed.GetSettings(), seed.GenerateRandomColourSettings());
        GameManager.Instance.MovePlanet(this);
    }

    void Initialize(ShapeSettings settings, ColourSettings colourSettings)
    {
        shapeGenerator.UpdateSettings(settings);
        colourGenerator.UpdateSettings(colourSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                meshObj.AddComponent<MeshCollider>();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = mat;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet(ShapeSettings settings, ColourSettings colourSettings)
    {
        Initialize(settings, colourSettings);
        GenerateMesh();
        GenerateColours(seed.GenerateRandomColourSettings());
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize(shapeSettings, colourSettings);
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize(shapeSettings, colourSettings);
            GenerateColours(seed.GenerateRandomColourSettings());
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax, this);
    }

    void GenerateColours(ColourSettings settings)
    {
        colourGenerator.UpdateColours(this, seed.GenerateRandomColourSettings());
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colourGenerator);
            }
        }
    }
}
