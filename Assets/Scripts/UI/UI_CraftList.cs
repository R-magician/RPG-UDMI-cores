//UI材料列表

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    //工艺插槽的父类
    [SerializeField] private Transform craftSlotParent;
    //工业插槽预制体
    [SerializeField] private GameObject craftSlotPrefab;

    //材料装备的预制清单
    [SerializeField] private List<ItemDataEquipment> craftEquipments;

    private void Awake()
    {
        //设置材料列表
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindown();
    }
    

    //设置材料列表
    public void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipments.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipments[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    //设置默认材料面板
    public void SetupDefaultCraftWindown()
    {
        if (craftEquipments[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipments[0]);
        }
    }
}
