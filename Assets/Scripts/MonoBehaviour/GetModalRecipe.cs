using System;
using TMPro;
using UnityEngine;

public class GetModalRecipe : MonoBehaviour
{
   [SerializeField] private GameObject modalPanel;
   
   [SerializeField] private TMP_Text inputFieldTitle;
   [SerializeField] private TMP_Text inputFieldDescription;

   private string stockTitle;
   private string stockDescription;
   
   private InputActionManager inputActionManager;
   private PlayerInputAction playerInputAction;
   
   public delegate void OnShowNotification(string message);
   public static event OnShowNotification OnShowNotificationFunc;

   public void SetStockTitle(TMP_Text _text)
   {
      stockTitle = _text.text;
      inputFieldTitle.text = _text.text;
   }
   
   public void SetStockDescription(TMP_Text _text)
   {
      stockDescription = _text.text;
      inputFieldDescription.text = _text.text;
   }

   public void CloseModal()
   {
      if (!inputActionManager) inputActionManager = InputActionManager.Instance;
      playerInputAction = inputActionManager.GetPlayerInputAction();
      modalPanel.SetActive(false);
      playerInputAction.Player.Enable();
   }
   
   public void OpenModal()
   {
      if (!inputActionManager) inputActionManager = InputActionManager.Instance;
      playerInputAction = inputActionManager.GetPlayerInputAction();
      if (stockTitle != "" && stockDescription != "" && stockTitle != stockDescription)
      {
         modalPanel.SetActive(true);
         playerInputAction.Player.Disable();
         OnShowNotificationFunc?.Invoke("Descriptif de la recette ouverte avec succès!");
      }
      else
      {
         modalPanel.SetActive(false);
         playerInputAction.Player.Enable();
         OnShowNotificationFunc?.Invoke("Impossible d'ouvrir une page vide!");
      }
   }
}
