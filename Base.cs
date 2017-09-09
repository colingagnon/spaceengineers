using System.Collections.Generic;
using System;

namespace Dythco
{
    public class Base 
    {   
        // Gets a current out of total display with precision
        //, int size = 28, string spacer = "-", int padding = 1
        public string getLevel(int current, int total, int precision = 3, string units = "", Boolean showPercent = false) {
            string text = getNumber(current, precision) + " / " + getNumber(total, precision);

            if (units != "") {
                text += " " + units;
            }

            if (showPercent) {
                text += " - " + getPercent(calcPercent((double) current, (double) total));
            }

            return text;
        }
        
        // Display a percent bar of given size from list of dictionaries using keys, with size and optional brackets
        public string getBar(List<Dictionary<string, string>> list, string l_key = "low", string h_key = "high", int size = 20, Boolean showBrackets = true, Boolean showPercent = true) {
            return getBar(calcListPercent(list, l_key, h_key), size, showBrackets, showPercent);
        }

        // Display a percent bar of given size, with optional brackets
        public string getBar(double percent, int size = 20, Boolean showBrackets = true, Boolean showPercent = true) {
            int count = (int) Math.Floor((percent) * size);
            string text = getString(count, "|") + getString(size - count, " ");
            
            if (showBrackets) {
                text = "(" + text + ")";
            }

            if (showPercent) {
                text += " - " + getPercent(percent);
            }
            
            return text;
        }

        // Gets a title for a panel that takes a whole line
        public string getTitle(string label, string spacer = ":", int padding = 1) {
            return label + spacer + getString(padding, " ");
        }

        // Gets a number string from a int
        public string getNumber(int number, int precision = 3)
        {
            int length = 0;
            if (number == 0) {
                length = 1;
            } else {
                length = (int) Math.Floor(Math.Log10(number)) + 1;
            }

            return getString(precision - length, "0") + number.ToString();
        }

        // Gets a string of concated characters
        public string getString(int count, string character)
        {
            string text = "";
            for (int i = 0; i < count; i++)  
            {  
                text += character; 
            }

            return text;
        }

        // Gets a displayable percent value from double
        public string getPercent(double percent)
        {
            percent = percent * 100;
            if (percent == 0) {
                return "  0%";
            } else if (percent < 100) {
                return string.Concat(" ", percent.ToString(), "%");
            } else {
                return "100%";
            }
        }

        // Combines items in list with linebreak
        public string getScreen(List<string> items, string seperator = "\n")
        {
            string text = "";
            for (int i = 0; i < items.Count; i++)  
            {  
                text += items[i] + seperator; 
            }

            return text.TrimEnd(seperator.ToCharArray());
        }

        // Detects the type of data and returns number
        public double calcUnits(string data)
        {   
            // TODO implement kg
            if (data.Contains("KWh") || data.Contains("MWh") || data.Contains("Wh")) {
                return calcKWh(data);
            } else if (data.Contains("KW") || data.Contains("MW") || data.Contains("W")) {
                return calcKW(data);
            } else {
                return Convert.ToDouble(data);
            }
        }

        // Calculates KWh value from KWh or MWh string
        public double calcKW(string data)
        {
            return calcKilowatt(data, "MW", "KW", "W");
        }

        // Calculates KWh value from KWh or MWh string
        public double calcKWh(string data)
        {
            return calcKilowatt(data, "MWh", "KWh", "Wh");
        }

        // Calculates kilowatt values
        public double calcKilowatt(string data, string m = "MWh", string k = "KWh", string w = "Wh")
        {
            string[] pair = data.Split(" ".ToCharArray());
            double power = 0.0;

            if (pair.Length == 2) {
                if (pair[1] == m) {
                    power = Convert.ToDouble(pair[0]) * 1000;
                } else if (pair[1] == k) {
                    power = Convert.ToDouble(pair[0]);
                } else if (pair[1] == w) {
                    power = Convert.ToDouble(pair[0]) / 1000;
                }
            }

            return power;
        }

        // Calculates the percent from a list of objects
        public double calcListPercent(List<Dictionary<string, string>> list, string l_key = "low", string h_key = "high")
        {   
            double l = 0.0;
            double h = 0.0;
            foreach (Dictionary<string, string> item in list)
            {   
                l += calcUnits(item[l_key]);
                h += calcUnits(item[h_key]);
            }
    
           return calcPercent(l, h);
        }
        
        // Calculates percent from low and high values
        public double calcPercent(double l, double h)
        {
            if (h > 0) {
                return Math.Round(l / h, 2);
            } else {
                return 0.0;
            }
        }
        
        // Gets data from string into string dictionary from text source
        public Dictionary<string, string> getData(string data)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            string[] lines = data.Split(Environment.NewLine.ToCharArray());

            foreach (string line in lines) 
            {
                string[] pairs = line.Split(":".ToCharArray());
                res[pairs[0].Trim()] = pairs[1].Trim();
            }

            return res;
        }
    }
}