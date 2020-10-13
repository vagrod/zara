using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public virtual void Check()
        {
            
        }

        public virtual void OnConsumeItem(InventoryConsumableItemBase item)
        {
            
        }

    }
}
