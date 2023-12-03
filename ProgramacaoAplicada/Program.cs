using System.Globalization;

namespace PlantacaoMorangos
{

    class Program
    {
        static void Main(string[] args)
        {
            PlantacaoManager plantacaoManager = new();

            GerarLeituras(plantacaoManager);
            IniciaMenu(plantacaoManager);

            Console.WriteLine();
        }

        private static void GerarLeituras(PlantacaoManager plantacaoManager)
        {
            Random random = new();

            for (int pontoMedicao = 1; pontoMedicao <= 10; pontoMedicao++)
            {
                double temperatura = random.NextDouble() * (42 - 0) + 0;
                double umidade = random.NextDouble() * (100 - 0) + 0;
                int indice = random.Next(1, 10);

                string condicao = plantacaoManager.VerificarCondicoes(temperatura, umidade);

                plantacaoManager.SalvarLeitura(pontoMedicao, temperatura, umidade, indice, condicao);
            }
        }

        private static void IniciaMenu(PlantacaoManager plantacaoManager)
        {

            bool sair = false;
            while (!sair)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("MENU:");
                Console.ResetColor();
                Console.WriteLine("──────────────────────────────────────────────────────");
                Console.WriteLine(" 1 - Buscar todas as leituras");
                Console.WriteLine(" 2 - Buscar leituras críticas");
                Console.WriteLine(" 3 - Buscar leituras a partir de uma data específica");
                Console.WriteLine(" 4 - Buscar ordem cresceste");
                Console.WriteLine(" 5 - Buscar datas com emergencias");
                Console.WriteLine(" 6 - Gerar Leituras");
                Console.WriteLine(" 0 - Sair");
                Console.WriteLine("──────────────────────────────────────────────────────");
                Console.WriteLine();

                Console.Write("Escolha uma opção: ");
                string escolha = Console.ReadLine();

                switch (escolha)
                {
                    case "1":
                        BuscarTodasLeituras(plantacaoManager);
                        break;
                    case "2":
                        BuscarLeiturasCriticas(plantacaoManager);
                        break;
                    case "3":
                        BuscarLeiturasData(plantacaoManager);
                        break;
                    case "4":
                        BuscarOrdemCrescente(plantacaoManager);
                        break;
                    case "5":
                        BuscarDatasEmergencia(plantacaoManager);
                        break;
                    case "6":
                        GerarLeituras(plantacaoManager);
                        break;
                    case "0":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                Console.WriteLine();
            }
        }
        private static void BuscarTodasLeituras(PlantacaoManager plantacaoManager)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Buscar todas as leituras");
            Console.ResetColor();

            List<Leitura> todasLeituras = plantacaoManager.BuscarLeituras();
            int i = 1;
            foreach (Leitura leitura in todasLeituras)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Leitura {i}");
                Console.ResetColor();
                Console.WriteLine($"DataHora: {leitura.DataHora:dd/MM/yyy HH:mm:ss}, Ponto de Medição: {leitura.PontoMedicao}, Temperatura: {leitura.Temperatura}, Umidade: {leitura.Umidade}, Índice: {leitura.Indice}, Condicao: {leitura.Condicao}");
                i++;
            }
        }

        private static void BuscarLeiturasCriticas(PlantacaoManager plantacaoManager)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Buscar leituras críticas");
            Console.ResetColor();

            List<Leitura> leiturasCriticas = plantacaoManager.BuscarLeituras(apenasCriticas: true);
            int i = 1;
            foreach (Leitura leitura in leiturasCriticas)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Leitura {i}");
                Console.ResetColor();
                Console.WriteLine($"DataHora: {leitura.DataHora:dd/MM/yyy HH:mm:ss}, Ponto de Medição: {leitura.PontoMedicao}, Temperatura: {leitura.Temperatura}, Umidade: {leitura.Umidade}, Índice: {leitura.Indice}, Condicao: {leitura.Condicao}");
                i++;
            }
        }

        private static void BuscarLeiturasData(PlantacaoManager plantacaoManager)
        {
            DateTime data = PegarData();
            if (data != DateTime.MinValue)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Buscar leituras a partir da data: {data:dd/MM/yyyy}");
                Console.ResetColor();

                List<Leitura> leiturasDataEspecifica = plantacaoManager.BuscarLeituras(data);
                int i = 1;
                foreach (Leitura leitura in leiturasDataEspecifica)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Leitura {i}");
                    Console.ResetColor();
                    Console.WriteLine($"DataHora: {leitura.DataHora:dd/MM/yyy HH:mm:ss}, Ponto de Medição: {leitura.PontoMedicao}, Temperatura: {leitura.Temperatura}, Umidade: {leitura.Umidade}, Índice: {leitura.Indice}, Condicao: {leitura.Condicao}");
                    i++;
                }
            }
        }
        private static DateTime PegarData()
        {
            Console.WriteLine("Digite o data para buscar leitura (no formato dd/MM/yyyy): ");
            string dataString = Console.ReadLine();

            try
            {
                var culture = new CultureInfo("pt-BR");                              
                return DateTime.ParseExact(dataString, "dd/MM/yyyy", culture);
            }
            catch (Exception)
            {
                ConsoleWriteRed("Opção inválida.");
            }

            return DateTime.MinValue;
        }

        private static void BuscarOrdemCrescente(PlantacaoManager plantacaoManager)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Buscar ordem cresceste");
            Console.ResetColor();

            List<Leitura> leituras = plantacaoManager.BuscarLeituras();
            int i = 1;
            foreach (Leitura leitura in leituras.OrderBy(l => l.DataHora).ThenBy(l => l.Temperatura))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Leitura {i}");
                Console.ResetColor();
                Console.WriteLine($"DataHora: {leitura.DataHora:dd/MM/yyy HH:mm:ss}, Ponto de Medição: {leitura.PontoMedicao}, Temperatura: {leitura.Temperatura}, Umidade: {leitura.Umidade}, Índice: {leitura.Indice}, Condicao: {leitura.Condicao}");
                i++;
            }
        }

        private static void BuscarDatasEmergencia(PlantacaoManager plantacaoManager)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Buscar datas com emergencias");
            Console.ResetColor();

            List<Leitura> leituras = plantacaoManager.BuscarLeituras(apenasCriticas: true);
            int i = 1;
            foreach (Leitura leitura in leituras)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Leitura {i}");
                Console.ResetColor();
                Console.WriteLine($"DataHora: {leitura.DataHora:dd/MM/yyy HH:mm:ss}, Ponto de Medição: {leitura.PontoMedicao}, Temperatura: {leitura.Temperatura}, Umidade: {leitura.Umidade}, Índice: {leitura.Indice}, Condicao: {leitura.Condicao}");
                i++;
            }
        }
        private static void ConsoleWriteRed(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(texto);
            Console.ResetColor();
        }
    }

}