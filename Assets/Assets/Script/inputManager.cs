using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    private bool CanInteract, ParamIsClosed, CanOpenChest, tryToUseObj;
    private GameObject chest;

    #region Input
    private void OnInteract()
    {
        if (!CanOpenChest)
        {
            CanInteract = true;
            StartCoroutine(DelaiTake());
        }
        else
        {
            chest.GetComponent<Chest>().IsInteracted();
        }
    }

    private void OnParam()
    {
        if (ParamIsClosed)
            ParamIsClosed = false;
        else
            ParamIsClosed = true;
    }

    private void OnUseItem()
    {
        tryToUseObj = true;
        StartCoroutine(DelaiUse());
    }
    #endregion

    #region Get/Set
    public bool GetCanInteract()
    {
        return CanInteract;
    }

    public bool GetParamIsClosed()
    {
        return ParamIsClosed;
    }

    public bool GetTryToUse()
    {
        return tryToUseObj;
    }

    public void SetCanOpenChest()
    {
        CanOpenChest = false;
    }

    public void SetParamIsClosed()
    {
        ParamIsClosed = false;
    }
    #endregion

    #region Trigger
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            CanOpenChest = true;
            chest = other.gameObject;
        }
    }
    #endregion

    IEnumerator DelaiTake()
    {
        yield return new WaitForSeconds(0.2f);
        CanInteract = false;
    }

    IEnumerator DelaiUse()
    {
        yield return new WaitForSeconds(0.2f);
        tryToUseObj = false;
    }
}
