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
    private float inversRotationValue;

    private Vector3 pointerLeftRotation;
    private Vector3 pointerRightRotation;
    private Vector3 pointerUpRotation;


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
          pointerLeftRotation = new Vector3(0,0,-90f);
          pointerRightRotation = new Vector3(0, 0, 90f);
          pointerUpRotation = new Vector3(0, 0, 180f); ;

        isTargetBehind = false;
        isTargetVisible = false;
        Image image = pointerScreenTransform.GetComponent<Image>();
        spriteHeight = image.sprite.texture.height;
        spriteWidth = image.sprite.texture.width;
        inversRotationValue = 0f;
    }




    void LateUpdate()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position);        
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        Vector3 tmpScreenPos = screenPos;



        if (rect.Contains(screenPos) && !IsTargetBehind())
        {
            ChangeSprite(pointer);
            pointerScreenTransform.position = tmpScreenPos;
            inversRotationValue = 1f;
            isTargetVisible = true;

        }
        else
        {
            ChangeSprite(arrow);

            if (IsTargetBehind())
            {
               
                tmpScreenPos = new Vector3((Screen.width - screenPos.x), 0, 0);
            }

            else 
                if(!rect.Contains(screenPos))
            {
                isTargetVisible = false;

            }
            

        }
        float sizeX = spriteWidth / 2;
        float sizeY = spriteHeight / 2;

        tmpScreenPos.x = Mathf.Clamp(tmpScreenPos.x, sizeX, Screen.width - sizeX);
        tmpScreenPos.y = Mathf.Clamp(tmpScreenPos.y, sizeY  , Screen.height - sizeY);

        pointerScreenTransform.position = tmpScreenPos;

        CalculateAngle(screenPos, tmpScreenPos);

        pointerScreenTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        IsTargetForward();
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



    bool IsTargetForward()
    {
        Vector3 playerDirection = mainCamera.transform.TransformDirection(Vector3.forward);
        Vector3 vectorBetweenPlayerAndTarget = targetTransform.position - mainCamera.transform.position;
        playerDirection.Normalize();
        vectorBetweenPlayerAndTarget.Normalize();

        //if (Vector3.Dot(playerDirection, vectorBetweenPlayerAndTarget) < 0)
        //{
        //    isTargetBehind = true;
        //    return true;
        //}
        //isTargetBehind = false;
        return false;
        
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
        }

        if (!isTargetVisible)        
        {
            angle = 360 - angle;
        }

            
    }
  
    void ChangeSprite(Sprite sprite )
    {
        Image im = pointerScreenTransform.GetComponent<Image>();

        if(im.sprite != sprite)
        im.sprite = sprite;

    }


    #endregion


    #region Public methods

    #endregion
}
