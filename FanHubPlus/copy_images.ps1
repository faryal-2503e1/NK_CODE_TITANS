$artDir = 'C:\Users\Dell\.gemini\antigravity\brain\4c9f84ee-3986-42c3-869f-94a46a98e2a4'

$dsHoodie = Get-ChildItem -Path $artDir -Filter 'ds_hoodie_hd*.png' | Select-Object -First 1
$genshinMat = Get-ChildItem -Path $artDir -Filter 'genshin_mat_hd*.png' | Select-Object -First 1
$luffyFig = Get-ChildItem -Path $artDir -Filter 'luffy_fig_hd*.png' | Select-Object -First 1
$sungStatue = Get-ChildItem -Path $artDir -Filter 'sung_statue_hd*.png' | Select-Object -First 1
$sungPortrait = Get-ChildItem -Path $artDir -Filter 'sung_jinwoo_hd*.png' | Select-Object -First 1
$heroCyber = Get-ChildItem -Path $artDir -Filter 'hero_cyber_hd*.png' | Select-Object -First 1

if ($dsHoodie) { Copy-Item $dsHoodie.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\merchandise\demon-slayer-hoodie.png' -Force }
if ($genshinMat) { Copy-Item $genshinMat.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\merchandise\genshin-poster.png' -Force }
if ($luffyFig) { 
    Copy-Item $luffyFig.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\merchandise\luffy-figure.png' -Force 
    Copy-Item $luffyFig.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\explore\one-piece.png' -Force
}
if ($sungStatue) { Copy-Item $sungStatue.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\merchandise\sung-jinwoo-figure.png' -Force }
if ($sungPortrait) {
    Copy-Item $sungPortrait.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\content\characters\sung-jinwoo.png' -Force
    Copy-Item $sungPortrait.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\explore\solo-leveling.png' -Force
}
if ($heroCyber) {
    Copy-Item $heroCyber.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\images\home\home-hero-art.png' -Force
    Copy-Item $heroCyber.FullName 'C:\Users\Dell\Pictures\FanHubPlus\FanHubPlus\wwwroot\assets\images\hero\anime-hero.png' -Force
}

Write-Host "HD Images copied successfully!"
