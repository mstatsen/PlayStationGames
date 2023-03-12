namespace PlayStationGames.GameEngine.Data.Types
{
    public static class LinkTypeHelper
    {
        public static string DefaultUrl(LinkType type, string name = "") =>
            type switch
            {
                LinkType.Stratege =>
                    $"https://www.stratege.ru/site_search#args:ajax=1&queryfr={name}",
                LinkType.PSNProfiles =>
                    $"https://psnprofiles.com/search/games?q={name}",
                _ =>
                    $"https://yandex.ru/search/?text={name} walkthrough",
            };
    }
}