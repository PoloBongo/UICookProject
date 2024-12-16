using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RaycastPickable : MonoBehaviour
{
    private GameObject curPickedObj;
    private GameObject curPickedCanvas;

    [Header("Settings Camera")]
    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;
    
    private StockVariationMaterial stockMaterial;
    private MeshRenderer hitRenderer;
    
    private int chooseHand;

    private void Start()
    {
        chooseHand = -1;
    }

    private void OnEnable()
    {
        ButtonClickPickable.OnButtonClick += HandleButtonClick;
        ContentCloset.OnSelectedHand += HandleButtonClick;
    }

    private void OnDisable()
    {
        ButtonClickPickable.OnButtonClick -= HandleButtonClick;
        ContentCloset.OnSelectedHand -= HandleButtonClick;
    }

    private bool CheckChildInHand()
    {
        if (chooseHand == -1) return false;

        Camera handCamera = chooseHand == 0 ? leftHandCamera : chooseHand == 1 ? rightHandCamera : null;
    
        if (handCamera != null && handCamera.transform.childCount > 0)
        {
            return true;
        }
        return false;
    }

    private void HandleButtonClick(string _buttonName, GameObject _curPickedObj = null)
    {
        if (_curPickedObj)
        {
            curPickedObj = Instantiate(_curPickedObj);
        }
        switch (_buttonName)
        {
            case "LeftHand":
                chooseHand = 0;
                break;
            case "RightHand":
                chooseHand = 1;
                break;
        }
        
        if (!CheckChildInHand())
        {
            SwitchLayer(curPickedObj, _buttonName);
            for (int i = 0; i < curPickedObj.transform.childCount; i++)
            {
                if (curPickedObj.transform.GetChild(i).GetComponent<Canvas>())
                {
                    break;
                }
                SwitchLayer(curPickedObj.transform.GetChild(i).gameObject, _buttonName);
            }

            SwitchParent();
        }
    }

    private void SwitchLayer(GameObject _obj, string _layer)
    {
        _obj.layer = LayerMask.NameToLayer(_layer);
    }

    private void SwitchParent()
    {
        curPickedObj.GetComponentInChildren<MeshCollider>().enabled = false;
        curPickedObj.transform.parent = chooseHand switch
        {
            0 => leftHandCamera.transform,
            1 => rightHandCamera.transform,
            _ => curPickedObj.transform.parent
        };
        curPickedObj.transform.localPosition = new Vector3(0, 0, 2);
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    
        PickableLayerUpdate(ray);
        
        if (curPickedCanvas && Vector3.Distance(transform.position, curPickedObj.transform.position) > 5)
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
        if (!curPickedObj)
        {
            curPickedObj = hitInfo.collider.gameObject;
            curPickedCanvas = hitInfo.collider.gameObject.GetComponentInChildren<Canvas>(true).gameObject;
            Debug.Log(curPickedCanvas.name);
            Debug.Log(curPickedObj.name);
        }
    }

    private void ResetStatsObj()
    {
        curPickedCanvas.SetActive(false);
        curPickedObj = null;
        curPickedCanvas = null;
    }

    private void PickableLayerUpdate(Ray ray)
    {
        RaycastHit hitInfo;
        //Debug.DrawRay(camPos, ray.direction.normalized * 10f, Color.yellow);
        if (Physics.Raycast(ray, out hitInfo, 10f, LayerMask.GetMask("Pickable")))
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
