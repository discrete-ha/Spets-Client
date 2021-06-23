using System;
using Google.Maps.Coord;
using Google.Maps.Feature.Style;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spets.Airflare.Shared;

[RequireComponent(
    typeof(DynamicMapsService), typeof(BuildingTexturer), typeof(EmissionController))]
public sealed class CityView : MonoBehaviour
{
    public SunAndMoon SunAndMoonController;
    public GameObject Avatar;
    
    
    public static DynamicMapsService DynamicMapsService;

    public Material SegmentMaterial;
    public Material BorderMaterial;
    public Material RegionMaterial;

    private SegmentStyle OutlineStyleConfiguration;

    private void Start()
    {
        // Verify that a Sun and Moon Controller has been defined.
        if (SunAndMoonController == null)
        {
            return;
        }
        //Input.compass.enabled = true;

        BuildingTexturer buildingTexturer = GetComponent<BuildingTexturer>();
        GetComponent<EmissionController>().SetMaterials(buildingTexturer.WallMaterials);

        DynamicMapsService = GetComponent<DynamicMapsService>();

        //DynamicMapsService.MapsService.Events.ExtrudedStructureEvents.DidCreate.AddListener(
        //    args => buildingTexturer.AssignNineSlicedMaterials(args.GameObject));

        DynamicMapsService.MapsService.Events.MapEvents.Loaded.AddListener(
                  args => {
                      SunAndMoonController.setupTime();
                  }
                );

        Shader standardShader = Shader.Find("Google/Maps/Shaders/Standard");
        Material wallMaterial = new Material(standardShader) { color = new Color(1f, 0.75f, 0.5f) };
        Material roofMaterial = new Material(standardShader) { color = new Color(1f, 0.8f, 0.6f) };

        Shader baseMapShader = Shader.Find("Google/Maps/Shaders/BaseMap Color");
     

        Material waterMaterial = new Material(baseMapShader)
        {
            color = new Color(0.0f, 1.0f, 1.0f),
        };
        waterMaterial.SetFloat("_Glossiness", 1f);

        //Material intersectionMaterial = new Material(baseMapShader)
        //{
        //    color = new Color(0.4f, 0.4f, 0.4f),
        //};
        //intersectionMaterial.SetFloat("_Glossiness", 0.5f);

        //Shader segmentShader = Shader.Find("Google/Maps/Shaders/BaseMap Color");
        //Material segmentMaterial = new Material(baseMapShader)
        //{
        //    color = new Color(0.5f, 0.5f, 0.5f),
        //};
        //SegmentMaterial.SetFloat("_Glossiness", 0.5f);

        //Material borderMaterial = new Material(baseMapShader)
        //{
        //    color = new Color(0.5f, 0.5f, 0.5f),
        //};
        //BorderMaterial.SetFloat("_Glossiness", 0.5f);

        DynamicMapsService.MapsService.Events.SegmentEvents.WillCreate.AddListener(arg0 => {
            var StyleConfiguration = new SegmentStyle.Builder
            {
                Material = this.SegmentMaterial,
                Width = 8,
                BorderMaterial = this.BorderMaterial,
                BorderWidth = 2
            }.Build();

            arg0.Style = StyleConfiguration;
        });

        DynamicMapsService.MapsService.Events.RegionEvents.WillCreate.AddListener((arg0) => {
            OutlineStyleConfiguration = ExampleDefaults.DefaultGameObjectOptions.SegmentStyle;
            var regionStyleBuilder = new RegionStyle.Builder()
            {
                //FillMaterial = RegionMaterial,
                Fill = true,
                FillMaterial = RegionMaterial
            }.Build();

            arg0.Style = regionStyleBuilder;
        });

        //DynamicMapsService.MapsService.Events.TerrainEvents

    }

    

}