using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SMS_Reminder
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string connetionString = @"Data Source=[CONNECTION_STRING]";
            SqlCommand command;
            string sql = "Select * from [TABLE]";
            SqlDataReader dataReader;
            SqlConnection connection = new SqlConnection(connetionString);

            string accountSid = "[ACCOUNT_SID]";
            string authToken = "[AUTH_TOKEN]";
            TwilioClient.Init(accountSid, authToken);

            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                while (!stoppingToken.IsCancellationRequested)
                {
                    dataReader = command.ExecuteReader();
                    MessageResource message;
                    var now = DateTime.Now;
                    while (dataReader.Read())
                    {
                        var date = DateTime.Parse(dataReader["date"].ToString());
                        if ((now.Date.AddDays(1) == date.Date)&&(now.Hour==date.Hour))
                        {
                            _logger.LogInformation("It works");
                            message = MessageResource.Create(
                            body: $"Dear {dataReader.GetValue(0).ToString().Trim()} {dataReader.GetValue(1).ToString().Trim()}, we remind you of your appointment scheduled for {dataReader.GetValue(3).ToString().Trim()}",
                            from: new Twilio.Types.PhoneNumber("[PHONE_NUMBER]"),
                            to: new Twilio.Types.PhoneNumber($"{dataReader.GetValue(2)}"));
                            _logger.LogInformation("New message sent");
                        }
                    }
                    dataReader.Close();
                    await Task.Delay(3600*1000, stoppingToken);
                }
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
        }
    }
}