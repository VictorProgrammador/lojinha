using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.InfraStructure
{
    public class LogWriter : ILogWritter
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public LogWriter(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Writer(string Message)
        {
            var insertLog = @"INSERT INTO [Log] VALUES (@Message)";

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                int rows = db.Execute(insertLog, new
                {
                    Message = Message
                });

                db.Close();
            }
        }
    }
}
