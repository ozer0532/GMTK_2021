using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Zro.UI
{
    public class TabGroupSwitcher : MonoBehaviour, IPointerClickHandler
    {
        public TabGroupController tabGroup;
        public GameObject tabPage;

        public void OnPointerClick(PointerEventData eventData)
        {
            SwitchToPage();
        }

        public void SwitchToPage()
        {
            tabGroup.ChangeActivePage(tabPage);
        }
    }
}
