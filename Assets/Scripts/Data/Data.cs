using System;

using UnityEngine;

namespace CatFight.Data
{
    public interface IData
    {
    }

    [Serializable]
    public abstract class Data : IData
    {
        [SerializeField]
        private int id;

        public int Id => id;

        [SerializeField]
        private string name = string.Empty;

        public string Name => name;
    }
}
