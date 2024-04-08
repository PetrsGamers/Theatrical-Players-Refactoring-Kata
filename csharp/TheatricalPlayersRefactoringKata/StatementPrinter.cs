using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = new StringBuilder();
            // result.Append("<html>\n\t<body>");
            result.Append(String.Format("Statement for {0}\n", invoice.Customer));
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = 0;
                thisAmount = getAmountPerPerformance(play, perf);
                // add volume credits
                volumeCredits += getCreditsPerPerformance(perf, play);

                // print line for this order
                result.Append(String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience));
                totalAmount += thisAmount;
            }
            result.Append(String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100)));
            result.Append(String.Format("You earned {0} credits\n", volumeCredits));
            return result.ToString();
        }

        private static int getCreditsPerPerformance(Performance perf, Play play)
        {
            int volumeCredits = Math.Max(perf.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
            return volumeCredits;
        }

        private static int getAmountPerPerformance(Play play, Performance perf)
        {
            int thisAmount;
            switch (play.Type) 
            {
                case "tragedy":
                    thisAmount = 40000;
                    if (perf.Audience > 30) {
                        thisAmount += 1000 * (perf.Audience - 30);
                    }
                    break;
                case "comedy":
                    thisAmount = 30000;
                    if (perf.Audience > 20) {
                        thisAmount += 10000 + 500 * (perf.Audience - 20);
                    }
                    thisAmount += 300 * perf.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + play.Type);
            }

            return thisAmount;
        }
        public string HtmlPrint(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = new StringBuilder();
            // result.Append("<html>\n\t<body>");
            result.Append(String.Format("Statement for {0}\n", invoice.Customer));
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = 0;
                thisAmount = getAmountPerPerformance(play, perf);
                // add volume credits
                volumeCredits += getCreditsPerPerformance(perf, play);

                // print line for this order
                result.Append(String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience));
                totalAmount += thisAmount;
            }
            result.Append(String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100)));
            result.Append(String.Format("You earned {0} credits\n", volumeCredits));
            return result.ToString();
        }
    }
}
