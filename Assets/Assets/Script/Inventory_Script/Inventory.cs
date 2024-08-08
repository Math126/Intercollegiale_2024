using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> Slots;
    public GameObject ItemPrefab;
    public GameObject TextQtePrefab;
    private bool CanStack;
    private GameObject SlotToStack;
    private int Used_Slot = 0;
    public Inventory_Item item;
    private int SelectedSlot = 0;
    private bool hasJustBeenUsed = false;
    private bool RedCardCanBeUse, OrangeCardCanBeUse, BlueCardCanBeUse;
    private GameObject Door = null;

    #region Ajout Inventaire
    private void OnTriggerStay(Collider other)
    {
        item = other.GetComponent<Inventory_Item>();

        if (item != null && Used_Slot < 7 && GetComponent<inputManager>().GetCanInteract())
        {
            if (item.CanStack)
            {
                CanStack = false;

                //Valid si existe deja
                foreach (GameObject slot in Slots)
                {
                    if (slot.transform.Find("Item") != null) {
                        if (item.imgObj == slot.transform.Find("Item").GetComponent<Image>().sprite)
                        {
                            CanStack = true;
                            SlotToStack = slot.gameObject;
                        }
                    }
                }

                if (!CanStack)
                {
                    AjoutImg();
                }
                else
                {
                    if (SlotToStack.transform.Find("Quantite"))
                    {
                        SlotToStack.transform.Find("Quantite").GetComponent<TextMeshProUGUI>().text = (int.Parse(SlotToStack.transform.Find("Quantite").GetComponent<TextMeshProUGUI>().text) + 1).ToString();
                    }
                    else
                    {
                        //Ajout Text Qte
                        GameObject TextQte = Instantiate(TextQtePrefab);
                        TextQte.name = "Quantite";
                        TextQte.GetComponent<TextMeshProUGUI>().text = "2";
                        TextQte.transform.parent = SlotToStack.transform;
                        TextQte.transform.position = SlotToStack.transform.position + new Vector3(30, 30, 0);
                    }
                }
            }
            else
            {
                AjoutImg();
            }

            Destroy(other.gameObject);
        }
    }

    private void AjoutImg()
    {
        GameObject NewItem = Instantiate(ItemPrefab);
        NewItem.name = "Item";
        NewItem.transform.parent = Slots[Used_Slot].transform;
        NewItem.transform.position = Slots[Used_Slot].transform.position;
        NewItem.GetComponent<Image>().sprite = item.imgObj;
        Used_Slot++;
    }
    #endregion

    #region UseObj
    private void UseSelectedObj(bool Stack)
    {
        bool hasBeenUsed = false;

        string img = Slots[SelectedSlot].transform.Find("Item").GetComponent<Image>().sprite.ToString();
        if (img == "Batterie_Img (UnityEngine.Sprite)")
        {
            GameObject.FindWithTag("Player").GetComponent<FlashLight>().RechargeBattery();
            hasBeenUsed = true;
        }
        else if (img == "Orange_AccessCard (UnityEngine.Sprite)")
        {
            if (OrangeCardCanBeUse)
            {
                Door.GetComponent<Door>().Open();
                hasBeenUsed = true;
            }
        }
        else if (img == "FlashLight_Img (UnityEngine.Sprite)")
        {
            GameObject.FindWithTag("Player").GetComponent<FlashLight>().SetFlashLightOn(true);
        }
        else if (img == "Skull_Img (UnityEngine.Sprite)")
        {
            GameObject.FindWithTag("Player").GetComponent<Throwing>().Throw();
            hasBeenUsed = true;
        }

        if (hasBeenUsed && Stack)
        {
            Slots[SelectedSlot].transform.Find("Quantite").GetComponent<TextMeshProUGUI>().text = (int.Parse(Slots[SelectedSlot].transform.Find("Quantite").GetComponent<TextMeshProUGUI>().text) - 1).ToString();

            if (Slots[SelectedSlot].transform.Find("Quantite").GetComponent<TextMeshProUGUI>().text == "1")
            {

                Destroy(Slots[SelectedSlot].transform.Find("Quantite").gameObject);
            }
        }
        else if (hasBeenUsed)
        {
            Destroy(Slots[SelectedSlot].transform.Find("Item").gameObject);
        }
    }

    //Vérifier les portes
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            if (other.GetComponent<Door>().imgName == "Orange_AccessCard")
            {
                OrangeCardCanBeUse = true;
                Door = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            if (other.GetComponent<Door>().imgName == "Orange_AccessCard")
            {
                OrangeCardCanBeUse = false;
                Door = null;
            }
        }
    }
    #endregion

    private void Start()
    {
        ShowChanges(0);
    }

    private void Update()
    {
        //Changer l'obj Selectionner
        ChangeSlots();

        //Utiliser l'obj
        if (GameObject.FindWithTag("Player").GetComponent<inputManager>().GetTryToUse())
        {

            if (SelectedSlot >= 0 && SelectedSlot < Slots.Count && Slots[SelectedSlot].transform.Find("Item") != null && !hasJustBeenUsed)
            {
                if (Slots[SelectedSlot].transform.Find("Quantite") != null)
                {
                    UseSelectedObj(true);
                }
                else
                {
                    UseSelectedObj(false);
                }

                hasJustBeenUsed = true;
                StartCoroutine(DelaiUsed());
            }
        }

    }

    private void ChangeSlots()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { ShowChanges(0); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { ShowChanges(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { ShowChanges(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { ShowChanges(3); }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { ShowChanges(4); }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { ShowChanges(5); }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { ShowChanges(6); }
    }

    private void ShowChanges(int index)
    {
        DeselectedSlot();
        GameObject.FindWithTag("Player").GetComponent<FlashLight>().SetFlashLightOn(false);
        SelectedSlot = index;
        Slots[SelectedSlot].GetComponent<Image>().color = new Color(0f, 0.55f, 1f);
    }

    private void DeselectedSlot()
    {
        foreach(GameObject slot in Slots)
        {
            slot.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
    }

    IEnumerator DelaiUsed()
    {
        yield return new WaitForSeconds(1);
        hasJustBeenUsed = false;
    }
}