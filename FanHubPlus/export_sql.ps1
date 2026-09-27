param(
    [string]$Server = "DESKTOP-4OEQRJ0",
    [string]$Database = "FanHub21",
    [string]$User = "sa",
    [string]$Password = "aptech",
    [string]$OutputFile = "C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\FanHub21_Database_Full.sql"
)

Add-Type -AssemblyName "System.Data"

$connStr = "Server=$Server;Database=$Database;User Id=$User;Password=$Password;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)

try {
    $conn.Open()
    Write-Host "Connected successfully to $Database on $Server!"
    
    $sb = [System.Text.StringBuilder]::new()
    [void]$sb.AppendLine("-- =========================================================")
    [void]$sb.AppendLine("-- FanHub21 Complete SQL Server Database Script (Schema + Data)")
    [void]$sb.AppendLine("-- Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
    [void]$sb.AppendLine("-- =========================================================")
    [void]$sb.AppendLine("")
    [void]$sb.AppendLine("USE [master];")
    [void]$sb.AppendLine("GO")
    [void]$sb.AppendLine("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '$Database') BEGIN")
    [void]$sb.AppendLine("    CREATE DATABASE [$Database];")
    [void]$sb.AppendLine("END")
    [void]$sb.AppendLine("GO")
    [void]$sb.AppendLine("USE [$Database];")
    [void]$sb.AppendLine("GO")
    [void]$sb.AppendLine("")

    # Get tables
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME"
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
    $tablesTable = New-Object System.Data.DataTable
    [void]$adapter.Fill($tablesTable)

    foreach ($tRow in $tablesTable.Rows) {
        $tableName = $tRow["TABLE_NAME"]
        Write-Host "Exporting schema & data for: $tableName..."
        
        [void]$sb.AppendLine("-- ---------------------------------------------------------")
        [void]$sb.AppendLine("-- Table Structure for [$tableName]")
        [void]$sb.AppendLine("-- ---------------------------------------------------------")
        
        # Get Column definitions
        $colCmd = $conn.CreateCommand()
        $colCmd.CommandText = @"
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    CHARACTER_MAXIMUM_LENGTH, 
    IS_NULLABLE,
    COLUMNPROPERTY(OBJECT_ID('$tableName'), COLUMN_NAME, 'IsIdentity') as IS_IDENTITY
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = '$tableName'
ORDER BY ORDINAL_POSITION
"@
        $colAdapter = New-Object System.Data.SqlClient.SqlDataAdapter($colCmd)
        $colsDt = New-Object System.Data.DataTable
        [void]$colAdapter.Fill($colsDt)

        [void]$sb.AppendLine("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[$tableName]') AND type in (N'U'))")
        [void]$sb.AppendLine("BEGIN")
        [void]$sb.AppendLine("CREATE TABLE [dbo].[$tableName] (")
        
        $colDefs = @()
        foreach ($cRow in $colsDt.Rows) {
            $colName = $cRow["COLUMN_NAME"]
            $dataType = $cRow["DATA_TYPE"]
            $maxLen = $cRow["CHARACTER_MAXIMUM_LENGTH"]
            $isNullable = if ($cRow["IS_NULLABLE"] -eq "YES") { "NULL" } else { "NOT NULL" }
            $isIdentity = if ($cRow["IS_IDENTITY"] -eq 1) { "IDENTITY(1,1)" } else { "" }

            $typeStr = $dataType
            if ($dataType -in @("varchar", "nvarchar", "char", "nchar", "binary", "varbinary")) {
                if ($maxLen -eq -1) { $typeStr += "(max)" }
                elseif ($maxLen -gt 0) { $typeStr += "($maxLen)" }
            } elseif ($dataType -in @("decimal", "numeric")) {
                $typeStr += "(18,2)"
            }

            $colDefs += "    [$colName] [$dataType] $isIdentity $isNullable"
        }
        [void]$sb.AppendLine(($colDefs -join ",`n"))
        [void]$sb.AppendLine(");")
        [void]$sb.AppendLine("END")
        [void]$sb.AppendLine("GO")
        [void]$sb.AppendLine("")

        # Data export
        $dataCmd = $conn.CreateCommand()
        $dataCmd.CommandText = "SELECT * FROM [$tableName]"
        $dataAdapter = New-Object System.Data.SqlClient.SqlDataAdapter($dataCmd)
        $dt = New-Object System.Data.DataTable
        [void]$dataAdapter.Fill($dt)

        if ($dt.Rows.Count -gt 0) {
            $idCmd = $conn.CreateCommand()
            $idCmd.CommandText = "SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('$tableName') AND is_identity = 1"
            $hasIdentity = ($idCmd.ExecuteScalar() -gt 0)

            if ($hasIdentity) {
                [void]$sb.AppendLine("SET IDENTITY_INSERT [$tableName] ON;")
                [void]$sb.AppendLine("GO")
            }

            foreach ($row in $dt.Rows) {
                $cols = @()
                $vals = @()
                
                foreach ($col in $dt.Columns) {
                    $cols += "[$($col.ColumnName)]"
                    $val = $row[$col.ColumnName]
                    if ($val -eq [DBNull]::Value) {
                        $vals += "NULL"
                    } elseif ($col.DataType.Name -in @("Int32", "Int64", "Decimal", "Double", "Boolean", "Single", "Byte")) {
                        if ($col.DataType.Name -eq "Boolean") {
                            $vals += if ($val) { "1" } else { "0" }
                        } else {
                            $vals += "$val"
                        }
                    } else {
                        $escaped = "$val".Replace("'", "''")
                        $vals += "N'$escaped'"
                    }
                }
                
                $colStr = $cols -join ", "
                $valStr = $vals -join ", "
                [void]$sb.AppendLine("INSERT INTO [$tableName] ($colStr) VALUES ($valStr);")
            }

            if ($hasIdentity) {
                [void]$sb.AppendLine("SET IDENTITY_INSERT [$tableName] OFF;")
                [void]$sb.AppendLine("GO")
            }
            [void]$sb.AppendLine("GO")
            [void]$sb.AppendLine("")
        }
    }

    [System.IO.File]::WriteAllText($OutputFile, $sb.ToString(), [System.Text.Encoding]::UTF8)
    Write-Host "Complete database setup script successfully generated at: $OutputFile"
} catch {
    Write-Host "Error during export: $_"
} finally {
    $conn.Close()
}
