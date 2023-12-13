using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;

public class FoodPlacementManager : MonoBehaviour
{
    public GameObject SpawnableFood;
    public XROrigin sessionOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private int itemcnt;

    private void Start()
    {
        itemcnt = 0;
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                bool collision = raycastManager.Raycast(Input.GetTouch(0).position, raycastHits,
                TrackableType.PlaneWithinPolygon);

                if (collision && isButtonPressed() == false && itemcnt == 0)
                {
                    GameObject _object = Instantiate(SpawnableFood);
                    _object.transform.position = raycastHits[0].pose.position;
                    _object.transform.rotation = raycastHits[0].pose.rotation;
                    itemcnt = 1;
                }

                foreach (var planes in planeManager.trackables)
                {
                    planes.gameObject.SetActive(false);
                }

                planeManager.enabled = false;
            }
        }
    }

    public void SwitchFood(GameObject food)
    {
        SpawnableFood = food;
        itemcnt = 0;
    }

    public bool isButtonPressed()
    {
        if (EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

