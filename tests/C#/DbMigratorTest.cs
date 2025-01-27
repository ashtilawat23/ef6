using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using Moq;
using Xunit;

namespace System.Data.Entity.Migrations.Tests
{
    public class DbMigratorTests
    {
        private Mock<DbMigrationsConfiguration> _mockConfiguration;
        private Mock<DbContext> _mockContext;
        private Mock<DbProviderFactory> _mockProviderFactory;
        private Mock<MigrationAssembly> _mockMigrationAssembly;
        private Mock<HistoryRepository> _mockHistoryRepository;
        private DbMigrator _dbMigrator;

        public DbMigratorTests()
        {
            _mockConfiguration = new Mock<DbMigrationsConfiguration>();
            _mockContext = new Mock<DbContext>();
            _mockProviderFactory = new Mock<DbProviderFactory>();
            _mockMigrationAssembly = new Mock<MigrationAssembly>();
            _mockHistoryRepository = new Mock<HistoryRepository>();

            _dbMigrator = new DbMigrator(_mockConfiguration.Object, _mockContext.Object);
        }

        [Fact]
        public void Constructor_ShouldInitialize_WhenCalledWithValidConfiguration()
        {
            // Arrange
            var configuration = new DbMigrationsConfiguration();

            // Act
            var migrator = new DbMigrator(configuration);

            // Assert
            Assert.NotNull(migrator);
            Assert.Equal(configuration, migrator.Configuration);
        }

        [Fact]
        public void GetLocalMigrations_ShouldReturnMigrationIds_WhenMigrationsExist()
        {
            // Arrange
            var migrationIds = new[] { "Migration1", "Migration2" };
            _mockMigrationAssembly.Setup(m => m.MigrationIds).Returns(migrationIds);
            _dbMigrator = new DbMigrator(_mockConfiguration.Object, _mockContext.Object);

            // Act
            var result = _dbMigrator.GetLocalMigrations();

            // Assert
            Assert.Equal(migrationIds, result);
        }

        [Fact]
        public void GetDatabaseMigrations_ShouldReturnAppliedMigrations_WhenCalled()
        {
            // Arrange
            var migrationIds = new[] { "Migration1", "Migration2" };
            _mockHistoryRepository.Setup(h => h.GetMigrationsSince(DbMigrator.InitialDatabase)).Returns(migrationIds);
            _dbMigrator = new DbMigrator(_mockConfiguration.Object, _mockContext.Object);

            // Act
            var result = _dbMigrator.GetDatabaseMigrations();

            // Assert
            Assert.Equal(migrationIds, result);
        }

        [Fact]
        public void GetPendingMigrations_ShouldReturnPendingMigrations_WhenCalled()
        {
            // Arrange
            var migrationIds = new[] { "Migration3", "Migration4" };
            _mockHistoryRepository.Setup(h => h.GetPendingMigrations(It.IsAny<IEnumerable<string>>())).Returns(migrationIds);
            _dbMigrator = new DbMigrator(_mockConfiguration.Object, _mockContext.Object);

            // Act
            var result = _dbMigrator.GetPendingMigrations();

            // Assert
            Assert.Equal(migrationIds, result);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenMigrationNotFound()
        {
            // Arrange
            _mockMigrationAssembly.Setup(m => m.GetMigration(It.IsAny<string>())).Returns((DbMigration)null);
            _dbMigrator = new DbMigrator(_mockConfiguration.Object, _mockContext.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _dbMigrator.Update("NonExistentMigration"));
        }

        [Fact]
        public void Update_ShouldCallEnsureDatabaseExists_WhenCalled()
        {
            // Arrange
            var wasCalled = false;
            var migrator = new Mock<DbMigrator>(_mockConfiguration.Object, _mockContext.Object)
            {
                CallBase = true
            };

            migrator.Setup(m => m.EnsureDatabaseExists(It.IsAny<Action>())).Callback(() => wasCalled = true);

            // Act
            migrator.Object.Update("Migration1");

            // Assert
            Assert.True(wasCalled);
        }

        [Fact]
        public void ScaffoldInitialCreate_ShouldReturnNull_WhenDatabaseModelIsNull()
        {
            // Arrange
            string migrationId;
            _mockHistoryRepository.Setup(h => h.GetLastModel(out migrationId, out It.Ref<string>.IsAny, It.IsAny<string>())).Returns((XDocument)null);
            _dbMigrator = new DbMigrator(_mockConfiguration.Object, _mockContext.Object);

            // Act
            var result = _dbMigrator.ScaffoldInitialCreate("TestNamespace");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Scaffold_ShouldThrowException_WhenPendingMigrationsExist()
        {
            // Arrange
            var pendingMigrations = new List<string> { "Migration2" };
            _mockHistoryRepository.Setup(h => h.GetPendingMigrations(It.IsAny<IEnumerable<string>>())).Returns(pendingMigrations);
            _dbMigrator = new DbMigrator(_mockConfiguration.Object, _mockContext.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _dbMigrator.Scaffold("Migration1", "TestNamespace", false));
        }

        [Fact]
        public void ValidateMigrationIds_ShouldReturnTrue_WhenAllIdsAreValid()
        {
            // Arrange
            var migrationIds = new[] { "Migration1", "Migration2" };

            // Act
            var result = _dbMigrator.ValidateMigrationIds(migrationIds);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateMigrationIds_ShouldReturnFalse_WhenAnyIdIsNullOrEmpty()
        {
            // Arrange
            var migrationIds = new[] { "Migration1", "" };

            // Act
            var result = _dbMigrator.ValidateMigrationIds(migrationIds);

            // Assert
            Assert.False(result);
        }
    }
}