using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPointer : MonoBehaviour
{

    #region Private fields
       
    private float angle;
    private float arrowXBoundary;
    private float arrowYBoundary;
    private Vector3 yPointerOffsetVector;
    private Rect rect;

    #endregion
    

    #region Serializable fields

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private RectTransform pointerScreenTransform;

    [SerializeField]
    private Sprite arrow;

    [SerializeField]
    private Sprite pointer;

    #endregion


    #region Properties

    public bool isTargetBehind { get; set; }
    public bool isTargetVisible { get; set; }
    
    #endregion


    #region Unity lifecycle

    void Awake()
    {
        CalculateArrowBoundary();
        yPointerOffsetVector = new Vector3(0f,1f,0f);
        isTargetBehind = false;
        isTargetVisible = false;
    }




    void LateUpdate()
    {
        rect = new Rect(0, 0, Screen.width, Screen.height);

        Vector3 targetPos = targetTransform.position;
        targetPos += yPointerOffsetVector; // add small y offset for pointer
       
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetPos);          
        Vector3 tmpScreenPos = screenPos;


        if (rect.Contains((screenPos - yPointerOffsetVector)) && !IsTargetBehind()) //we are looking at object
        {
            ChangeSprite(pointer);
            isTargetVisible = true;
        }
        else
        {
            ChangeSprite(arrow);

            if (IsTargetBehind()) // our targer is behind
            {
                if (mainCamera.transform.position.y > targetPos.y) 
                    tmpScreenPos = new Vector3((Screen.width - screenPos.x),0, 0);

                if (mainCamera.transform.position.y < targetPos.y)
                    tmpScreenPos = new Vector3((Screen.width - screenPos.x), Screen.height, 0);

            }
            else // we are turned left or right, so we can't see target, but target is not behind us 
            {
                if (!rect.Contains(screenPos))
                {
                    isTargetVisible = false;
                }
            }
            
        }
       

        tmpScreenPos.x = Mathf.Clamp(tmpScreenPos.x, arrowXBoundary, Screen.width - arrowXBoundary); //restrict position of our arrow 
        tmpScreenPos.y = Mathf.Clamp(tmpScreenPos.y, arrowYBoundary, Screen.height - arrowYBoundary);
        
        pointerScreenTransform.position = tmpScreenPos; //update arrow position


        angle = CalculateAngle(screenPos, tmpScreenPos); //calculate arrow angle 

        pointerScreenTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //set angle for arrow sprite
    }


    #endregion


    #region Private methods

    bool IsTargetBehind()
    {
        Vector3 playerDirection = mainCamera.transform.TransformDirection(Vector3.forward);
        Vector3 vectorBetweenPlayerAndTarget = targetTransform.position - mainCamera.transform.position;
                

        if(Vector3.Dot(playerDirection,vectorBetweenPlayerAndTarget) < 0)
        {
            isTargetBehind = true;
            return true;
        }

        isTargetBehind = false;
        return false;

        

        
    } // check if our target is behind us


    void CalculateArrowBoundary()
    {
        arrowXBoundary = Screen.width * 0.1f;
        arrowYBoundary = Screen.height * 0.1f;
       

    } // calculate the 10% boundary position for Mathf.clamp


    float CalculateAngle(Vector3 screenPos, Vector3 tmpScreenPos)
    {
        Vector3 rotationDirection = screenPos - tmpScreenPos;
        rotationDirection.Normalize();
        float tmpAngle;
        tmpAngle = Mathf.Atan2(rotationDirection.x, rotationDirection.y);
        tmpAngle *= Mathf.Rad2Deg;

        if (isTargetBehind)       //some forced corrections of angle                 
            tmpAngle -= 180f;


        if (!isTargetVisible) //we turned left or right from target
        {
            tmpAngle *= -1f; //inverse rotation
        }


        if (tmpAngle >= 360f)
            tmpAngle -= 360f;
        return tmpAngle;
    } // calculate angle for our arrow
  

    void ChangeSprite(Sprite sprite)
    {
        Image im = pointerScreenTransform.GetComponent<Image>();

        if(im.sprite != sprite)
        im.sprite = sprite;

    } // Change sprite method
    
    #endregion
      
}
