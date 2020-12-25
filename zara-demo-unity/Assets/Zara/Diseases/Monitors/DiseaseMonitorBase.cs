using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases
{
    public abstract class DiseaseMonitorBase
    {

        protected IGameController _gc;

        protected DiseaseMonitorBase(IGameController gc)
        {
            _gc = gc;
        }

        public virtual void Check(float deltaTime)
        {
            
        }

        public virtual void OnConsumeItem(InventoryConsumableItemBase item)
        {
            
        }

    }
}
