using static Mazzatech.Domain.EntitiesModels.ProtocoloEntityModel;

namespace Mazzatech.CrossCutting
{
    public static class Util
    {
        public static byte[] LerImagemComoBytes(string caminho)
        {
            try
            {
                return File.ReadAllBytes(caminho);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static RegistroViaRG ObterRegistro(int registro)
        {
            RegistroViaRG registroViaRG;
            switch (registro)
            {
                case 1:
                    registroViaRG = new RegistroViaRG(via: enumVia.PrimeiraVia, motivo: enumMotivo.Padrao);
                    break;
                case 2:
                    registroViaRG = new RegistroViaRG(via: enumVia.SegundaVia, motivo: enumMotivo.Perda);
                    break;
                case 3:
                    registroViaRG = new RegistroViaRG(via: enumVia.SegundaVia, motivo: enumMotivo.Roubo);
                    break;
                case 4:
                    registroViaRG = new RegistroViaRG(via: enumVia.TerceiraVia, motivo: enumMotivo.Perda);
                    break;
                case 5:
                    registroViaRG = new RegistroViaRG(via: enumVia.TerceiraVia, motivo: enumMotivo.Roubo);
                    break;
                case 6:
                    registroViaRG = new RegistroViaRG(via: enumVia.QuartaVia, motivo: enumMotivo.Perda);
                    break;
                case 7:
                    registroViaRG = new RegistroViaRG(via: enumVia.QuartaVia, motivo: enumMotivo.Roubo);
                    break;
                case 8:
                    registroViaRG = new RegistroViaRG(via: enumVia.QuintaVia, motivo: enumMotivo.Perda);
                    break;
                case 9:
                    registroViaRG = new RegistroViaRG(via: enumVia.QuintaVia, motivo: enumMotivo.Roubo);
                    break;
                case 10:
                    registroViaRG = new RegistroViaRG(via: enumVia.SextaVia, motivo: enumMotivo.Perda);
                    break;
                default:
                    registroViaRG = new RegistroViaRG(via: enumVia.SextaVia, motivo: enumMotivo.Roubo);
                    break;
            }
            return registroViaRG;
        }

        public static string GerarCPFMoque()
        {
            Random random = new Random();

            // Os nove primeiros dígitos do CPF
            int digito1 = random.Next(10);
            int digito2 = random.Next(10);
            int digito3 = random.Next(10);
            int digito4 = random.Next(10);
            int digito5 = random.Next(10);
            int digito6 = random.Next(10);
            int digito7 = random.Next(10);
            int digito8 = random.Next(10);
            int digito9 = random.Next(10);

            // Calculando os dois últimos dígitos do CPF
            int soma1 = digito1 * 10 + digito2 * 9 + digito3 * 8 + digito4 * 7 + digito5 * 6 + digito6 * 5 + digito7 * 4 + digito8 * 3 + digito9 * 2;
            int resto1 = soma1 % 11;
            int verificador1 = (resto1 < 2) ? 0 : (11 - resto1);

            int soma2 = verificador1 * 11 + digito1 * 10 + digito2 * 9 + digito3 * 8 + digito4 * 7 + digito5 * 6 + digito6 * 5 + digito7 * 4 + digito8 * 3 + digito9 * 2;
            int resto2 = soma2 % 11;
            int verificador2 = (resto2 < 2) ? 0 : (11 - resto2);

            // Formatando o CPF
            return $"{digito1}{digito2}{digito3}.{digito4}{digito5}{digito6}.{digito7}{digito8}{digito9}-{verificador1}{verificador2}";
        }

        public static string GerarRGMoque()
        {
            Random random = new Random();

            // Geração de um número fictício de RG
            string rgFicticio = $"{random.Next(1000000):000000}";

            // Formatando o RG
            return rgFicticio;
        }
    }
}
