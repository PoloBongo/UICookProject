using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RaycastRecipe : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private static readonly int IsClose = Animator.StringToHash("IsClose");
    private GameObject curSelectedObj;
    private Animator curSelectedAnimator;
    private bool isOpen;
    private bool antiAutoOpen;
    
    private StockVariationMaterial stockMaterial;
    private MeshRenderer hitRenderer;
    private BoxCollider hitCollider;

    private void Start()
    {
        antiAutoOpen = false;
        isOpen = false;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector3 camPos = Camera.main.transform.position;
    
        RecipeLayerUpdate(camPos, ray);
    }

    private void StockSelectedObj(RaycastHit hitInfo, bool isClicked = false)
    {
        if (isClicked)
        {
            ResetStatsObj();
        }
        if (!curSelectedObj)
        {
            curSelectedObj = hitInfo.collider.gameObject.GetComponentInParent<Animator>().gameObject;
            curSelectedAnimator = curSelectedObj.GetComponent<Animator>();
            if (!curSelectedAnimator) Debug.LogError(curSelectedObj.name + " not found");
            Debug.Log(curSelectedObj.name);
        }
    }

    private void ResetStatsObj()
    {
        curSelectedObj = null;
    }
    
    private void RecipeLayerUpdate(Vector3 camPos, Ray ray)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 10f, LayerMask.GetMask("RecipeBook")))
        {
            SwitchMaterialWhenMouseIsHover(hitInfo);
            // stock l'obj ciblé
            StockSelectedObj(hitInfo);
            antiAutoOpen = false;
            
            if (Input.GetMouseButtonDown(0) && !antiAutoOpen)
            {
                if (!isOpen)
                {
                    hitCollider = hitInfo.transform.gameObject.GetComponentInChildren<BoxCollider>();
                    hitCollider.enabled = false;
                }
                OpenCloseRecipeBook();
                StockSelectedObj(hitInfo, true);
            }
        }
        else
        {
            ResetMaterialWhenMouseIsNotHover();
        }
    }

    public void OpenCloseRecipeBook()
    {
        curSelectedAnimator.ResetTrigger(isOpen ? IsClose : IsOpen);
        curSelectedAnimator.SetTrigger(isOpen ? IsClose : IsOpen);
        if (isOpen) hitCollider.enabled = true;
        isOpen = !isOpen;
    }
    
    private void SwitchMaterialWhenMouseIsHover(RaycastHit hitInfo)
    {
        hitRenderer = hitInfo.transform.gameObject.GetComponentInChildren<MeshRenderer>();
        if (hitRenderer)
        {
            // change le material
            stockMaterial = hitInfo.transform.gameObject.GetComponentInChildren<StockVariationMaterial>();
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

    public bool GetAntiAutoOpen()
    {
        return antiAutoOpen;
    }

    public void SetAntiAutoOpen(bool _antiAutoOpen)
    {
        antiAutoOpen = _antiAutoOpen;
    }
}
