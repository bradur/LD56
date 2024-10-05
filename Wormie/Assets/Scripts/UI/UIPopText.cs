using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIPopText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtMessage;
    [SerializeField]
    private Transform container;
    [SerializeField]
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Show(string message, Vector2 position, Color color)
    {
        container.position = Camera.main.WorldToScreenPoint(position);
        //        Debug.Log($"Pos: {position}");
        txtMessage.text = $"<color=#{color.ToHexString()}>{message}</color>";
        animator.Play("uiPopTextShow");
    }

    public void AnimationFinished()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
