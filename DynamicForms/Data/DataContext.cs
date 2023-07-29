using System;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;


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
    }
}

