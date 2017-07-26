using System;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public abstract class ItemData
    {
        [SerializeField]
        private int _id;

        public int Id => _id;

        [SerializeField]
        private string _name;

        public string Name => _name;
    }
}
