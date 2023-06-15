using Dapper;
using Microsoft.Extensions.FileProviders.Embedded;
using MySql.Data.MySqlClient;
using Quartz.Impl.AdoJobStore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Domain.Repositories
{
    /// <summary>
    /// QuartzMySql持久化仓储
    /// </summary>
    public class QuartzMySqlRepository : QuartzBaseRepository
    {
        private readonly IDbProvider dbProvider;

        public QuartzMySqlRepository(IDbProvider dbProvider):base(dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public override async Task<int> InitTables()
        {
            using (var connection = new MySqlConnection(dbProvider.ConnectionString))
            {
                var check_sql = @"SELECT
	                                        COUNT(1)
                                        FROM
	                                        information_schema. TABLES
                                        WHERE
	                                        table_name IN (
		                                        'QRTZ_BLOB_TRIGGERS',
		                                        'QRTZ_CALENDARS',
		                                        'QRTZ_CRON_TRIGGERS',
		                                        'QRTZ_FIRED_TRIGGERS',
		                                        'QRTZ_JOB_DETAILS',
		                                        'QRTZ_LOCKS',
		                                        'QRTZ_PAUSED_TRIGGER_GRPS',
		                                        'QRTZ_SCHEDULER_STATE',
		                                        'QRTZ_SIMPLE_TRIGGERS',
		                                        'QRTZ_SIMPROP_TRIGGERS',
		                                        'QRTZ_TRIGGERS'
	                                        );";
                var count = await connection.QueryFirstOrDefaultAsync<int>(check_sql);
                //初始化 建表
                if (count == 0)
                {

                    string init_sql = "";
                    
                    return await connection.ExecuteAsync(init_sql);
                }
            }
            return 0;
        }
    }
}
