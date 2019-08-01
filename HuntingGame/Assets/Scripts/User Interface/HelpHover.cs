using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HelpHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject helpPanel;
    private RectTransform panelRect;
    [SerializeField] private Vector2 anchoredPosOrigin;
    public bool isHovering;
    public float speed;
    private void Awake()
    {
        if (helpPanel)
        {
            panelRect = helpPanel.GetComponent<RectTransform>();
            anchoredPosOrigin = panelRect.anchoredPosition;
        }
    }


    private void Update()
    {
        if (isHovering)
        {
            float xPos = panelRect.anchoredPosition.x;
            float yPos = panelRect.anchoredPosition.y;
            Debug.Log(xPos);
            if (xPos > -(anchoredPosOrigin.x / 2))
            {
                panelRect.anchoredPosition = new Vector2(xPos -= Time.deltaTime * speed, yPos);
            }
        }
        else
        {
            float xPos = panelRect.anchoredPosition.x;
            float yPos = panelRect.anchoredPosition.y;
            Debug.Log(xPos);
            if (xPos < anchoredPosOrigin.x)
            {
                panelRect.anchoredPosition = new Vector2(xPos += Time.deltaTime * speed, yPos);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }


}
