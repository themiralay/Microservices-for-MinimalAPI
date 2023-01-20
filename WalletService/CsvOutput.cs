using CsvHelper.Configuration.Attributes;

namespace WalletService
{
    public class CsvOutput
    {
        [Index(0)]
        public int OutA { get; set; }
        [Index(1)]
        public int OutB { get; set; }
        [Index(2)]
        public int OutC { get; set; }
    }
}
