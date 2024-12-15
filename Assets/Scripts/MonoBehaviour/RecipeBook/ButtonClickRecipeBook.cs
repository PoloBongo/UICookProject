using UnityEngine;
using UnityEngine.UI;

public class ButtonClickRecipeBook : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    [SerializeField] private GameObject canvasUI;
    public delegate void OnRecipeBookClick(string buttonName);
    public static event OnRecipeBookClick OnButtonRecipeBookClick;
    void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        OnButtonRecipeBookClick?.Invoke(gameObject.name);
        canvasUI.SetActive(false);
    }
}
