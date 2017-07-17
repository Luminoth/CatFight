using System;

namespace CatFight.Data
{
    public interface IData
    {
        void Process();
    }

    [Serializable]
    public abstract class Data : IData
    {
        public int id;

        public string name = string.Empty;

        public abstract void Process();
    }
}
