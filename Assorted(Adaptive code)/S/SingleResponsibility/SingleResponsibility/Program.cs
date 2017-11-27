using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SingleResponsibility
{
    public class TradeRecord
    {
        internal string DestinationCurrency;
        internal float Lots;
        internal decimal Price;
        internal string SourceCurrency;
    } 
    public class BloatedTradeProcessor
    {
        public void ProcessTrades(Stream stream)
        {
            // read rows
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            // parse fields and validate
            var trades = new List<TradeRecord>();

            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(new char[] { ',' });

                if (fields.Length != 3)
                {
                    Console.WriteLine("WARN: Line {0} malformed. Only {1} field(s) found.", lineCount, fields.Length);
                    continue;
                }

                if (fields[0].Length != 6)
                {
                    Console.WriteLine("WARN: Trade currencies on line {0} malformed: '{1}'", lineCount, fields[0]);
                    continue;
                }

                int tradeAmount;
                if (!int.TryParse(fields[1], out tradeAmount))
                {
                    Console.WriteLine("WARN: Trade amount on line {0} not a valid integer: '{1}'", lineCount, fields[1]);
                }

                decimal tradePrice;
                if (!decimal.TryParse(fields[2], out tradePrice))
                {
                    Console.WriteLine("WARN: Trade price on line {0} not a valid decimal: '{1}'", lineCount, fields[2]);
                }

                var sourceCurrencyCode = fields[0].Substring(0, 3);
                var destinationCurrencyCode = fields[0].Substring(3, 3);

                // calculate values
                var trade = new TradeRecord
                {
                    SourceCurrency = sourceCurrencyCode,
                    DestinationCurrency = destinationCurrencyCode,
                    Lots = tradeAmount / LotSize,
                    Price = tradePrice
                };

                trades.Add(trade);

                lineCount++;
            }
            // insert into db
            using (var connection = new System.Data.SqlClient.SqlConnection("Data Source=(local);Initial Catalog=TradeDatabase;Integrated Security=True;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.insert_trade";
                        command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                connection.Close();
            }

            Console.WriteLine("INFO: {0} trades processed", trades.Count);
        }

        private static float LotSize = 100000f;
    }

    public class ClarifiedTradeProcessor
    {
        private IEnumerable<string> ReadTradeData(Stream stream)
        {
            var tradeData = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tradeData.Add(line);
                }
            }
            return tradeData;
        }

        private IEnumerable<TradeRecord> ParseTrades(IEnumerable<string> tradeData)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in tradeData)
            {
                var fields = line.Split(new char[] { ',' });

                // use method to simplify
                if (!ValidateTradeData(fields, lineCount))
                {
                    continue;
                }

                // use method to simplify
                var trade = MapTradeDataToTradeRecord(fields);

                trades.Add(trade);

                lineCount++;
            }

            return trades;
        }

        private bool ValidateTradeData(string[] fields, int currentLine)
        {
            if (fields.Length != 3)
            {
                LogMessage("WARN: Line {0} malformed. Only {1} field(s) found.", currentLine, fields.Length);
                return false;
            }

            if (fields[0].Length != 6)
            {
                LogMessage("WARN: Trade currencies on line {0} malformed: '{1}'", currentLine, fields[0]);
                return false;
            }

            int tradeAmount;
            if (!int.TryParse(fields[1], out tradeAmount))
            {
                LogMessage("WARN: Trade amount on line {0} not a valid integer: '{1}'", currentLine, fields[1]);
                return false;
            }

            decimal tradePrice;
            if (!decimal.TryParse(fields[2], out tradePrice))
            {
                LogMessage("WARN: Trade price on line {0} not a valid decimal: '{1}'", currentLine, fields[2]);
                return false;
            }

            return true;
        }

        private void LogMessage(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        private TradeRecord MapTradeDataToTradeRecord(string[] fields)
        {
            var sourceCurrencyCode = fields[0].Substring(0, 3);
            var destinationCurrencyCode = fields[0].Substring(3, 3);
            var tradeAmount = int.Parse(fields[1]);
            var tradePrice = decimal.Parse(fields[2]);

            var trade = new TradeRecord
            {
                SourceCurrency = sourceCurrencyCode,
                DestinationCurrency = destinationCurrencyCode,
                Lots = tradeAmount / LotSize,
                Price = tradePrice
            };

            return trade;
        }

        private void StoreTrades(IEnumerable<TradeRecord> trades)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection("Data Source=(local);Initial Catalog=TradeDatabase;Integrated Security=True;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.insert_trade";
                        command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                connection.Close();
            }

            LogMessage("INFO: {0} trades processed", trades.Count());
        }

        public void ProcessTrades(Stream stream)
        {
            // separate methods
            var lines = ReadTradeData(stream);
            var trades = ParseTrades(lines);
            StoreTrades(trades);
        }

        private static float LotSize = 100000f;
    }

    public interface ITradeDataProvider
    {
        // method does not depend on Stream any more
        IEnumerable<string> GetTradeData();
    }
    public interface ITradeParser
    {
        IEnumerable<TradeRecord> Parse(IEnumerable<string> tradeData);
    }
    public interface ITradeValidator
    {
        bool Validate(string[] tradeData);
    }
    public interface ITradeMapper
    {
        TradeRecord Map(string[] fields);
    }
    public interface ILogger
    {
        void LogWarning(string message, params object[] args);

        void LogInfo(string message, params object[] args);
    }
    public interface ITradeStorage
    {
        void Persist(IEnumerable<TradeRecord> trades);
    }
    /// <summary>
    /// The class models the process of transferring trade data from one format to another.
    /// This is its only responsibility, its only concern, and the only reason that this class should change.
    /// If the process itself changes, this class will change to reflect it.
    /// But if you decide you no longer want to retrieve data from a Stream,
    /// log to the console, or store the trades in a database, this class remains as is.
    /// </summary>
    public class TradeProcessor
    {
        private readonly ITradeDataProvider tradeDataProvider;
        private readonly ITradeParser tradeParser;
        private readonly ITradeStorage tradeStorage;

        public TradeProcessor(ITradeDataProvider tradeDataProvider, ITradeParser tradeParser, ITradeStorage tradeStorage)
        {
            this.tradeDataProvider = tradeDataProvider;
            this.tradeParser = tradeParser;
            this.tradeStorage = tradeStorage;
        }
        public void ProcessTrades()
        {
            var lines = tradeDataProvider.GetTradeData();
            var trades = tradeParser.Parse(lines);
            tradeStorage.Persist(trades);
        }
    }
    public class StreamTradeDataProvider : ITradeDataProvider
    {
        // StreamTradeDataProvider requires a Stream as a constructor parameter,
        // instead of a method parameter. By using the constructor, you can depend
        // on almost anything without polluting the interface
        public StreamTradeDataProvider(Stream stream)
        {
            this.stream = stream;
        }

        public IEnumerable<string> GetTradeData()
        {
            var tradeData = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tradeData.Add(line);
                }
            }
            return tradeData;
        }

        private readonly Stream stream;
    }
    /// <summary>
    /// Its single reason for change is if the overall structure of the trade data changes—for instance,
    /// if the data no longer uses comma-separated values and changes to using tabs, or perhaps XML.
    /// </summary>
    public class SimpleTradeParser : ITradeParser
    {
        // again delegate the work to interfaces
        private readonly ITradeValidator tradeValidator;
        private readonly ITradeMapper tradeMapper;

        public SimpleTradeParser(ITradeValidator tradeValidator, ITradeMapper tradeMapper)
        {
            this.tradeValidator = tradeValidator;
            this.tradeMapper = tradeMapper;
        }

        public IEnumerable<TradeRecord> Parse(IEnumerable<string> tradeData)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in tradeData)
            {
                var fields = line.Split(new char[] { ',' });

                if (!tradeValidator.Validate(fields))
                {
                    continue;
                }

                var trade = tradeMapper.Map(fields);

                trades.Add(trade);

                lineCount++;
            }

            return trades;
        }
    }
    public class SimpleTradeValidator : ITradeValidator
    {
        private readonly ILogger logger;

        public SimpleTradeValidator(ILogger logger)
        {
            this.logger = logger;
        }

        public bool Validate(string[] tradeData)
        {
            if (tradeData.Length != 3)
            {
                logger.LogWarning("Line malformed. Only {0} field(s) found.", tradeData.Length);
                return false;
            }

            if (tradeData[0].Length != 6)
            {
                logger.LogWarning("Trade currencies malformed: '{0}'", tradeData[0]);
                return false;
            }

            int tradeAmount;
            if (!int.TryParse(tradeData[1], out tradeAmount))
            {
                logger.LogWarning("Trade not a valid integer: '{0}'", tradeData[1]);
                return false;
            }

            decimal tradePrice;
            if (!decimal.TryParse(tradeData[2], out tradePrice))
            {
                logger.LogWarning("Trade price not a valid decimal: '{0}'", tradeData[2]);
                return false;
            }

            return true;
        }
    }
    public class ConsoleLogger : ILogger
    {
        public void LogWarning(string message, params object[] args)
        {
            Console.WriteLine(string.Concat("WARN: ", message), args);
        }

        public void LogInfo(string message, params object[] args)
        {
            Console.WriteLine(string.Concat("INFO: ", message), args);
        }
    }
    public class SimpleTradeMapper : ITradeMapper
    {
        public TradeRecord Map(string[] fields)
        {
            var sourceCurrencyCode = fields[0].Substring(0, 3);
            var destinationCurrencyCode = fields[0].Substring(3, 3);
            var tradeAmount = int.Parse(fields[1]);
            var tradePrice = decimal.Parse(fields[2]);

            var trade = new TradeRecord
            {
                SourceCurrency = sourceCurrencyCode,
                DestinationCurrency = destinationCurrencyCode,
                Lots = tradeAmount / LotSize,
                Price = tradePrice
            };

            return trade;
        }

        private static float LotSize = 100000f;
    }
    public class AdoNetTradeStorage : ITradeStorage
    {
        private readonly ILogger logger;

        public AdoNetTradeStorage(ILogger logger)
        {
            this.logger = logger;
        }

        public void Persist(IEnumerable<TradeRecord> trades)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection("Data Source=(local);Initial Catalog=TradeDatabase;Integrated Security=True;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.insert_trade";
                        command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                connection.Close();
            }

            logger.LogInfo("{0} trades processed", trades.Count());
        }
    }

    class Program
    {
        /// <summary>
        /// The single responsibility principle (SRP) instructs developers to write
        /// code that has one and only one reason to change. If a class has more than one
        /// reason to change, it has more than one responsibility.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // One class is bloated
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("somefilename.txt");
            var tradeProcessor = new BloatedTradeProcessor();
            tradeProcessor.ProcessTrades(tradeStream);
            // Still one bloated class but with several methods
            var tradeProcessor2 = new ClarifiedTradeProcessor();
            tradeProcessor2.ProcessTrades(tradeStream);
            // With single responsibility
            var logger = new ConsoleLogger();
            var tradeValidator = new SimpleTradeValidator(logger);
            var tradeDataProvider = new StreamTradeDataProvider(tradeStream);
            var tradeMapper = new SimpleTradeMapper();
            var tradeParser = new SimpleTradeParser(tradeValidator, tradeMapper);
            var tradeStorage = new AdoNetTradeStorage(logger);

            var tradeProcessor3 = new TradeProcessor(tradeDataProvider, tradeParser, tradeStorage);
            tradeProcessor3.ProcessTrades();
            Console.ReadKey();
        }
    }
}
