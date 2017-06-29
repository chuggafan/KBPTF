using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steam.Models.GameEconomy;
using SteamWebAPI2.Interfaces;
using SchemaItemModel = Steam.Models.TF2.SchemaItemModel;

namespace backpack.tf
{
    internal class SteamInventoryData
    {
        private readonly EconItems _econItems;
        private IList<SchemaItemModel> _items;

        public SteamInventoryData(string apiKey)
        {
            _econItems = new EconItems(apiKey, EconItemsAppId.TeamFortress2);
            RetrieveBaseItems();
        }

        private async void RetrieveBaseItems()
        {
            var items = await _econItems.GetSchemaForTF2Async();
            _items = items.Data.Items;
        }

        public async Task<IReadOnlyCollection<EconItemModel>> GetItems(ulong steamId)
        {
            var result = await _econItems.GetPlayerItemsAsync(steamId);
            return result.Data.Items;
        }

        public string GetItemName(uint itemDefIndex)
        {
            return _items.FirstOrDefault(x => x.DefIndex == itemDefIndex)?.Name;
        }
    }
}
