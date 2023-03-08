using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Seed : ScriptableObject
{
    public ShapeSettings minSettings;
    public ShapeSettings maxSettings;
    public ColourSettings[] colourSettings;


    public ShapeSettings GetSettings()
    {
        Random.InitState((int)(Time.realtimeSinceStartup * 10000f));

        ShapeSettings newSettings = ScriptableObject.CreateInstance<ShapeSettings>();
        newSettings.planetRadius = 5.43f;
        newSettings.noiseLayers = new ShapeSettings.NoiseLayer[minSettings.noiseLayers.Length];
        Debug.Log(newSettings.noiseLayers.Length);
        for (int i = 0; i < newSettings.noiseLayers.Length; i++)
        {
            newSettings.noiseLayers[i] = new ShapeSettings.NoiseLayer();
            newSettings.noiseLayers[i].enabled = true;

            newSettings.noiseLayers[i].useFirstLayerAsMask = (i != 0);

            NoiseSettings settings = new NoiseSettings();

            settings.filterType = minSettings.noiseLayers[i].noiseSettings.filterType;
            settings.simpleNoiseSettings = new NoiseSettings.SimpleNoiseSettings();

            settings.simpleNoiseSettings.strength = minSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.strength;
            settings.simpleNoiseSettings.numLayers = minSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.numLayers;
            settings.simpleNoiseSettings.baseRoughness = (Random.Range(minSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.baseRoughness, maxSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.baseRoughness));
            settings.simpleNoiseSettings.roughness = minSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.roughness;
            settings.simpleNoiseSettings.persistence = minSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.persistence;
            settings.simpleNoiseSettings.centre = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            settings.simpleNoiseSettings.minValue = (Random.Range(minSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.minValue, maxSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.minValue));

            if (settings.filterType == NoiseSettings.FilterType.Ridgid)
            {
                settings.ridgidNoiseSettings = new NoiseSettings.RidgidNoiseSettings();

                settings.ridgidNoiseSettings.strength = (Random.Range(minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.strength, maxSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.strength));
                settings.ridgidNoiseSettings.numLayers = (Random.Range(minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.numLayers, maxSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.numLayers));
                settings.ridgidNoiseSettings.baseRoughness = (Random.Range(minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.baseRoughness, maxSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.baseRoughness));
                settings.ridgidNoiseSettings.roughness = (Random.Range(minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.baseRoughness, maxSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.baseRoughness));
                settings.ridgidNoiseSettings.persistence = (Random.Range(minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.baseRoughness, maxSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.baseRoughness));
                settings.ridgidNoiseSettings.centre = minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.centre;
                settings.ridgidNoiseSettings.minValue = (Random.Range(minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.minValue, maxSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.minValue));
                settings.ridgidNoiseSettings.weightMultiplier = (Random.Range(minSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.weightMultiplier, maxSettings.noiseLayers[i].noiseSettings.ridgidNoiseSettings.weightMultiplier));

            }

            newSettings.noiseLayers[i].noiseSettings = settings;
        }


        return newSettings;
    }

   public ColourSettings GenerateRandomColourSettings()
    {
        ColourSettings newSettings = colourSettings[Random.Range(0, colourSettings.Length)];

        return newSettings;
    }

}
