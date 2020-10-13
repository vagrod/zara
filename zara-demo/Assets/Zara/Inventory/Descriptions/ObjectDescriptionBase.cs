using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public class ObjectDescriptionBase : IObjectDescriptionBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RequiredTool { get; set; }
        public List<string> AvailableToolsToPerformAction { get; set; }
        public bool CanBeUsedWithoutTool { get; set; }

        public Func<string, List<IInventoryItem>> GetItemsFromObjectFunc{ get; set; }
        public Func<string, IGameController, bool> CanToolBeUsedFunc { get; set; }
        public Action<string> InteractionAction { get; set; }

        public List<IInventoryItem> GetItemsFromObject(string toolName)
        {
            if (GetItemsFromObjectFunc == null)
                return null;

            return GetItemsFromObjectFunc.Invoke(toolName);
        }

        public void OnInteractionWithObject(string toolName)
        {
            if (InteractionAction != null)
                InteractionAction.Invoke(toolName);
        }

        public bool CanToolBeUsed(string toolName, IGameController gc)
        {
            if (CanToolBeUsedFunc != null)
                return CanToolBeUsedFunc(toolName, gc);

            return true;
        }

    }
}
