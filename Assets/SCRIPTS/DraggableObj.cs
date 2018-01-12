using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableObj : MonoBehaviour {

    public const string dragTag = "UIdrag";

    private bool dragging = false;
    private Vector2 origPos;
    private Transform objToDrag;
    private Image objToDragImage;

    List<RaycastResult> hitObjects = new List<RaycastResult>();



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            objToDrag = GetDraggableTransformUnderMouse();
            if(objToDrag != null)
            {
                dragging = true;
                objToDrag.SetAsLastSibling();

                origPos = objToDrag.position;
                objToDragImage = objToDrag.GetComponent<Image>();
                objToDragImage.raycastTarget = false;
            }
        }
        if (dragging)
        {
            objToDrag.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(objToDrag != null)
            {
                Transform objToReplace = GetDraggableTransformUnderMouse();
                if(objToReplace != null)
                {
                    objToDrag.position = objToReplace.position;
                    objToReplace.position = origPos;
                }
                else
                {
                    objToDrag.position = origPos;
                }
                objToDragImage.raycastTarget = true;
                objToDrag = null;
            }
            dragging = false;
        }
	}

    private GameObject GetObjUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointer, hitObjects);
        if (hitObjects.Count <= 0) return null;
        return hitObjects[0].gameObject;
    }

    private Transform GetDraggableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjUnderMouse();
        if(clickedObject !=null && clickedObject.tag == dragTag)
        {
            return clickedObject.transform;
        }
        return null;
    }
}
