using Dapper;
using Quartz.Impl.AdoJobStore.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Domain.Repositories
{
    public class QuartzSqlServerRepository : QuartzBaseRepository
    {
        IDbProvider dbProvider;

        public QuartzSqlServerRepository(IDbProvider dbProvider):base(dbProvider)
        {
            this.dbProvider = dbProvider;
        }
        
        public override async Task<int> InitTables()
        {
            using (var connection = new SqlConnection(dbProvider.ConnectionString))
            {
                var check_sql = @"SELECT
	                                    COUNT (1)
                                    FROM
	                                    sys.tables
                                    WHERE
	                                    name IN (
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
                    string init_sql = await File.ReadAllTextAsync("Tables/tables_sqlServer.sql");
                    return await connection.ExecuteAsync(init_sql);
                }
            }
            return 0;
        }
    }
}
