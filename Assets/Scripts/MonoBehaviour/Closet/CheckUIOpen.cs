using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckUIOpen : MonoBehaviour
{
    private bool itemDescriptionIsOpen = false;
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
        selectedCheck = null;
        InOutElasticY.OnFinishMotionElastic += HandleCheckCanClick;
        ContentCloset.OnShowIconCheck += OnCloseOpenItemDescription;
    }

    private void OnDisable()
    {
        InOutElasticY.OnFinishMotionElastic -= HandleCheckCanClick;
        ContentCloset.OnShowIconCheck -= OnCloseOpenItemDescription;
        foreach (var tiroir in allTiroirs)
        {
            tiroir.SetActive(false);
        }
    }

    private void HandleCheckCanClick(bool _canClick)
    {
        canClick = _canClick;
    }

    private void OnCloseOpenItemDescription(GameObject _check, bool _isShow, bool _isShowIcon)
    {
        panelItemDescription.SetActive(_isShowIcon);
            
        inOutElasticYCloseForm.StartAnimationBounce();
        inOutElasticYGlobalPanel.StartAnimationBounce();
        inOutElasticYInfoPanel.StartAnimationBounce();
        
        _check.SetActive(_isShowIcon);
            
        selectedCheck = _check;
    }

    public void CloseClosetCheckUI()
    {
        if (panelItemDescription.activeSelf)
        {
            panelItemDescription.SetActive(false);
            inOutElasticYCloseForm.StartAnimationBounce();
            inOutElasticYGlobalPanel.StartAnimationBounce();
            inOutElasticYInfoPanel.StartAnimationBounce();
        }
    }
}
