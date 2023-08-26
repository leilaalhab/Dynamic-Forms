using System;
using DynamicForms.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using DynamicForms.Models.Answers;


namespace DynamicForms.Data
{
	public class DataContext : DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {   
        }

        public DbSet<Form> Forms => Set<Form>();
        public DbSet<Input> Inputs => Set<Input>();
        public DbSet<Choice> Choices => Set<Choice>();
        public DbSet<Step> Steps => Set<Step>();
        public DbSet<Requirement> Requirements => Set<Requirement>();
        public DbSet<Condition> Conditions => Set<Condition>();
        public DbSet<Progress> Progresses => Set<Progress>();
        public DbSet<TextAnswer> TextAnswers => Set<TextAnswer>();
        public DbSet<DoubleAnswer> DoubleAnswers => Set<DoubleAnswer>();
        public DbSet<IntegerAnswer> IntegerAnswers => Set<IntegerAnswer>();
        
        
    }
}

