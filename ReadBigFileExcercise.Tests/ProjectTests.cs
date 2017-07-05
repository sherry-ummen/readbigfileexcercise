using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadBigFileExcercise.Tests
{
    [TestFixture()]
    public class ProjectTests
    {
        private IDictionary<string, int> columnNameWithCorrectOrder = Project.ColumnNames.ToDictionary<string, int>();
        private IDictionary<string, int> columnNameWithDifferentOrder => GetReverseOrder(Project.ColumnNames);

        [Test()]
        public void ParseValidProject()
        {
            string lineEntry = "8944\td5c93d3e-74c4-40bf-8e6a-fdaad223f8e9\t2017-10-02 18:37:08.075\t7ee7\te72eae\t10484.0651573646\tINR\tSimple";
            var project = Project.Create(lineEntry.Split('\t'), columnNameWithCorrectOrder);
            project.IsSuccess.Should().BeTrue();
            project.Value.Id.Should().Be(8944);
            project.Value.Description.Should().Be("d5c93d3e-74c4-40bf-8e6a-fdaad223f8e9");
            project.Value.StartDate.Should().Be(DateTime.ParseExact("2017-10-02 18:37:08.075", "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            project.Value.Category.Should().Be("7ee7");
            project.Value.Responsible.Should().Be("e72eae");
            project.Value.Savings.Should().Be(10484.0651573646);
            project.Value.Currency.Should().Be("INR");
            project.Value.Complexity.Should().Be(Complexity.Simple);
        }

        [Test()]
        public void ParseValidProjectButDifferentColumnOrder()
        {
            string lineEntry = "Simple\tINR\t10484.0651573646\te72eae\t7ee7\t2017-10-02 18:37:08.075\td5c93d3e-74c4-40bf-8e6a-fdaad223f8e9\t8944";
            var project = Project.Create(lineEntry.Split('\t'), columnNameWithDifferentOrder);
            project.IsSuccess.Should().BeTrue();
            project.Value.Id.Should().Be(8944);
            project.Value.Description.Should().Be("d5c93d3e-74c4-40bf-8e6a-fdaad223f8e9");
            project.Value.StartDate.Should().Be(DateTime.ParseExact("2017-10-02 18:37:08.075", "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            project.Value.Category.Should().Be("7ee7");
            project.Value.Responsible.Should().Be("e72eae");
            project.Value.Savings.Should().Be(10484.0651573646);
            project.Value.Currency.Should().Be("INR");
            project.Value.Complexity.Should().Be(Complexity.Simple);
        }

        [Test()]
        public void ParseProjectWithSavingsAndCurrencyWithEmptyValues()
        {
            string lineEntry = "8944\td5c93d3e-74c4-40bf-8e6a-fdaad223f8e9\t2017-10-02 18:37:08.075\t7ee7\te72eae\tNULL\tNULL\tSimple";
            var project = Project.Create(lineEntry.Split('\t'), columnNameWithCorrectOrder);
            project.IsSuccess.Should().BeTrue();
            project.Value.Savings.Should().Be(0.0);
            project.Value.Currency.Should().BeEmpty();
        }

        [Test()]
        public void ParseProjectWithInvalidValues()
        {
            string lineEntry = "8944\td5c93d3e-74c4-40bf-8e6a-fdaad223f8e9\t2017-10-02 18:37:08.075\t7ee7\te72eae\tNULL\tNULL\tINVALID";
            var project = Project.Create(lineEntry.Split('\t'), columnNameWithCorrectOrder);
            project.Error.Should().Be("Value conversion error {Requested value 'INVALID' was not found.} for the column {Complexity}");
        }

        [Test()]
        public void ToStringTest()
        {
            string lineEntry = "8944\td5c93d3e-74c4-40bf-8e6a-fdaad223f8e9\t2017-10-02 18:37:08.075\t7ee7\te72eae\t10484.0651573646\tINR\tSimple";
            var project = Project.Create(lineEntry.Split('\t'), columnNameWithCorrectOrder);
            project.Value.ToString().Should().Be(lineEntry);
        }

        [Test()]
        public void ToStringWithNullValuesTest()
        {
            string lineEntry = "8944\td5c93d3e-74c4-40bf-8e6a-fdaad223f8e9\t2017-10-02 18:37:08.075\t7ee7\te72eae\tNULL\tNULL\tSimple";
            var project = Project.Create(lineEntry.Split('\t'), columnNameWithCorrectOrder);
            project.Value.ToString().Should().Be("8944	d5c93d3e-74c4-40bf-8e6a-fdaad223f8e9	2017-10-02 18:37:08.075	7ee7	e72eae			Simple");
        }

        private IDictionary<string, int> GetReverseOrder(IEnumerable<string> items)
        {
            var dict = new Dictionary<string, int>();
            var itemsCount = items.Count() - 1;
            foreach (var item in items)
            {
                dict.Add(item, itemsCount--);
            }
            return dict;
        }

    }

}