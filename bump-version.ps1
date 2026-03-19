$csproj = "PersonManager/PersonManager.csproj"
$content = Get-Content $csproj
$versionLine = $content | Where-Object { $_ -match '<Version>(.*)</Version>' }
$currentVersion = ($versionLine -replace '.*<Version>(.*)</Version>.*', '$1')
$parts = $currentVersion.Split('.')
$parts[2] = [int]$parts[2] + 1
$newVersion = "$($parts[0]).$($parts[1]).$($parts[2])"
$newContent = $content -replace "<Version>$currentVersion</Version>", "<Version>$newVersion</Version>"
Set-Content $csproj $newContent
Write-Host "Nowa wersja: $newVersion"
