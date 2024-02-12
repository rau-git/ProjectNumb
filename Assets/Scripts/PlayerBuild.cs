using System;
using FishNet;
using FishNet.Object;
using FishNet.Utility.Extension;
using UnityEngine;
using UnityEngine.Video;

public class PlayerBuild : NetworkBehaviour
{
    [SerializeField] private GameObject selectedBuildingPrefab;
    [SerializeField] private GameObject selectedGhostPrefab;
    [SerializeField] private GameObject buildLocation;
    private GameObject ghostObject;
    private PlayerIngameControls playerControls;
    private bool buildMode;
    private Vector3 ghostScale = new Vector3(1.0f, 1.0f, 1.0f);

    private void Start()
    {
        playerControls = new PlayerIngameControls();
        playerControls.Enable();
        buildMode = false;
    }

    private void Update()
    {
        BuildMode();
        
        if (playerControls.PlayerControls.BuildMenu.WasPressedThisFrame())
        {
            ToggleBuilding();
        }
    }

    private void ToggleBuilding()
    {
        buildMode = !buildMode;

        if (buildMode)
        {
            ghostObject = Instantiate(selectedGhostPrefab, buildLocation.transform.position, buildLocation.transform.rotation, buildLocation.transform);
        }
        else
        {
            Destroy(ghostObject);
        }
    }

    private void BuildMode()
    {
        if (!buildMode) return;
        
        if (!playerControls.PlayerControls.PrimaryFire.WasPressedThisFrame()) return;

        var builtObject = Instantiate(selectedBuildingPrefab, ghostObject.transform.position, ghostObject.transform.rotation);
        PlaceBuilding(builtObject);
    }

    public void SetGhostTransform(Transform transformToCopy)
    {
        ghostObject.transform.parent = transformToCopy;
        ghostObject.transform.position = transformToCopy.transform.position;
        ghostObject.transform.rotation = transformToCopy.transform.rotation;
        ghostObject.transform.localScale = ghostScale;
    }

    [Server]
    private void PlaceBuilding(GameObject spawnGameObject)
    {
        InstanceFinder.ServerManager.Spawn(spawnGameObject);
    }

}
