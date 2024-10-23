using OxDAOEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public partial class Accessories : ListDAO<Accessory>
    {
        public string ShortColumnText()
        {
            string result = string.Empty;

            Dictionary<ShortAccessoryInfo, int> dictionary = new();

            foreach (Accessory accessory in this)
            {
                ShortAccessoryInfo shortInfo = accessory.ShortInfo;

                if (dictionary.ContainsKey(shortInfo))
                    dictionary[shortInfo] += 1;
                else dictionary.Add(shortInfo, accessory.Count);
            }

            foreach (KeyValuePair<ShortAccessoryInfo, int> item in dictionary)
                result +=
                    (item.Value < 2
                        ? string.Empty
                        : $"{item.Value} x ")
                    + item.Key.ToString() + "\n";

            return result;
        }
    }
}
