using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RaycastContainer : MonoBehaviour
{
    [SerializeField] private RaycastPickable raycastPickable;
    private GameObject curContainerPickedObj;
    private GameObject curPickedCanvas;
    private Camera chooseFreeCamForGetContainer;
    private string layerGetContainer;

    [Header("Settings Camera")]
    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;
    
    private StockVariationMaterial stockMaterial;
    private MeshRenderer hitRenderer;
    
    private void OnEnable()
    {
        ButtonClickContainer.OnContainerButtonClick += HandleContainerButtonClick;
    }

    private void OnDisable()
    {
        ButtonClickContainer.OnContainerButtonClick -= HandleContainerButtonClick;
    }

    private bool CheckChildInHand()
    {
        switch (leftHandCamera.transform.childCount)
        {
            case > 0 when rightHandCamera.transform.childCount > 0:
                return false;
            case <= 0:
                chooseFreeCamForGetContainer = leftHandCamera;
                layerGetContainer = "LeftHand";
                return true;
        }

        if (rightHandCamera.transform.childCount > 0) return true;
        chooseFreeCamForGetContainer = rightHandCamera;
        layerGetContainer = "RightHand";
        return true;
    }
    
    private void HandleContainerButtonClick(string _buttonName, GameObject _parent)
    {
        GameObject _pickedObj = null;

        switch (_buttonName)
        {
            case "DropLeftHand":
                if (leftHandCamera.transform.childCount > 0)
                {
                    _pickedObj = leftHandCamera.transform.GetChild(0).gameObject;
                    SwitchLayer(_pickedObj, _buttonName);
                    SwitchLayerMainFunc(_pickedObj, _buttonName);
                    SwitchParentChild(_parent, _pickedObj);
                }
                break;
            case "DropRightHand":
                if (rightHandCamera.transform.childCount > 0)
                {
                    _pickedObj = rightHandCamera.transform.GetChild(0).gameObject;
                    SwitchLayer(_pickedObj, _buttonName);
                    SwitchLayerMainFunc(_pickedObj, _buttonName);
                    SwitchParentChild(_parent, _pickedObj);
                }
                break;
            case "Hand":
                if (CheckChildInHand())
                {
                    SwitchLayer(curContainerPickedObj, layerGetContainer);
                    SwitchLayerMainFunc(curContainerPickedObj, layerGetContainer);
                    SwitchParent(chooseFreeCamForGetContainer.gameObject, curContainerPickedObj);
                }
                break;
        }
    }

    private void SwitchLayerMainFunc(GameObject _obj, string _layer)
    {
        for (int i = 0; i < _obj.transform.childCount; i++)
        {
            if (_obj.transform.GetChild(i).GetComponent<Canvas>())
            {
                continue;
            }
            Debug.Log(_obj.transform.gameObject.name + " is " + _layer);
            SwitchLayer(_obj.transform.GetChild(i).gameObject, _layer);
        }
    }

    private void SwitchLayer(GameObject _obj, string _layer)
    {
        _obj.layer = LayerMask.NameToLayer(_layer);
    }

    private void SwitchParentChild(GameObject _parent, GameObject _pickedObj)
    {
        _pickedObj.transform.parent = _parent.transform;
        _pickedObj.transform.localPosition = new Vector3(0, 0, 0);
    }
    
    private void SwitchParent(GameObject _parent, GameObject _pickedObj)
    {
        _pickedObj.GetComponentInChildren<MeshCollider>().enabled = false;
        _pickedObj.transform.parent = _parent.transform;
        _pickedObj.transform.localPosition = new Vector3(0, 0, 2);
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    
        ContainerLayerUpdate(ray);
        
        if (curPickedCanvas && Vector3.Distance(transform.position, curContainerPickedObj.transform.position) > 5)
        {
            ResetStatsObj();
        }
    }

    private void StockPickedObj(RaycastHit hitInfo, bool isClicked = false)
    {
        if (isClicked)
        {
            ResetStatsObj();
        }
        if (!curContainerPickedObj)
        {
            curContainerPickedObj = hitInfo.collider.gameObject;
            curPickedCanvas = hitInfo.collider.gameObject.GetComponentInChildren<Canvas>(true).gameObject;
            Debug.Log(curPickedCanvas.name);
            Debug.Log(curContainerPickedObj.name);
        }
    }

    private void ResetStatsObj()
    {
        curPickedCanvas.SetActive(false);
        curContainerPickedObj = null;
        curPickedCanvas = null;
    }
    
    private void ContainerLayerUpdate(Ray ray)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 10f, LayerMask.GetMask("Container")))
        {
            SwitchMaterialWhenMouseIsHover(hitInfo);

            // stock l'obj ciblé
            StockPickedObj(hitInfo);

            if (Input.GetMouseButtonDown(0))
            {
                StockPickedObj(hitInfo, true);
                if (curPickedCanvas)
                {
                    curPickedCanvas.transform.LookAt(new Vector3(transform.position.x, curPickedCanvas.transform.position.y, transform.position.z));
                    curPickedCanvas.SetActive(true);
                }
            }
        }
        else
        {
            ResetMaterialWhenMouseIsNotHover();
        }
    }

    private void SwitchMaterialWhenMouseIsHover(RaycastHit hitInfo)
    {
        hitRenderer = hitInfo.transform.gameObject.GetComponent<MeshRenderer>();
        if (hitRenderer)
        {
            // change le material
            stockMaterial = hitInfo.transform.gameObject.GetComponent<StockVariationMaterial>();
            hitRenderer.material = stockMaterial.GetHoverMaterial();
        }
    }

    private void ResetMaterialWhenMouseIsNotHover()
    {
        if (hitRenderer)
        {
            hitRenderer.material = stockMaterial.GetDefaultMaterial();
        }
    }
}
