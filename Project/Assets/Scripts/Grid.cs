using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Text pos;
    public Image img;

    public Node pathPNode;

    Action<Grid> selectAction;
    
    public Grid Init(Node node, Action<Grid> select)
    {
        pathPNode = node;
        selectAction = select;
        
        var rect = gameObject.GetComponent<RectTransform>();
        gameObject.transform.localPosition = new Vector3(rect.sizeDelta.x * node.X, rect.sizeDelta.y * node.Y);
        
        pos.text = $"{node.X},{node.Y}";
        img.color = node.IsWalkable ? Color.white : Color.red;
        return this;
    }
    
    public void Move()
    {
        img.color = Color.yellow;
    }

    public void Select()
    {
        selectAction?.Invoke(this);
        img.color = Color.magenta;
    }

    public void Rest()
    {
        img.color = pathPNode.IsWalkable ? Color.white : Color.red;
    }
}
