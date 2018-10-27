using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{

    #region Private fields


    #endregion


    #region Serializable fields

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private RectTransform pointerScreenTransform;


    #endregion


    #region Unity lifecycle

    void Awake()
    {
    }




    void LateUpdate()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position);
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);

        Vector3 tmpScreenPos = screenPos;

        if (rect.Contains(screenPos) && !isTargetBehind())
        {
            pointerScreenTransform.position = screenPos;
        }
        else
        {
            if(isTargetBehind())
            {
               if (mainCamera.transform.position.y > targetTransform.position.y)
                {
                    tmpScreenPos = new Vector3(screenPos.x, 0, 0);
                    Debug.Log("Камера выше");
                }
               
               if (mainCamera.transform.position.y < targetTransform.position.y)
                {
                    tmpScreenPos = new Vector3(screenPos.x, Screen.height, 0);
                    Debug.Log("Камера ниже");

                }
                       

            }



        }



        pointerScreenTransform.position = tmpScreenPos;

    }


    #endregion


    #region Private methods

    bool isTargetBehind()
    {
        Vector3 playerDirection = mainCamera.transform.TransformDirection(Vector3.forward);
        Vector3 vectorBetweenPlayerAndTarget = targetTransform.position - mainCamera.transform.position;
                

        if(Vector3.Dot(playerDirection,vectorBetweenPlayerAndTarget) < 0)
        {
            Debug.Log("Target is behind");
            return true;

        }      
        return false;

        

        
    }


    void CheckDotProduct()
    {
        Vector3 playerDirection = mainCamera.transform.TransformDirection(Vector3.forward);
        Vector3 vectorBetweenPlayerAndTarget = targetTransform.position - mainCamera.transform.position;
        playerDirection.Normalize();
        vectorBetweenPlayerAndTarget.Normalize();
         Debug.Log(Vector3.Dot(playerDirection, vectorBetweenPlayerAndTarget    ));


    
       
    }


    #endregion


    #region Public methods

    #endregion
}
