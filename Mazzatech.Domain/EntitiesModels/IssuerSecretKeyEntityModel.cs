namespace Mazzatech.Domain.EntitiesModels
{
    public class IssuerSecretKeyEntityModel
    {
        public int Id { get; set; }
        public string? Issuer { get; set; }
        public string? SecretKey { get; set; }
        public int FlagActive { get; set; }
    }
}
