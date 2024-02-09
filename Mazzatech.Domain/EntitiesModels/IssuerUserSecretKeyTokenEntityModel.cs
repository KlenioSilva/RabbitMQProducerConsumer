namespace Mazzatech.Domain.EntitiesModels
{
    public class IssuerUserSecretKeyTokenEntityModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int IssuerSecretKeyId { get; set;}
        public string? Token { get; set; }
        public int FlagActive { get; set;}
    }
}
