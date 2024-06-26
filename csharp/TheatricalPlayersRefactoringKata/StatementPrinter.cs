﻿using System;
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
            // result.Append("<html>\n<body>");
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
                var decimalPerformanceAmount = Convert.ToDecimal(thisAmount / 100);
                result.Append(String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, decimalPerformanceAmount, perf.Audience));
                totalAmount += thisAmount;
            }

            var deccimalTotalAmount = Convert.ToDecimal(totalAmount / 100);
            result.Append(String.Format(cultureInfo, "Amount owed is {0:C}\n", deccimalTotalAmount));
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
            result.Append("<html>\n<body>\n");
            result.Append(String.Format("<h1>Statement for {0}</h1>\n", invoice.Customer));
            CultureInfo cultureInfo = new CultureInfo("en-US");

            result.Append("<table>\n");
            result.Append("<tr><th>play</th><th>seats</th><th>cost</th></tr>\n");
            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var amountPerPerformance = 0;
                amountPerPerformance = getAmountPerPerformance(play, perf);
                // add volume credits
                volumeCredits += getCreditsPerPerformance(perf, play);

                // print line for this order
                var decimalPerformanceAmount = Convert.ToDecimal(amountPerPerformance / 100);
                result.Append(String.Format(cultureInfo, "<tr><td>{0}</td><td>{2}</td><td>{1:C}</td></tr>\n",
                    play.Name, decimalPerformanceAmount, perf.Audience));
                totalAmount += amountPerPerformance;
            }
            result.Append("</table>\n");
            var decimalTotalAmount = Convert.ToDecimal(totalAmount / 100);
            result.Append(String.Format(cultureInfo, "<p>Amount owed is <em>{0:C}</em></p>\n", decimalTotalAmount));
            result.Append(String.Format("<p>You earned <em>{0}</em> credits</p>\n", volumeCredits));
            result.Append("</body>\n</html>");

            return result.ToString();
        }
        
    }
}
