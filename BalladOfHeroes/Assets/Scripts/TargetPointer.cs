using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPointer : MonoBehaviour
{

    #region Private fields

    private float spriteWidth ;
    private float spriteHeight ;
    private float angle;
    private float arrowXBoundary;
    private float arrowYBoundary;
    private float yPointerOffset;
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
        yPointerOffset = 0.5f;
        isTargetBehind = false;
        isTargetVisible = false;
        Image image = pointerScreenTransform.GetComponent<Image>();
        spriteHeight = image.sprite.texture.height;
        spriteWidth = image.sprite.texture.width;
    }




    void LateUpdate()
    {
        Vector3 targetPos = targetTransform.position;
        targetPos += new Vector3(0, yPointerOffset, 0);

        Debug.Log(targetPos);


        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetPos);        
        rect = new Rect(0, 0, Screen.width, Screen.height);
        Vector3 tmpScreenPos = screenPos;
        


        if (rect.Contains(screenPos) && !IsTargetBehind())
        {
            ChangeSprite(pointer);
            isTargetVisible = true;
        }
        else
        {

            ChangeSprite(arrow);

            if (IsTargetBehind())
            {
                isTargetBehind = true;

                
                tmpScreenPos = new Vector3((Screen.width - screenPos.x), 0, 0);
                
                
            }

            else
            {
                isTargetBehind = false;
                if (!rect.Contains(screenPos))
                {
                    isTargetVisible = false;

                }
            }
            

        }
       

        tmpScreenPos.x = Mathf.Clamp(tmpScreenPos.x, arrowXBoundary, Screen.width - arrowXBoundary);
        tmpScreenPos.y = Mathf.Clamp(tmpScreenPos.y, arrowYBoundary, Screen.height - arrowYBoundary);
        

        pointerScreenTransform.position = tmpScreenPos;

        CalculateAngle(screenPos, tmpScreenPos);

        pointerScreenTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

        

        
    }


    void CalculateArrowBoundary()
    {
        arrowXBoundary = Screen.width * 0.1f;
        arrowYBoundary = Screen.height * 0.1f;
        
    }


    void CalculateAngle(Vector3 screenPos, Vector3 tmpScreenPos)
    {
        Vector3 rotationDirection = screenPos - tmpScreenPos;
        rotationDirection.Normalize();
        angle = Mathf.Atan2(rotationDirection.x, rotationDirection.y);
        angle *= Mathf.Rad2Deg;

        if (isTargetBehind)
        {
            angle += 180;
            Debug.Log("+180");
        }

        if (!isTargetVisible)        
        {
            angle = 360 - angle;
            Debug.Log("360 - angle");

        }


    }
  

    void ChangeSprite(Sprite sprite)
    {
        Image im = pointerScreenTransform.GetComponent<Image>();

        if(im.sprite != sprite)
        im.sprite = sprite;

    }
    
    #endregion


  
}
