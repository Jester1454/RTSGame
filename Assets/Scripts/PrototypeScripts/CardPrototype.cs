using System;
using Cards;
using UnityEngine;

namespace PrototypeScripts
{
    [Serializable]
    public class CardPrototype
    {
        public int Id;

        public void LoadFromObject(GameObject baseGameObject)
        {
            Card card = baseGameObject.GetComponent<Card>();
            Id = card.Id;
        }
    }
}