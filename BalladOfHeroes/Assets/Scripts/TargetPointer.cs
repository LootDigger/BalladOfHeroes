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
        if (rect.Contains(screenPos))
        {
            pointerScreenTransform.position = screenPos;
        }
        else
        {
            if(isTargetBehind())
            {
                
                pointerScreenTransform.position = new Vector3( mainCamera.transform.position.x, 0, 0);

            }



        }




        // isTargetBehind();
        CheckDotProduct();

    }


    #endregion


    #region Private methods

    bool isTargetBehind()
    {
        Vector3 playerDirection = mainCamera.transform.TransformDirection(Vector3.forward);
        Vector3 vectorBetweenPlayerAndTarget = targetTransform.position - mainCamera.transform.position;

        

        if(Vector3.Dot(vectorBetweenPlayerAndTarget,playerDirection) < 0)
        {
            Debug.Log("Target is behind");
            return true;

        }
        else
        {
            return false;

        }

        
    }


    void CheckDotProduct()
    {
        Vector3 playerDirection = mainCamera.transform.TransformDirection(Vector3.forward);
        Vector3 vectorBetweenPlayerAndTarget = targetTransform.position - mainCamera.transform.position;
        playerDirection.Normalize();
        vectorBetweenPlayerAndTarget.Normalize();
       // Debug.Log(Vector3.Dot(vectorBetweenPlayerAndTarget, playerDirection));

        Debug.Log("Angle between vectors" + Vector3.Angle(playerDirection, vectorBetweenPlayerAndTarget));
       
    }


    #endregion


    #region Public methods

    #endregion
}
