using System.Collections.Generic;

namespace DataCollector
{
    public interface IDataCollector
    {
        /// <summary>
        /// Get a list of items from the item provider.
        /// </summary>
        /// <param name="items">
        /// Type of items to get.  Valid types are obtained from the list returned by GetItemTypes().
        /// </param>
        /// <returns>A list of items in JSON format</returns>
        string GetItems(string items);

        /// <summary>
        /// Get the item specified by itemId
        /// </summary>
        /// <param name="items">
        /// Type of items.  Valid types are obtained from the list returned by GetItemTypes().
        /// </param>
        /// <param name="itemId">Id of the item to get</param>
        /// <returns>item in JSON format</returns>
        string GetItem(string items, string itemId);

        /// <summary>
        /// Get a list of subItems of the item specified by itemId
        /// </summary>
        /// <param name="items">
        /// Type of items.  Valid types are obtained from the list returned by GetItemTypes().
        /// </param>
        /// <param name="itemId">item Id</param>
        /// <param name="subItems">
        /// Type of subItems.  Valid types are obtained from the list returned by GetSubItemTypes().
        /// </param>
        /// <returns>List of subItems in JSON format</returns>
        string GetSubItems(string items, string itemId, string subItems);

        /// <summary>
        /// Get the list of valid item types for the item provider
        /// </summary>
        /// <returns>List of valid item types</returns>
        IEnumerable<string> GetItemTypes();

        /// <summary>
        /// Get the list of valid subItem types for the item provider
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetSubItemTypes();
    }
}
