using AutoMapper;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ReadBigFileExcercise
{
    public enum Complexity
    {
        Simple, Moderate, Hazardous
    }

    public class Project
    {
        [Description("Project")]
        public int Id { get; set; }

        [Description("Description")]
        public string Description { get; set; }

        [Description("Start date")]
        public DateTime StartDate { get; set; }

        [Description("Category")]
        public string Category { get; set; }

        [Description("Responsible")]
        public string Responsible { get; set; }

        [Description("Savings amount")]
        public double Savings { get; set; }

        [Description("Currency")]
        public string Currency { get; set; }

        [Description("Complexity")]
        public Complexity Complexity { get; set; }

        public static IEnumerable<string> ColumnNames {
            get {
                return typeof(Project).GetProperties()
                                        .Where(z => z.CustomAttributes.Count() > 0)
                                        .Select(x => x.CustomAttributes.First().ConstructorArguments.First().Value.ToString());
            }
        }

        public static Result<Project> Create(string[] columnValues, IDictionary<string, int> columnNameWithOrder)
        {
            try
            {
                var columnIndices = columnNameWithOrder.Values.ToArray();
                Mapper.Initialize(cfg => cfg.CreateMap<string[], Project>()
                .ForMember(fe => fe.Id, opt => opt.MapFrom(s => columnValues[columnIndices[0]]))
                .ForMember(fe => fe.Description, opt => opt.MapFrom(s => columnValues[columnIndices[1]]))
                .ForMember(fe => fe.StartDate, opt => opt.MapFrom(s => DateTime.ParseExact(columnValues[columnIndices[2]], "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture)))
                .ForMember(fe => fe.Category, opt => opt.MapFrom(s => columnValues[columnIndices[3]]))
                .ForMember(fe => fe.Responsible, opt => opt.MapFrom(s => columnValues[columnIndices[4]]))
                .ForMember(fe => fe.Savings, opt => opt.ResolveUsing<CustomNullValueResolverForDoubleValues, string>(s => columnValues[columnIndices[5]]))
                .ForMember(fe => fe.Currency, opt => opt.ResolveUsing<CustomNullValueResolverForStringValues, string>(s => columnValues[columnIndices[6]]))
                .ForMember(fe => fe.Complexity, opt => opt.MapFrom(s => columnValues[columnIndices[7]]))
                );
                var project = Mapper.Map<string[], Project>(columnValues);
                return Result.Ok(project);
            }
            catch (AutoMapperMappingException exception)
            {
                return Result.Fail<Project>($"Value conversion error {{{exception.InnerException.Message}}} for the column {{{exception.PropertyMap.DestinationProperty.Name}}}");
            }
            catch (Exception exception)
            {
                return Result.Fail<Project>(exception.Message);
            }

        }

        public override string ToString()
        {
            var savings = Savings == 0.0 ? "" : Savings.ToString();
            var currency = Currency == "" ? "" : Currency;
            return $"{Id}\t{Description}\t{StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}\t{Category}\t{Responsible}\t{savings}\t{currency}\t{Complexity}";
        }

    }

    public class CustomNullValueResolverForDoubleValues : IMemberValueResolver<string[], Project, string, double>
    {
        public double Resolve(string[] source, Project destination, string sourceMember, double destMember, ResolutionContext context)
        {
            if (sourceMember.Equals("NULL")) return 0.0;
            return Double.Parse(sourceMember);
        }
    }

    public class CustomNullValueResolverForStringValues : IMemberValueResolver<string[], Project, string, string>
    {
        public string Resolve(string[] source, Project destination, string sourceMember, string destMember, ResolutionContext context)
        {
            if (sourceMember.Equals("NULL")) return "";
            return sourceMember;
        }
    }
}
