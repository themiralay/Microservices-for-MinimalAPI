namespace WalletService
{
    public class RequestHeader
    {
        public readonly Dictionary<string, string> BSCRequestHeader = new Dictionary<string, string>
        {
            {"User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0" },
            {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8" },
            {"Accept-Language", "tr-TR,tr;q=0.8,en-US;q=0.5,en;q=0.3" },
            {"Upgrade-Insecure-Requests", "1" },
            {"Sec-Fetch-Dest", "document" },
            {"Sec-Fetch-Mode", "navigate" },
            {"Sec-Fetch-Site", "none" },
            {"Sec-Fetch-User", "?1" }
        };
    }
}
