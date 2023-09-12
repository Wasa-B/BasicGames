using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WUI
{
    
    public interface CompareText
    {

    }

    public class SelectListUI : MonoBehaviour
    {

        
        public UnityEvent<int> SelectEvent;



        public GameObject itemViewUI;
        public GameObject itemList;
        
        public void PopUP()
        {

        }
        

    }
}