using UnityEngine;
using UnityEngine.UI;

public class ButtonClickContainer : MonoBehaviour
{
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject parent;
    [SerializeField] private bool activeSlot;
    public delegate void OnContainerClick(string buttonName, GameObject parent, bool _activeSlot);
    public static event OnContainerClick OnContainerButtonClick;
    void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        OnContainerButtonClick?.Invoke(gameObject.name, parent, activeSlot);
        canvasUI.SetActive(false);
    }
}
