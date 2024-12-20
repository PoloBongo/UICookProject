using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RaycastContainer : MonoBehaviour
{
    [SerializeField] private RaycastPickable raycastPickable;
    private GameObject curContainerPickedObj;
    private GameObject lastContainerPickedObj;
    private GameObject curPickedCanvas;
    private Camera chooseFreeCamForGetContainer;
    private string layerGetContainer;

    [Header("Settings Camera")]
    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;
    
    private Dictionary<MeshRenderer, StockVariationMaterial> rendererMaterials = new Dictionary<MeshRenderer, StockVariationMaterial>();
    private List<MeshRenderer> hitRenderers = new List<MeshRenderer>();
    
    // slots
    private List<Vector3> availableSlots = new List<Vector3>();
    private List<Vector3> allSlots = new List<Vector3>();
    private List<Vector3> occupiedSlots = new List<Vector3>();
    private int indexDropdownContainer = 0;
    private string actualTag;

    public delegate void SendGameObjectAssociateToUsedSlot(GameObject curObj, int slotIndex);
    public static event SendGameObjectAssociateToUsedSlot SendGameObjectAssociateToUsedSlotFunc;
    
    public delegate void OnShowNotification(string message);
    public static event OnShowNotification OnShowNotificationFunc;
    private void OnEnable()
    {
        ButtonClickContainer.OnContainerButtonClick += HandleContainerButtonClick;
        DropdownSlots.OnChangeIndexDropdownContainerFunc += HandleGetIndexDropdownContainer;
        Slots.OnSetupCopySlotPositionsFunc += SetupCopySlotPositions;
    }

    private void OnDisable()
    {
        ButtonClickContainer.OnContainerButtonClick -= HandleContainerButtonClick;
        DropdownSlots.OnChangeIndexDropdownContainerFunc -= HandleGetIndexDropdownContainer;
        Slots.OnSetupCopySlotPositionsFunc -= SetupCopySlotPositions;
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
    
    private void HandleContainerButtonClick(string _buttonName, GameObject _parent, bool _activeSlot)
    {
        GameObject _pickedObj = null;
        if (availableSlots.Count <= 0 && _activeSlot) return;
        
        switch (_buttonName)
        {
            case "DropLeftHand":
                if (leftHandCamera.transform.childCount > 0)
                {
                    _pickedObj = leftHandCamera.transform.GetChild(0).gameObject;
                    SwitchLayer(_pickedObj, _buttonName);
                    SwitchLayerMainFunc(_pickedObj, _buttonName);
                    SwitchParentChild(_parent, _pickedObj, _activeSlot);
                }
                break;
            case "DropRightHand":
                if (rightHandCamera.transform.childCount > 0)
                {
                    _pickedObj = rightHandCamera.transform.GetChild(0).gameObject;
                    SwitchLayer(_pickedObj, _buttonName);
                    SwitchLayerMainFunc(_pickedObj, _buttonName);
                    SwitchParentChild(_parent, _pickedObj, _activeSlot);
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

    private void HandleGetIndexDropdownContainer(int _index)
    {
        indexDropdownContainer = _index;
    }

    private void SwitchLayerMainFunc(GameObject _obj, string _layer)
    {
        for (int i = 0; i < _obj.transform.childCount; i++)
        {
            if (_obj.transform.GetChild(i).GetComponent<Canvas>())
            {
                continue;
            }
            SwitchLayer(_obj.transform.GetChild(i).gameObject, _layer);
        }
    }

    private void SwitchLayer(GameObject _obj, string _layer)
    {
        _obj.layer = LayerMask.NameToLayer(_layer);
        OnShowNotificationFunc?.Invoke("Vous venez de déposer : " + _obj.name);
    }

    private void SwitchParentChild(GameObject _parent, GameObject _pickedObj, bool _activeSlot)
    {
        _pickedObj.transform.parent = _parent.transform;
        if (_activeSlot)
        {
            PlaceOnSlot(_parent, _pickedObj);
        }
        //_pickedObj.transform.localPosition = new Vector3(0, 0, 0);
    }
    
    public void PlaceOnSlot(GameObject parent, GameObject pickedObj)
    {
        if (availableSlots.Count > 0)
        {
            Vector3 slotPosition = availableSlots[0];
            int slotIndex = allSlots.IndexOf(slotPosition);
            pickedObj.transform.SetParent(parent.transform);
            pickedObj.transform.localPosition = slotPosition;

            occupiedSlots.Add(slotPosition);
            SendGameObjectAssociateToUsedSlotFunc?.Invoke(pickedObj, slotIndex);
            availableSlots.RemoveAt(0);
        }
    }
    
    public void ReleaseSlot()
    {
        if (indexDropdownContainer >= 0 && indexDropdownContainer < occupiedSlots.Count)
        {
            Vector3 slotPosition = occupiedSlots[indexDropdownContainer];
            occupiedSlots.RemoveAt(indexDropdownContainer);
            availableSlots.Add(slotPosition);
        }
        else
        {
            Debug.Log("Index invalide : Impossible de libérer le slot.");
        }
    }
    
    public void ReleaseSlotWithIndex(int slotIndex = -1)
    {
        if (slotIndex != -1) indexDropdownContainer = slotIndex;
        if (indexDropdownContainer >= 0 && indexDropdownContainer < occupiedSlots.Count)
        {
            Vector3 slotPosition = occupiedSlots[indexDropdownContainer];
            occupiedSlots.RemoveAt(indexDropdownContainer);
            availableSlots.Add(slotPosition);
        }
        else
        {
            Debug.Log("Index invalide : Impossible de libérer le slot.");
        }
    }

    private void CanTakeIngredient(int _deletedIndex = -1)
    {
        ReleaseSlot();
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
        
        if (curPickedCanvas && Vector3.Distance(transform.position, curContainerPickedObj.transform.position) > 2.5f)
        {
            ResetStatsObj();
        }
    }

    private void StockPickedObj(RaycastHit hitInfo, bool _isClicked = false)
    {
        if (availableSlots.Count <= 0)
        {
            var slotsComponent = hitInfo.transform.gameObject.GetComponent<Slots>()
                                 ?? hitInfo.transform.gameObject.GetComponentInChildren<Slots>();
            availableSlots = slotsComponent.SlotPositions;
        }
        if (_isClicked)
        {
            if (!hitInfo.transform.gameObject.CompareTag(actualTag))
            {
                var slotsComponent = hitInfo.transform.gameObject.GetComponent<Slots>() 
                                     ?? hitInfo.transform.gameObject.GetComponentInChildren<Slots>();
                if (slotsComponent) availableSlots = slotsComponent.SlotPositions;
            }
            ResetStatsObj();
        }
        if (!curContainerPickedObj)
        {
            curContainerPickedObj = hitInfo.collider.gameObject;
            curPickedCanvas = hitInfo.collider.gameObject.GetComponentInChildren<Canvas>(true).gameObject;
        }
    }

    private void SetupCopySlotPositions(List<Vector3> _slotPos)
    {
        allSlots = new List<Vector3>(_slotPos);
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
        if (Physics.Raycast(ray, out hitInfo, 2.5f, LayerMask.GetMask("Container")))
        {
            actualTag = hitInfo.transform.gameObject.tag;
            SwitchMaterialWhenMouseIsHover(hitInfo);
            // stock l'obj ciblé
            StockPickedObj(hitInfo);

            if (Input.GetMouseButtonDown(0))
            {
                lastContainerPickedObj = hitInfo.transform.gameObject;
                StockPickedObj(hitInfo, true);
                if (curPickedCanvas)
                {
                    //curPickedCanvas.transform.LookAt(new Vector3(transform.position.x, curPickedCanvas.transform.position.y, transform.position.z));
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
        MeshRenderer[] renderers = hitInfo.transform.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in renderers)
        {
            StockVariationMaterial stockMaterial = meshRenderer.gameObject.GetComponent<StockVariationMaterial>();
            if (stockMaterial)
            {
                hitRenderers.Add(meshRenderer);
                rendererMaterials[meshRenderer] = stockMaterial; 
                meshRenderer.material = stockMaterial.GetHoverMaterial();
            }
        }
    }

    private void ResetMaterialWhenMouseIsNotHover()
    {
        foreach (MeshRenderer meshRenderer in hitRenderers)
        {
            if (rendererMaterials.ContainsKey(meshRenderer))
            {
                if (meshRenderer) meshRenderer.material = rendererMaterials[meshRenderer].GetDefaultMaterial();
            }
        } 
        hitRenderers.Clear(); 
        rendererMaterials.Clear();
    }
}
