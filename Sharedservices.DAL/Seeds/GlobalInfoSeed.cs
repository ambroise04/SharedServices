using SharedServices.Mutual;

namespace SharedServices.DAL.Seeds
{
    public static class GlobalInfoSeed
    {
        public static GlobalInfo GlobalInfo()
        {
            return new GlobalInfo
            {
                Id = 1,
                Email = "labakoam@gmail.com",
                AddressFR = "Place Cardinal Mercier, 2 Wavre Belgique",
                AddressEN = "Place Cardinal Mercier, 2 Wavre Belgium",
                DescriptionFR = "Description de cette plateforme",
                DescriptionEN = "Description of this platform",
                Phone = "+32 (0)494 68 00 38",
                AuthorLink = "https://www.labak.azurewebsites.net",
                DefaultPointForUsers = 10
            };
        }
    }
}