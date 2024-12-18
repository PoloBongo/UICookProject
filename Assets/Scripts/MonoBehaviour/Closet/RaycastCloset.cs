using System.Collections.Generic;
using UnityEngine;

public class RaycastCloset : MonoBehaviour
{
    private GameObject curClosetPickedObj;
    private GameObject curPickedCanvas;

    [Header("Settings Camera")]
    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;
    
    private Dictionary<MeshRenderer, StockVariationMaterial> rendererMaterials = new Dictionary<MeshRenderer, StockVariationMaterial>();
    private List<MeshRenderer> hitRenderers = new List<MeshRenderer>();
    
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    
        ClosetLayerUpdate(ray);
        
        if (curPickedCanvas && Vector3.Distance(transform.position, curClosetPickedObj.transform.position) > 5)
        {
            ResetStatsObj();
        }
    }

    private void StockPickedObj(RaycastHit hitInfo, bool isClicked = false)
    {
        if (isClicked)
        {
            curClosetPickedObj.layer = LayerMask.NameToLayer("Default");
            ResetStatsObj();
        }
        if (!curClosetPickedObj)
        {
            curClosetPickedObj = hitInfo.collider.gameObject;
            curPickedCanvas = hitInfo.collider.gameObject.GetComponentInChildren<Canvas>(true).gameObject;
        }
    }

    private void ResetStatsObj()
    {
        curPickedCanvas.SetActive(false);
        curClosetPickedObj = null;
        curPickedCanvas = null;
    }
    
    private void ClosetLayerUpdate(Ray ray)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 10f, LayerMask.GetMask("Closet")))
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
                meshRenderer.material = rendererMaterials[meshRenderer].GetDefaultMaterial();
            }
        } 
        hitRenderers.Clear(); 
        rendererMaterials.Clear();
    }
}
