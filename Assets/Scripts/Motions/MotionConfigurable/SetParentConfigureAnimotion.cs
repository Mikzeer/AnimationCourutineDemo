using System;
using UnityEngine;
namespace PositionerDemo
{
    public class SetParentConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        private bool active;

        public SetParentConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder, bool isForced = true, bool active = true) : base(firstConfigure, secondConfigure, configureOrder, isForced)
        {
            this.active = active;
        }

        public override void Configure()
        {
            // el ultimo child es el que agregue recien, el ante ultimo es el holder vacio
            firstConfigure.SetParent(secondConfigure);
            firstConfigure.SetSiblingIndex(secondConfigure.childCount - 2);
            //secondConfigure.GetChild(secondConfigure.childCount - 1).SetAsLastSibling();
            //secondConfigure.GetChild(secondConfigure.childCount - 1).gameObject.SetActive(false);
            //secondConfigure.GetChild(secondConfigure.childCount - 2).SetAsLastSibling();
        }
    }
}

