using System;
using ZaraEngine.HealthEngine;

namespace ZaraEngine {

    public interface IGameController {

        DateTime? WorldTime { get; }

        Player.IPlayerStatus Player { get; }

        Player.BodyStatusController Body { get; }

        IWeatherDescription Weather { get; }

        HealthController Health { get; }

        Inventory.InventoryController Inventory { get; }

        TimesOfDay TimeOfDay { get; }

    }

}