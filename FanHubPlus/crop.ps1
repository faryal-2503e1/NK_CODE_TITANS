Add-Type -AssemblyName System.Drawing

$darkPath = "C:\Users\HP\Desktop\FanHubPlus\FanHubPlus\wwwroot\assets\images\dark-bg.png"
if (Test-Path $darkPath) {
    $img = [System.Drawing.Image]::FromFile($darkPath)
    $w = $img.Width - 45
    $h = $img.Height
    $bmp = New-Object System.Drawing.Bitmap($w, $h)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $srcRect = New-Object System.Drawing.Rectangle(45, 0, $w, $h)
    $destRect = New-Object System.Drawing.Rectangle(0, 0, $w, $h)
    $g.DrawImage($img, $destRect, $srcRect, [System.Drawing.GraphicsUnit]::Pixel)
    $img.Dispose()
    $g.Dispose()
    $bmp.Save($darkPath + ".clean.png")
    $bmp.Dispose()
    Move-Item -Path ($darkPath + ".clean.png") -Destination $darkPath -Force
    Write-Host "Dark background cleaned!"
}

$lightPath = "C:\Users\HP\Desktop\FanHubPlus\FanHubPlus\wwwroot\assets\images\light-bg.png"
if (Test-Path $lightPath) {
    $img2 = [System.Drawing.Image]::FromFile($lightPath)
    $w2 = $img2.Width - 45
    $h2 = $img2.Height
    $bmp2 = New-Object System.Drawing.Bitmap($w2, $h2)
    $g2 = [System.Drawing.Graphics]::FromImage($bmp2)
    $srcRect2 = New-Object System.Drawing.Rectangle(45, 0, $w2, $h2)
    $destRect2 = New-Object System.Drawing.Rectangle(0, 0, $w2, $h2)
    $g2.DrawImage($img2, $destRect2, $srcRect2, [System.Drawing.GraphicsUnit]::Pixel)
    $img2.Dispose()
    $g2.Dispose()
    $bmp2.Save($lightPath + ".clean.png")
    $bmp2.Dispose()
    Move-Item -Path ($lightPath + ".clean.png") -Destination $lightPath -Force
    Write-Host "Light background cleaned!"
}
