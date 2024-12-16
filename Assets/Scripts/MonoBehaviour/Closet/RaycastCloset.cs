using UnityEngine;

public class RaycastCloset : MonoBehaviour
{
    private GameObject curClosetPickedObj;
    private GameObject curPickedCanvas;

    [Header("Settings Camera")]
    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;
    
    private StockVariationMaterial stockMaterial;
    private MeshRenderer hitRenderer;
    
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
