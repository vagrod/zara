using System.Collections.Generic;
using System.Linq;
using ZaraEngine.HealthEngine;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class ActiveMedicalAgentsMonitorsNode: IAcceptsStateChange
    {
        private const float EpinephrineActiveTime           = 64f;  // game minutes
        private const float AntiVenomActiveTime             = 123f; // game minutes
        private const float AtropineActiveTime              = 98f;  // game minutes
        private const float MorphineActiveTime              = 106f; // game minutes
        private const float AntibioticActiveTime            = 305f; // game minutes
        private const float AspirinActiveTime               = 71f;  // game minutes
        private const float AcetaminophenActiveTime         = 126f; // game minutes
        private const float LoperamideActiveTime            = 92f;  // game minutes
        private const float OseltamivirActiveTime           = 135f; // game minutes
        private const float SedativeActiveTime              = 147f; // game minutes
        private const float DoripenemActiveTime             = 191f; // game minutes

        public bool IsEpinephrineActive { get; private set; }
        public bool IsAntiVenomActive { get; private set; }
        public bool IsAtropineActive { get; private set; }
        public bool IsMorphineActive { get; private set; }
        public bool IsAntibioticActive { get; private set; }
        public bool IsAspirinActive { get; private set; }
        public bool IsAcetaminophenActive { get; private set; }
        public bool IsLoperamideActive { get; private set; }
        public bool IsOseltamivirActive { get; private set; }
        public bool IsSedativeActive { get; private set; }
        public bool IsDoripenemActive { get; private set; }

        private readonly IActiveMedicalAgent _epinephrineMedicalAgent;
        private readonly IActiveMedicalAgent _antiVenomMedicalAgent;
        private readonly IActiveMedicalAgent _atropineMedicalAgent;
        private readonly IActiveMedicalAgent _morphineMedicalAgent;
        private readonly IActiveMedicalAgent _antibioticMedicalAgent;
        private readonly IActiveMedicalAgent _aspirinMedicalAgent;
        private readonly IActiveMedicalAgent _acetaminophenMedicalAgent;
        private readonly IActiveMedicalAgent _loperamideMedicalAgent;
        private readonly IActiveMedicalAgent _oseltamivirMedicalAgent;
        private readonly IActiveMedicalAgent _sedativeMedicalAgent;
        private readonly IActiveMedicalAgent _doripenemMedicalAgent;

        private readonly List<IActiveMedicalAgent> _monitors = new List<IActiveMedicalAgent>();
        private readonly IGameController _gc;

        public bool IsAnyAgentActive { get; private set; }

        public IActiveMedicalAgent EpinephrineAgent
        {
            get { return _epinephrineMedicalAgent; }
        }
        public IActiveMedicalAgent AntiVenomAgent
        {
            get { return _antiVenomMedicalAgent; }
        }
        public IActiveMedicalAgent AtropineAgent
        {
            get { return _atropineMedicalAgent; }
        }
        public IActiveMedicalAgent MorphineAgent
        {
            get { return _morphineMedicalAgent; }
        }
        public IActiveMedicalAgent AntibioticAgent
        {
            get { return _antibioticMedicalAgent; }
        }
        public IActiveMedicalAgent AspirinAgent
        {
            get { return _aspirinMedicalAgent; }
        }
        public IActiveMedicalAgent AcetaminophenAgent
        {
            get { return _acetaminophenMedicalAgent; }
        }
        public IActiveMedicalAgent LoperamideAgent
        {
            get { return _loperamideMedicalAgent; }
        }
        public IActiveMedicalAgent OseltamivirAgent
        {
            get { return _oseltamivirMedicalAgent; }
        }
        public IActiveMedicalAgent SedativeAgent
        {
            get { return _sedativeMedicalAgent; }
        }
        public IActiveMedicalAgent DoripenemAgent
        {
            get { return _doripenemMedicalAgent; }
        }

        public List<IActiveMedicalAgent> ActiveAgents
        {
            get { return _monitors.Where(x => x.IsActive).ToList(); }
        }

        public ActiveMedicalAgentsMonitorsNode(IGameController gc)
        {
            _gc = gc;

            _epinephrineMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.EpinephrineGroup, ActiveMedicalAgent.CurveTypes.ActiveImmediately, EpinephrineActiveTime);
            _antiVenomMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.AntiVenomGroup, ActiveMedicalAgent.CurveTypes.SlowActivation, AntiVenomActiveTime);
            _atropineMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.AtropineGroup, ActiveMedicalAgent.CurveTypes.SlowActivation, AtropineActiveTime);
            _morphineMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.MorphineGroup, ActiveMedicalAgent.CurveTypes.ActiveImmediately, MorphineActiveTime);
            _doripenemMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.DoripenemGroup, ActiveMedicalAgent.CurveTypes.ActiveImmediately, DoripenemActiveTime);

            _antibioticMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.AntibioticGroup, ActiveMedicalAgent.CurveTypes.ActiveInSecondHalf, AntibioticActiveTime);
            _aspirinMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.AspirinGroup, ActiveMedicalAgent.CurveTypes.SlowActivation, AspirinActiveTime);
            _acetaminophenMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.AcetaminophenGroup, ActiveMedicalAgent.CurveTypes.SlowActivation, AcetaminophenActiveTime);
            _loperamideMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.LoperamideGroup, ActiveMedicalAgent.CurveTypes.ActiveImmediately, LoperamideActiveTime);
            _oseltamivirMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.OseltamivirGroup, ActiveMedicalAgent.CurveTypes.SlowActivation, OseltamivirActiveTime);
            _sedativeMedicalAgent = new ActiveMedicalAgent(_gc, MedicalConsumablesGroup.SedativeGroup, ActiveMedicalAgent.CurveTypes.ActiveImmediately, SedativeActiveTime);

            _monitors.AddRange(new []
            {
                _epinephrineMedicalAgent,
                _antiVenomMedicalAgent,
                _atropineMedicalAgent,
                _morphineMedicalAgent,
                _antibioticMedicalAgent,
                _aspirinMedicalAgent,
                _acetaminophenMedicalAgent,
                _loperamideMedicalAgent,
                _oseltamivirMedicalAgent,
                _sedativeMedicalAgent,
                _doripenemMedicalAgent
            });
        }

        public void Check()
        {
            _monitors.ForEach(x => x.Check());

            IsAnyAgentActive = _monitors.Any(x => x.IsActive);

            IsEpinephrineActive = _epinephrineMedicalAgent.IsActive;
            IsAntiVenomActive = _antiVenomMedicalAgent.IsActive;
            IsAtropineActive = _atropineMedicalAgent.IsActive;
            IsMorphineActive =_morphineMedicalAgent.IsActive;
            IsAntibioticActive =_antibioticMedicalAgent.IsActive;
            IsAspirinActive =_aspirinMedicalAgent.IsActive;
            IsAcetaminophenActive =_acetaminophenMedicalAgent.IsActive;
            IsLoperamideActive =_loperamideMedicalAgent.IsActive;
            IsOseltamivirActive = _oseltamivirMedicalAgent.IsActive;
            IsSedativeActive =_sedativeMedicalAgent.IsActive;
            IsDoripenemActive = _doripenemMedicalAgent.IsActive;
        }

        public void OnConsumeItem(InventoryConsumableItemBase item)
        {
            _monitors.ForEach(x => x.OnConsumeItem(item));
        }

        public void OnApplianceTaken(InventoryMedicalItemBase item, BodyParts bodyPart)
        {
            _monitors.ForEach(x => x.OnApplianceTaken(item, bodyPart));
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new ActiveMedicalAgentsMonitorsSnippet();

            state.IsEpinephrineActive = this.IsEpinephrineActive;
            state.IsAntiVenomActive = this.IsAntiVenomActive;
            state.IsAtropineActive = this.IsAtropineActive;
            state.IsMorphineActive = this.IsMorphineActive;
            state.IsAntibioticActive = this.IsAntibioticActive;
            state.IsAspirinActive = this.IsAspirinActive;
            state.IsAcetaminophenActive = this.IsAcetaminophenActive;
            state.IsLoperamideActive = this.IsLoperamideActive;
            state.IsOseltamivirActive =this.IsOseltamivirActive;
            state.IsSedativeActive = this.IsSedativeActive;
            state.IsDoripenemActive = this.IsDoripenemActive;

            state.ChildStates.Add("EpinephrineMedicalAgent", (_monitors[0] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("AntiVenomMedicalAgent", (_monitors[1] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("AtropineMedicalAgent", (_monitors[2] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("MorphineMedicalAgent", (_monitors[3] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("AntibioticMedicalAgent", (_monitors[4] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("AspirinMedicalAgent", (_monitors[5] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("AcetaminophenMedicalAgent", (_monitors[6] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("LoperamideMedicalAgent", (_monitors[7] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("OseltamivirMedicalAgent", (_monitors[8] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("SedativeMedicalAgent", (_monitors[9] as ActiveMedicalAgent).GetState());
            state.ChildStates.Add("DoripenemMedicalAgent", (_monitors[10] as ActiveMedicalAgent).GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (ActiveMedicalAgentsMonitorsSnippet)savedState;

            IsEpinephrineActive = state.IsEpinephrineActive;
            IsAntiVenomActive = state.IsAntiVenomActive;
            IsAtropineActive = state.IsAtropineActive;
            IsMorphineActive = state.IsMorphineActive;
            IsAntibioticActive = state.IsAntibioticActive;
            IsAspirinActive = state.IsAspirinActive;
            IsAcetaminophenActive = state.IsAcetaminophenActive;
            IsLoperamideActive = state.IsLoperamideActive;
            IsOseltamivirActive = state.IsOseltamivirActive;
            IsSedativeActive = state.IsSedativeActive;
            IsDoripenemActive = state.IsDoripenemActive;

            (_monitors[0] as ActiveMedicalAgent).RestoreState(state.ChildStates["EpinephrineMedicalAgent"]);
            (_monitors[1] as ActiveMedicalAgent).RestoreState(state.ChildStates["AntiVenomMedicalAgent"]);
            (_monitors[2] as ActiveMedicalAgent).RestoreState(state.ChildStates["AtropineMedicalAgent"]);
            (_monitors[3] as ActiveMedicalAgent).RestoreState(state.ChildStates["MorphineMedicalAgent"]);
            (_monitors[4] as ActiveMedicalAgent).RestoreState(state.ChildStates["AntibioticMedicalAgent"]);
            (_monitors[5] as ActiveMedicalAgent).RestoreState(state.ChildStates["AspirinMedicalAgent"]);
            (_monitors[6] as ActiveMedicalAgent).RestoreState(state.ChildStates["AcetaminophenMedicalAgent"]);
            (_monitors[7] as ActiveMedicalAgent).RestoreState(state.ChildStates["LoperamideMedicalAgent"]);
            (_monitors[8] as ActiveMedicalAgent).RestoreState(state.ChildStates["OseltamivirMedicalAgent"]);
            (_monitors[9] as ActiveMedicalAgent).RestoreState(state.ChildStates["SedativeMedicalAgent"]);
            (_monitors[10] as ActiveMedicalAgent).RestoreState(state.ChildStates["DoripenemMedicalAgent"]);
        }

        #endregion 

    }
}
