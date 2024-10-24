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
    //材料插槽
    [SerializeField] private List<UI_CraftSlot> craftSlots;

    private void Awake()
    {
        //分配材料插槽
        AssignCraftSlot();
    }

    //分配材料插槽
    private void AssignCraftSlot()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UI_CraftSlot>());   
        }
    }

    //设置材料列表
    public void SetupCraftList()
    {
        for (int i = 0; i < craftSlots.Count; i++)
        {
            Destroy(craftSlots[i].gameObject);
        }

        craftSlots = new List<UI_CraftSlot>();

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
}
