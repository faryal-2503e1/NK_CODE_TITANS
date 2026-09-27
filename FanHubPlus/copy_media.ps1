$src = 'C:\Users\Dell\Pictures\media_extracted\media'
$dest = 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\category_media'
if (-not (Test-Path $dest)) { New-Item -ItemType Directory -Path $dest -Force }
Copy-Item -Path "$src\*" -Destination $dest -Recurse -Force
Write-Host "Copied media_extracted files to wwwroot/images/category_media!"
