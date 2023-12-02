namespace PlantacaoMorangos
{
    public class Leitura
    {
        public DateTime DataHora { get; set; }
        public int PontoMedicao { get; set; }
        public double Temperatura { get; set; }
        public double Umidade { get; set; }
        public int Indice { get; set; }
        public string Condicao { get; set; }
    }

}