using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public abstract class WaterVesselItemBase : InventoryInfiniteConsumableToolItemBase
    {

        public int DosesLeft { get; private set; }

        public virtual int DosesCount
        {
            get { return -1; }
        }

        public virtual int WaterValuePerDose
        {
            get { return -1; }
        }

        public bool IsSafe { get; private set; }

        public bool IsFull
        {
            get { return DosesLeft == DosesCount; }
        }

        public bool IsEmpty
        {
            get { return DosesLeft == 0; }
        }

        public DateTime? LastFillTime { get; private set; }

        public DateTime? LastDisinfectTime { get; private set; }
        
        public DateTime? LastBoilTime { get; private set; }

        public virtual float VesselWeightInGramms
        {
            get { return 0f; }
        }

        public virtual float WaterDoseWeightInGramms
        {
            get { return 0f; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return WaterDoseWeightInGramms * DosesLeft + VesselWeightInGramms; }
        }

        public string NamePostfix
        {
            get
            {
                var safePostfix = IsSafe ? "$Safe" : "$Unsafe";

                if (DosesLeft == 0)
                    return string.Empty;

                return safePostfix + DosesLeft;
            }
        }

        public void FillUp(DateTime gameTime)
        {
            LastFillTime = gameTime;
            LastDisinfectTime = null;
            LastBoilTime = null;
            IsSafe = false;
            DosesLeft = DosesCount;
        }

        public void TakeAwayOneDose()
        {
            if (DosesLeft <= 0)
                return;

            DosesLeft -= 1;
        }

        public void Disinfect(DateTime gameTime)
        {
            IsSafe = true;
            LastDisinfectTime = gameTime;
        }

        public void Boil(DateTime gameTime)
        {
            IsSafe = true;
            LastDisinfectTime = gameTime;
            LastBoilTime = gameTime;
        }

    }
}
