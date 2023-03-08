using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject
{

    public Material planetMaterial;
    public Shader shader;
    public BiomeColourSettings biomeColourSettings;
    public Gradient oceanColour;

    [System.Serializable]
    public class BiomeColourSettings
    {
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0, 1)]
            public float startHeight;
            [Range(0, 1)]
            public float tintPercent;
        }
    }

    private void Awake()
    {
        Gradient g = GenerateRandomGradient();

        planetMaterial = new Material(shader);
        oceanColour = GenerateRandomGradient();
        for (int i = 0; i < biomeColourSettings.biomes.Length; i++)
        {
            biomeColourSettings.biomes[i].gradient = g;
        }
    }

    private Gradient GenerateRandomGradient()
    {
        Random.InitState((int)(Time.realtimeSinceStartup * 10000f));


        Gradient newGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[7];

        // Generate a random starting hue
        float hue = Random.Range(0f, 1f);

        for (int i = 0; i < colorKeys.Length; i++)
        {
            // Set the color key at a uniform time position
            colorKeys[i].time = (float)i / (colorKeys.Length - 1);

            // Generate a random color with a similar hue to the previous color
            float saturation = Random.Range(0.5f, 1f);
            float brightness = Random.Range(0.5f, 1f);
            float hueVariation = Random.Range(-0.1f, 0.1f);
            hue += hueVariation;
            hue = Mathf.Repeat(hue, 1f);
            Color color = Color.HSVToRGB(hue, saturation, brightness);

            // Set the color key
            colorKeys[i].color = color;
        }

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = 1f;

        newGradient.SetKeys(colorKeys, alphaKeys);

        return newGradient;
    }
}
