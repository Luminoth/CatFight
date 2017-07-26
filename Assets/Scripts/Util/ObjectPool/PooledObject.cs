namespace CatFight.Util.ObjectPool
{
    public sealed class PooledObject : MonoBehavior
    {
        public string Tag { get; set; }

        public void Recycle()
        {
            ObjectPoolManager.Instance.Recycle(this);
        }
    }
}
