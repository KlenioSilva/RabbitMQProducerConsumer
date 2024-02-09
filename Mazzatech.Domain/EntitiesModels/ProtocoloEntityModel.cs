using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazzatech.Domain.EntitiesModels
{
    public class ProtocoloEntityModel
    {
        public int Id { get; set; }
        [Required]
        public Guid Protocolo { get; set; }
        [Required]
        public string Nome { get; set; }
        [NotMapped]
        public RegistroViaRG ViaRG { get; set; }
        public struct RegistroViaRG
        {
            public enumVia _Via;
            public enumMotivo _Motivo;
            public RegistroViaRG(enumVia via, enumMotivo motivo)
            {
                _Via = via;
                _Motivo = motivo;
            }
        }

        public enumVia Via { get; set; }
        public enumMotivo Motivo { get; set; }

        [Required]
        [MaxLength(11, ErrorMessage = "Favor Informar apenas 11 caracteres")]
        public string CPF { get; set; }
        [Required]
        public string RG { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public byte[] Foto { get; set; }
        public int flagEnviado { get; set; }

        public enum enumVia
        {
            PrimeiraVia = 1,
            SegundaVia = 2,
            TerceiraVia = 3,
            QuartaVia = 4,
            QuintaVia = 5,
            SextaVia = 6
        }

        public enum enumMotivo
        {
            Padrao = 1,
            Perda = 2,
            Roubo = 3
        }
    }
}
