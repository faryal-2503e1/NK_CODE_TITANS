$lines = Get-Content 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\clean_setup_fanhub21.sql'
$valid = $lines[0..1643]
$valid += 'SET IDENTITY_INSERT [dbo].[ChatbotQueries] OFF'
$valid += 'GO'
Set-Content -Path 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\clean_setup_fanhub21.sql' -Value $valid -Encoding UTF8
Write-Host "Trimmed clean_setup_fanhub21.sql successfully!"
