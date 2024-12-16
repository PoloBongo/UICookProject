using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckUIOpen : MonoBehaviour
{
    private bool itemDescriptionIsOpen;
    private GameObject selectedCheck;
    
    [SerializeField] private GameObject panelItemDescription;
    [SerializeField] private InOutElasticY inOutElasticYCloseForm;
    [SerializeField] private InOutElasticY inOutElasticYGlobalPanel;
    [SerializeField] private InOutElasticY inOutElasticYInfoPanel;
    [SerializeField] private List<GameObject> allTiroirs;

    private bool canClick = true;
    private void OnEnable()
    {
        canClick = true;
        itemDescriptionIsOpen = false;
        selectedCheck = null;
        InOutElasticY.OnFinishMotionElastic += HandleCheckCanClick;
    }

    private void OnDisable()
    {
        InOutElasticY.OnFinishMotionElastic -= HandleCheckCanClick;
        foreach (var tiroir in allTiroirs)
        {
            tiroir.SetActive(false);
        }
    }

    private void HandleCheckCanClick(bool _canClick)
    {
        canClick = _canClick;
    }

    public void OnCloseOpenItemDescription(GameObject _check)
    {
        if (canClick)
        {
            itemDescriptionIsOpen = !itemDescriptionIsOpen;
            if (selectedCheck && selectedCheck != _check) itemDescriptionIsOpen = false;
            panelItemDescription.SetActive(itemDescriptionIsOpen);
            
            inOutElasticYCloseForm.StartAnimationBounce();
            inOutElasticYGlobalPanel.StartAnimationBounce();
            inOutElasticYInfoPanel.StartAnimationBounce();
            
            selectedCheck = _check;
            canClick = false;
        }
    }

    public void CloseClosetCheckUI()
    {
        if (itemDescriptionIsOpen)
        {
            panelItemDescription.SetActive(false);
            inOutElasticYCloseForm.StartAnimationBounce();
            inOutElasticYGlobalPanel.StartAnimationBounce();
            inOutElasticYInfoPanel.StartAnimationBounce();
        }
    }
}
