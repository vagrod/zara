using System.Collections.Generic;

namespace ZaraEngine.Inventory
{
    public interface IObjectDescriptionBase
    {

        string Name { get; set; }

        string Description { get; set; }

        bool CanBeUsedWithoutTool { get; set; }

        string RequiredTool { get; set; }

        List<string> AvailableToolsToPerformAction { get; set; }

        List<IInventoryItem> GetItemsFromObject(string toolName);

        void OnInteractionWithObject(string toolName);

        bool CanToolBeUsed(string toolName, IGameController gc);

    }
}
