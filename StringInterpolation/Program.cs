using System;
using System.IO;
using NLog;
using NLog.Web;
namespace StringInterpolation
{
    class Program
    {
        static void Main(string[] args)
        {
            // ask for input
            Console.WriteLine("Enter 1 to create data file.");
            Console.WriteLine("Enter 2 to parse data.");
            Console.WriteLine("Enter anything else to quit.");
            
            string path = Directory.GetCurrentDirectory() + "\\nlog.config";
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();
            
            // input response
            string resp = Console.ReadLine();

            if (resp == "1")
            {
                // create data file

                // ask a question
                Console.WriteLine("How many weeks of data is needed?");
                // input the response (convert to int)
                int weeks = int.Parse(Console.ReadLine());

                // determine start and end date
                DateTime today = DateTime.Now;
                // we want full weeks sunday - saturday
                DateTime dataEndDate = today.AddDays(-(int) today.DayOfWeek);
                // subtract # of weeks from endDate to get startDate
                DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));

                // random number generator
                Random rnd = new Random();

                // create file
                StreamWriter sw = new StreamWriter("data.txt");
                // loop for the desired # of weeks
                while (dataDate < dataEndDate)
                {
                    // 7 days in a week
                    int[] hours = new int[7];
                    for (int i = 0; i < hours.Length; i++)
                    {
                        // generate random number of hours slept between 4-12 (inclusive)
                        hours[i] = rnd.Next(4, 13);
                    }

                    // M/d/yyyy,#|#|#|#|#|#|#
                    //Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
                    sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
                    // add 1 week to date
                    dataDate = dataDate.AddDays(7);
                }

                sw.Close();
            }
            else if (resp == "2")
            {
                // TODO: parse data file
                /*
                 * Code executed on "2" to parse data created in file
                 */
                string file = "data.txt";

                /*
                 * Display error immediately if file not found
                 */
                if (!File.Exists(file))
                {
                    Console.WriteLine($"Error: {file} does not exist");
                }
                else
                {
                    /*
                     * Create streamreader object and loop thru file before EOF
                     */
                    StreamReader reader = new StreamReader(file);

                    double netTotal = 0;
                    double netCount = 0;
                    double netAvg = 0;
                    while (!reader.EndOfStream)
                    {
                        /*
                         * Running total vars - string line for readline of file
                         */
                        double totalHours = 0;
                        double average = 0;
                        double avgFormat = 0;
                        string input = reader.ReadLine();

                        /*
                         * splitting string on comma delimiter into array
                         */
                        string[] wk = input.Split(',');

                        /*
                         * create new datetime object - TryParse for validation and conversion
                         */
                        DateTime date;
                        DateTime.TryParse(wk[0], out date);

                        /*
                         * Handling field delimiting by | containing hours sleep data
                         */
                        double[] hr = Array.ConvertAll(wk[1].Split('|'), Double.Parse);

                        /*
                         * For each sleep hour value add to running total for avg, total
                         */
                        foreach (var value in hr)
                        {
                            totalHours += value;
                            netCount++;
                        }

                        /*
                         * Divide hrs by 7 - format to 2 decimal place for output isnt crazy
                         */
                        average = totalHours / 7;
                        netTotal += totalHours;
                        netAvg = netTotal / netCount;
                        avgFormat = Math.Round(average, 2);

                        Console.WriteLine($"Week of {date:MMM}, {date:dd}, {date:yyyy}");

                        Console.WriteLine(
                            $"{"Sun",4}{"Mon",4}{"Tue",4}{"Wed",4}{"Thu",4}{"Fri",4}{"Sat",4}{"Total",6}{"Average",8}");
                        Console.WriteLine(
                            $"{"---",4}{"---",4}{"---",4}{"---",4}{"---",4}{"---",4}{"---",4}{"-----",6}{"-------",8}");
                        // display hours of sleep for each day
                        Console.WriteLine(
                            $"{hr[0],4}{hr[1],4}{hr[2],4}{hr[3],4}{hr[4],4}{hr[5],4}{hr[6],4}{totalHours,6}{avgFormat,8}");
                        Console.WriteLine();
                        Console.WriteLine($"Net Total: {netTotal} hours");
                        Console.WriteLine($"Net Average: {netAvg:N2} hours");
                        Console.WriteLine();
                    }
                }
            }
            logger.Info("Program ended");
        }
    }
}