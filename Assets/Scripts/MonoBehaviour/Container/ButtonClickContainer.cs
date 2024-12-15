using UnityEngine;
using UnityEngine.UI;

public class ButtonClickContainer : MonoBehaviour
{
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject parent;
    public delegate void OnContainerClick(string buttonName, GameObject parent);
    public static event OnContainerClick OnContainerButtonClick;
    void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        OnContainerButtonClick?.Invoke(gameObject.name, parent);
        canvasUI.SetActive(false);
    }
}
