using FishNet;
using FishNet.Object;
using UnityEngine;

public class PlayerBuild : NetworkBehaviour
{
    [HideInInspector] public string canSnapTo;
    private GameObject ghostObject;
    private PlayerIngameControls playerControls;
    private bool buildMode;
    private Vector3 ghostScale = new Vector3(1.0f, 1.0f, 1.0f);

    [Header("Selected Building")]
    [SerializeField] private GameObject selectedBuildingPrefab;
    [SerializeField] private GameObject selectedGhostPrefab;
    [SerializeField] private GameObject buildLocation;
    
    [Header("Building Objects")] 
    [SerializeField] private GameObject foundationPrefab;
    [SerializeField] private GameObject foundationGhostPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject wallGhostPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject doorGhostPrefab;
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private GameObject windowGhostPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        if (!IsOwner)
        {
            Destroy(this);
            Destroy(buildLocation.gameObject);
        }
    }

    private void Start()
    {
        playerControls = new PlayerIngameControls();
        playerControls.Enable();
        buildMode = false;
        buildLocation.gameObject.SetActive(false);
        canSnapTo = selectedBuildingPrefab.GetComponent<SnapCompatibility>().snapTo;
    }

    private void Update()
    {
        SwitchBuildObject();
        BuildMode();
        
        if (playerControls.PlayerControls.BuildMenu.WasPressedThisFrame())
        {
            ToggleBuilding();
        }
    }

    private void SwitchBuildObject()
    {
        if (playerControls.PlayerControls.Foundation.WasPressedThisFrame())
        {
            selectedBuildingPrefab = foundationPrefab;
            selectedGhostPrefab = foundationGhostPrefab;
            canSnapTo = selectedBuildingPrefab.GetComponent<SnapCompatibility>().snapTo;
            ToggleBuilding();
            ToggleBuilding();
        }
        if (playerControls.PlayerControls.Wall.WasPressedThisFrame())
        {
            selectedBuildingPrefab = wallPrefab;
            selectedGhostPrefab = wallGhostPrefab;
            canSnapTo = selectedBuildingPrefab.GetComponent<SnapCompatibility>().snapTo;
            ToggleBuilding();
            ToggleBuilding();
        }
        if (playerControls.PlayerControls.Door.WasPressedThisFrame())
        {
            selectedBuildingPrefab = doorPrefab;
            selectedGhostPrefab = doorGhostPrefab;
            canSnapTo = selectedBuildingPrefab.GetComponent<SnapCompatibility>().snapTo;
            ToggleBuilding();
            ToggleBuilding();
        }
        if (playerControls.PlayerControls.Window.WasPressedThisFrame())
        {
            selectedBuildingPrefab = windowPrefab;
            selectedGhostPrefab = windowGhostPrefab;
            canSnapTo = selectedBuildingPrefab.GetComponent<SnapCompatibility>().snapTo;
            ToggleBuilding();
            ToggleBuilding();
        }
    }

    private void ToggleBuilding()
    {
        buildMode = !buildMode;
        buildLocation.gameObject.SetActive(buildMode);

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
        ghostObject.transform.position = transformToCopy.position;
        ghostObject.transform.rotation = transformToCopy.rotation;
        ghostObject.transform.localScale = ghostScale;
    }

    [Server]
    private void PlaceBuilding(GameObject spawnGameObject)
    {
        InstanceFinder.ServerManager.Spawn(spawnGameObject);
    }

}
