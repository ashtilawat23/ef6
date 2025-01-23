using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Utilities
{
    /// <summary>
    /// Provides validation and analysis of Entity Framework configurations to help identify potential issues
    /// and ensure best practices are followed.
    /// </summary>
    public class ConfigurationValidator
    {
        private readonly DbContext _context;
        private readonly List<ValidationResult> _validationResults;

        public ConfigurationValidator(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _validationResults = new List<ValidationResult>();
        }

        /// <summary>
        /// Performs a comprehensive validation of the DbContext configuration.
        /// </summary>
        /// <returns>A collection of validation results containing any warnings or errors found.</returns>
        public IReadOnlyList<ValidationResult> ValidateConfiguration()
        {
            ValidateConnectionString();
            ValidateDatabaseInitializer();
            ValidateModelConfiguration();
            ValidatePerformanceSettings();

            return _validationResults.AsReadOnly();
        }

        private void ValidateConnectionString()
        {
            var connection = _context.Database.Connection;
            
            if (string.IsNullOrEmpty(connection.ConnectionString))
            {
                _validationResults.Add(new ValidationResult(
                    Severity.Error,
                    "Connection string is missing",
                    "Ensure a valid connection string is provided in the configuration."));
            }

            if (connection is EntityConnection entityConnection)
            {
                ValidateEntityConnectionString(entityConnection);
            }
        }

        private void ValidateDatabaseInitializer()
        {
            var initializer = Database.GetInitializer(_context.GetType());
            if (initializer == null)
            {
                _validationResults.Add(new ValidationResult(
                    Severity.Warning,
                    "No database initializer configured",
                    "Consider configuring a database initializer for development environments."));
            }
        }

        private void ValidateModelConfiguration()
        {
            var modelBuilder = ((IObjectContextAdapter)_context).ObjectContext.MetadataWorkspace;
            
            // Check for unmapped properties
            var entityTypes = modelBuilder.GetItems<EntityType>(DataSpace.CSpace);
            foreach (var entityType in entityTypes)
            {
                if (!entityType.Properties.Any())
                {
                    _validationResults.Add(new ValidationResult(
                        Severity.Warning,
                        $"Entity {entityType.Name} has no mapped properties",
                        "Ensure all entities have properly mapped properties."));
                }
            }
        }

        private void ValidatePerformanceSettings()
        {
            // Check AutoDetectChangesEnabled
            if (_context.Configuration.AutoDetectChangesEnabled)
            {
                _validationResults.Add(new ValidationResult(
                    Severity.Info,
                    "AutoDetectChangesEnabled is enabled",
                    "Consider disabling AutoDetectChangesEnabled for better performance in specific scenarios."));
            }

            // Check LazyLoadingEnabled
            if (_context.Configuration.LazyLoadingEnabled)
            {
                _validationResults.Add(new ValidationResult(
                    Severity.Info,
                    "LazyLoadingEnabled is enabled",
                    "Be aware that lazy loading can lead to N+1 query problems. Consider eager loading for better performance."));
            }
        }

        private void ValidateEntityConnectionString(EntityConnection entityConnection)
        {
            try
            {
                var builder = new EntityConnectionStringBuilder(entityConnection.ConnectionString);
                if (string.IsNullOrEmpty(builder.ProviderConnectionString))
                {
                    _validationResults.Add(new ValidationResult(
                        Severity.Error,
                        "Provider connection string is missing",
                        "Entity connection string must include a valid provider connection string."));
                }
            }
            catch (ArgumentException)
            {
                _validationResults.Add(new ValidationResult(
                    Severity.Error,
                    "Invalid entity connection string format",
                    "The entity connection string is not properly formatted."));
            }
        }
    }

    public class ValidationResult
    {
        public Severity Severity { get; }
        public string Message { get; }
        public string Recommendation { get; }

        public ValidationResult(Severity severity, string message, string recommendation)
        {
            Severity = severity;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Recommendation = recommendation ?? throw new ArgumentNullException(nameof(recommendation));
        }
    }

    public enum Severity
    {
        Info,
        Warning,
        Error
    }
} 