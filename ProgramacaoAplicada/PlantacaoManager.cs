using System.Data.SQLite;

namespace PlantacaoMorangos
{
    public class PlantacaoManager
    {
        private string connectionString = "Data Source=dados_plantacao.db;Version=3;";
        public void SalvarLeitura(int pontoMedicao, double temperatura, double umidade, int indice, string condicao)
        {
            SQLiteConnection.CreateFile("mydatabase.db");
            using SQLiteConnection connection = new(connectionString);
            connection.Open();
            using SQLiteCommand command = connection.CreateCommand();

            command.CommandText = @"CREATE TABLE IF NOT EXISTS Leituras (DataHora TEXT, PontoMedicao INTEGER, Temperatura REAL, Umidade REAL, Indice TEXT, Condicao TEXT)";
            command.ExecuteNonQuery();

            command.CommandText = @"INSERT INTO Leituras (DataHora, PontoMedicao, Temperatura, Umidade, Indice, Condicao) VALUES (@dataHora, @pontoMedicao, @temperatura, @umidade, @indice, @condicao)";
            command.Parameters.AddWithValue("@dataHora", DateTime.Now.ToString());
            command.Parameters.AddWithValue("@pontoMedicao", pontoMedicao);
            command.Parameters.AddWithValue("@temperatura", temperatura);
            command.Parameters.AddWithValue("@umidade", umidade);
            command.Parameters.AddWithValue("@indice", indice);
            command.Parameters.AddWithValue("@condicao", condicao);
            command.ExecuteNonQuery();
        }

        public List<Leitura> BuscarLeituras(DateTime? data = null, bool apenasCriticas = false)
        {
            List<Leitura> leituras = new();

            using SQLiteConnection connection = new(connectionString);            
            connection.Open();
            using SQLiteCommand command = connection.CreateCommand();
            if (apenasCriticas)
            {
                command.CommandText = @"SELECT * FROM Leituras WHERE (Temperatura > 30 AND Umidade <= 30)";
            }
            else if (data.HasValue)
            {
                command.CommandText = @"SELECT * FROM Leituras WHERE DataHora >= @data";
                command.Parameters.AddWithValue("@data", data.Value.ToString());
            }
            else
            {
                command.CommandText = @"SELECT * FROM Leituras";
            }

            using SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Leitura leitura = new()
                {
                    DataHora = DateTime.Parse(reader["DataHora"].ToString()),
                    PontoMedicao = Convert.ToInt32(reader["PontoMedicao"]),
                    Temperatura = Convert.ToDouble(reader["Temperatura"]),
                    Umidade = Convert.ToDouble(reader["Umidade"]),
                    Indice = Convert.ToInt32(reader["Indice"]),
                    Condicao = reader["Condicao"].ToString()
                };
                leituras.Add(leitura);
            }            

            return leituras;
        }

        public string VerificarCondicoes(double temperatura, double umidade)
        {
            if (temperatura > 30 && umidade >= 75 && umidade <= 90)            
                return "Condições boas na plantação, mas requer atenção";            
            else if (temperatura > 30 && umidade <= 30)            
                return "Condições de emergência na plantação";            
            else            
                return "Condições normais na plantação";            
        }
    }

}