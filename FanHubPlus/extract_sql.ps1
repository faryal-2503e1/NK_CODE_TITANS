$logPath = "C:\Users\Dell\.gemini\antigravity\brain\4c9f84ee-3986-42c3-869f-94a46a98e2a4\.system_generated\logs\transcript_full.jsonl"
$raw = Get-Content -Path $logPath -Raw -Encoding UTF8
$lines = $raw -split "`n"
foreach ($l in $lines) {
    if ($l -like "*USE [master]*") {
        $json = $l | ConvertFrom-Json
        if ($json.type -eq "USER_INPUT") {
            $txt = $json.content
            $idx = $txt.IndexOf("USE [master]")
            if ($idx -ge 0) {
                $sql = $txt.Substring($idx)
                Set-Content -Path "C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\setup_fanhub21.sql" -Value $sql -Encoding UTF8
                Write-Host "Extracted SQL length: $($sql.Length)"
            }
        }
    }
}
