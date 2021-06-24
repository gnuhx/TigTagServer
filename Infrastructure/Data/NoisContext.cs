using System;
using System.Linq;
using System.Reflection;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ApplicationCore.Helpers;
using ApplicationCore.Extensions;
using Dapper;
using ApplicationCore.Entities.LogAggregate;
using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Entities.RelationAggregate;

namespace Infrastructure.Data
{
    public class NoisContext: IdentityDbContext<
        User, Role, int,
        UserClaim, UserRole, UserLogin,
        RoleClaim, UserToken>
    {
        private readonly string _connectionString;
        public NoisContext(DbContextOptions<NoisContext> options) : base(options)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Relation> Relations { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            InitMapping(builder);

            #region configure for stored procedure return model
            #if DEBUG
                        CreateStoredProcedures();
            #endif
            #endregion
        }
        #region Apply configurations

        private void InitMapping(ModelBuilder modelBuilder)
        {
            var type = typeof(IEntityTypeConfiguration<>);
            var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.Name == type.Name)).ToList();

            foreach (var configuration in typeConfigurations)
            {
                dynamic configurationInstance = Activator.CreateInstance(configuration);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }

        #endregion

        #region Database extensions
        public List<T> ExecuteStoredProcedure<T>(string query, Dictionary<string, string> parameters) where T : new()
        {
            try
            {
                var connect = Database.GetDbConnection();
                if (connect.State == ConnectionState.Closed)
                    connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var param in parameters.Keys)
                {
                    cmd.Parameters.Add(new SqlParameter($"@{param}", parameters[param].RemoveSpecificChar()));
                }

                List<T> result;
                using (var reader = cmd.ExecuteReader())
                {
                    result = reader.MapToList<T>();

                }
                return result;
            }
            catch (Exception)
            {
                Database.CloseConnection();
                throw;
            }

        }

        public List<T> ExecuteStoredProcedure<T>(string query, Dictionary<string, string> parameters, List<string> outputParam, out Dictionary<string, object> output) where T : class
        {
            DynamicParameters queryParams = new DynamicParameters();
            output = new Dictionary<string, object>();
            foreach (var param in parameters.Keys)
            {
                queryParams.Add($"@{param}", parameters[param]);
            }                
            if (outputParam.Any())
            {
                foreach (var param in outputParam)
                {
                    queryParams.Add($"@{param}", direction: ParameterDirection.Output, size: int.MaxValue);
                }
            }
            var procResultData = Query<T>(query, queryParams).ToList();
               
            if (outputParam.Any())
            {
                foreach (var param in outputParam)
                {
                    var value = queryParams.Get<object>(param);
                    output.Add(param, value);
                }
            }
            return procResultData;
        }
        #endregion

        private void CreateStoredProcedures()
        {
            
            var buildResPath = AppDomain.CurrentDomain.BaseDirectory;
            var buildResFolder = new DirectoryInfo(buildResPath);
            var dropSql = @"declare @procName varchar(500)
                                declare cur cursor

                                for select[name] from sys.objects where type = 'p'
                                open cur
                                fetch next from cur into @procName
                                while @@fetch_status = 0
                                begin
                                    exec('drop procedure [' + @procName + ']')
                                    fetch next from cur into @procName
                                end
                                close cur
                                deallocate cur";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(dropSql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            

            var folder = new DirectoryInfo(buildResFolder.Parent?.Parent?.Parent?.FullName + @"\App_Data\StoredProcedure");
            var storedProcedures = folder.GetFiles().ToList();
            foreach (var storedProcedure in storedProcedures)
            {
                var content = File.ReadAllText(storedProcedure.FullName);
                //Database.ExecuteSqlCommand(content);
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(content, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private IEnumerable<TResult> Query<TResult>(string storedProcedureName, DynamicParameters dynamicParameters)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result =  conn.Query<TResult>(storedProcedureName, dynamicParameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
