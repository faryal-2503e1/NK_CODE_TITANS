$fullSql = Get-Content -Path "C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\setup_fanhub21.sql" -Raw -Encoding UTF8

# Find line USE [FanHub21]
$idx = $fullSql.IndexOf("USE [FanHub21]")
if ($idx -ge 0) {
    $dbPart = $fullSql.Substring($idx)
    $header = "USE [master];`nGO`nIF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FanHub21') BEGIN CREATE DATABASE [FanHub21]; END`nGO`n"
    $cleanSql = $header + $dbPart
    Set-Content -Path "C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\clean_setup_fanhub21.sql" -Value $cleanSql -Encoding UTF8
    Write-Host "Clean SQL file written! Size:" $cleanSql.Length
}
