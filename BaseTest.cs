using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

using Dythco;

namespace Dythco
{
    [TestClass]
    public class BaseTest
    {   
        [TestMethod]
        public void getLevel()
        {
            Base item = new Base();

            string level = "";

            // Test defaults, precision 3, empty units, no percent
            Assert.IsTrue(item.getLevel(1, 100) == "001 / 100");
            Assert.IsTrue(item.getLevel(100, 100) == "100 / 100");

            // Change units
            level = item.getLevel(1, 100, 3, "items");
            Assert.IsTrue(level == "001 / 100 items");

            // Change precision
            level = item.getLevel(1, 100, 4);
            Assert.IsTrue(level == "0001 / 0100");

            // With percent
            level = item.getLevel(100, 100, showPercent: true);
            Assert.IsTrue(level == "100 / 100 - 100%");
            
            level = item.getLevel(50, 100, showPercent: true);
            Assert.IsTrue(level == "050 / 100 -  50%");

            level = item.getLevel(0, 100, showPercent: true);
            Assert.IsTrue(level == "000 / 100 -   0%");
        }

        [TestMethod]
        public void getTitle()
        {
            Base item = new Base();

            // Change spacer and padding
            Assert.IsTrue(item.getTitle("Test") == "Test: ");
        }

        [TestMethod]
        public void getBarList()
        {
            Base item = new Base();

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

            // First item
            Dictionary<string, string> first = new Dictionary<string, string>();
            first["Output"] = "10 KWh";
            first["Output Max"] = "10 KWh";
            list.Add(first);

            // Second item
            Dictionary<string, string> second = new Dictionary<string, string>();
            second["Output"] = "10 KWh";
            second["Output Max"] = "10 KWh";
            list.Add(second);
            
            string bar = item.getBar(list, l_key: "Output", h_key:"Output Max");
            Assert.IsTrue(bar == "(||||||||||||||||||||) - 100%");
        }

        [TestMethod]
        public void getBar()
        {
            Base item = new Base();

            // Test defaults with brackes and size of 20
            Assert.IsTrue(item.getBar(1) == "(||||||||||||||||||||) - 100%");
            Assert.IsTrue(item.getBar(0) == "(                    ) -   0%");

            // Check without brackets
            Assert.IsTrue(item.getBar(1, 20, false) == "|||||||||||||||||||| - 100%");
            
            // Check different size
            Assert.IsTrue(item.getBar(1, 10, false) == "|||||||||| - 100%");

            // Check without percent showing
            Assert.IsTrue(item.getBar(1, 10, false, false) == "||||||||||");
        }

        [TestMethod]
        public void getNumber()
        {
            Base item = new Base();

            // Check different levels
            Assert.IsTrue(item.getNumber(1) == "001");
            Assert.IsTrue(item.getNumber(10) == "010");
            Assert.IsTrue(item.getNumber(100) == "100");

            // Change precision
            Assert.IsTrue(item.getNumber(1000, 4) == "1000");
        }

        [TestMethod]
        public void getString()
        {
            Base item = new Base();

            Assert.IsTrue(item.getString(10, "-") == "----------");
            Assert.IsTrue(item.getString(5, " ") == "     ");
        }

        [TestMethod]
        public void getPercent()
        {
            Base item = new Base();
            
            Assert.IsTrue(item.getPercent(0) == "  0%");
            Assert.IsTrue(item.getPercent(0.5) == " 50%");
            Assert.IsTrue(item.getPercent(1) == "100%");
        }

        [TestMethod]
        public void getScreen()
        {
            Base item = new Base();
            List<string> items = new List<string>();

            items.Add("Test1");
            items.Add("Test2");

            Assert.IsTrue(item.getScreen(items) == "Test1\nTest2");
            Assert.IsTrue(item.getScreen(items, " - ") == "Test1 - Test2");

        }

        [TestMethod]
        public void calcUnits()
        {
            Base item = new Base();

            Assert.IsTrue(item.calcUnits("1 MWh") == 1000.0);
            Assert.IsTrue(item.calcUnits("1 KWh") == 1.0);
            Assert.IsTrue(item.calcUnits("1000 W") == 1.0);

            Assert.IsTrue(item.calcUnits("1 MW") == 1000.0);
            Assert.IsTrue(item.calcUnits("1 KW") == 1.0);
            Assert.IsTrue(item.calcUnits("1000 W") == 1.0);
        }

        [TestMethod]
        public void calcW()
        {
            Base item = new Base();

            Assert.IsTrue(item.calcKW("1 MW") == 1000.0);
            Assert.IsTrue(item.calcKW("750 KW") == 750.0);
        }

        [TestMethod]
        public void calcKW()
        {
            Base item = new Base();

            Assert.IsTrue(item.calcKW("1 MW") == 1000.0);
            Assert.IsTrue(item.calcKW("750 KW") == 750.0);
            Assert.IsTrue(item.calcKW("7500 W") == 7.5);
        }

        [TestMethod]
        public void calcKWh()
        {
            Base item = new Base();

            Assert.IsTrue(item.calcKWh("1 MWh") == 1000.0);
            Assert.IsTrue(item.calcKWh("750 KWh") == 750.0);
            Assert.IsTrue(item.calcKWh("7500 Wh") == 7.5);
        }

        [TestMethod]
        public void calcKilowatt()
        {
            Base item = new Base();

            Assert.IsTrue(item.calcKilowatt("1 MEGA", "MEGA", "KILO") == 1000.0);
            Assert.IsTrue(item.calcKilowatt("750 KILO", "MEGA", "KILO") == 750.0);
        }

        [TestMethod]
        public void calcListPercent()
        {
            Base item = new Base();
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

            // First item
            Dictionary<string, string> first = new Dictionary<string, string>();
            first["low"] = "5 KWh";
            first["high"] = "10 KWh";
            list.Add(first);

            // Second item
            Dictionary<string, string> second = new Dictionary<string, string>();
            second["low"] = "5 KWh";
            second["high"] = "10 KWh";
            list.Add(second);
            
            Assert.IsTrue(item.calcListPercent(list) == 0.5);
        }
        
        [TestMethod]
        public void calcPercent()
        {
            Base item = new Base();

            Assert.IsTrue(item.calcPercent(0.0, 0.0) == 0.0);
            Assert.IsTrue(item.calcPercent(1.0, 2.0) == 0.5);
            Assert.IsTrue(item.calcPercent(2.0, 1.0) == 2.0);
        }
        
        [TestMethod]
        public void getData()
        {
            Base item = new Base();
            string testData = "Test1 : Var1\nTest2 : Var2\nTest3 :";
            Dictionary<string, string> res = item.getData(testData);

            Assert.IsTrue(res["Test1"] == "Var1");
            Assert.IsTrue(res["Test2"] == "Var2");
            Assert.IsTrue(res["Test3"] == "");
        }
    }
}
