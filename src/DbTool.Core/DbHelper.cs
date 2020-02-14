﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using DbTool.Core.Entity;
using WeihanLi.Extensions;

namespace DbTool.Core
{
    /// <summary>
    /// 数据库操作查询帮助类
    /// </summary>
    public class DbHelper : IDisposable
    {
        private readonly DbConnection _conn;

        private readonly IDbProvider _dbProvider;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName => _conn.Database;

        public DbHelper(string connString, IDbProvider dbProvider)
        {
            if (connString.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(connString));
            }

            _dbProvider = dbProvider;
            _conn = _dbProvider.GetDbConnection(connString);
        }

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <returns></returns>
        public List<TableEntity> GetTablesInfo()
        {
            return _conn.Query<TableEntity>(_dbProvider.QueryDbTablesSqlFormat, new { dbName = DatabaseName }).ToList();
        }

        /// <summary>
        /// 获取数据库表的列信息
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public List<ColumnEntity> GetColumnsInfo(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }
            return _conn.Query<ColumnEntity>(
                    _dbProvider.QueryTableColumnsSqlFormat,
                new { dbName = DatabaseName, tableName }).ToList();
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _conn?.Dispose();
                _disposed = true;
            }
        }
    }
}
