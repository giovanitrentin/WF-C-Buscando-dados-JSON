using System;

namespace Model
{
    public class LogMaquina
    {

        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public bool IsErro { get; set; }
        public DateTime DataEvento { get; set; }

    }
}
