using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;

namespace ZaraEngine.HealthEngine
{
    public class MedicalBodyAppliance
    {

        public BodyParts BodyPart { get; set; }
        public InventoryMedicalItemBase Item { get; set; }

    }
}
