# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = $packFolder
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
	"Creekdream.AspNetCore",
    "Creekdream.Core",
	"Creekdream.Dependency.Windsor",
	"Creekdream.Mapping.Core",
	"Creekdream.Mapping.AutoMapper",
	"Creekdream.Orm.Dapper",
	"Creekdream.Orm.Dapper.MySql",
	"Creekdream.Orm.Dapper.Oracle",
	"Creekdream.Orm.EntityFrameworkCore"
)

# Rebuild solution
Set-Location $slnPath
& dotnet restore

# Copy all nuget packages to the pack folder
foreach($project in $projects) {
    
    $projectFolder = Join-Path $srcPath $project

    # Create nuget pack
    Set-Location $projectFolder
	$projectBinFolder = Join-Path $projectFolder "bin/Release"
    $isExistsBinFolder = Test-Path $projectBinFolder
	if($isExistsBinFolder -eq $True){
		Remove-Item -Recurse $projectBinFolder
	}
    & dotnet msbuild /p:Configuration=Release /p:SourceLinkCreate=true
    & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true

    # Copy nuget package
    $projectPackPath = Join-Path $projectBinFolder ($project + ".*.nupkg")
    Move-Item $projectPackPath $packFolder
}

# Go back to the pack folder
Set-Location $packFolder

Write-Host ""
Write-Host "Do you wish to post to microsoft nuget ?"
Write-Host ""
$isPublish = Read-Host 'Please enter y or n ?'
if ($isPublish -eq 'y') {
	$nugetAppId = Read-Host 'Please enter your nuget appid'
    foreach ($packfile in Get-ChildItem -Path $packFolder -Recurse -Include *.nupkg) {
		tools\nuget\nuget.exe push $packfile -Source https://www.nuget.org/api/v2/package $nugetAppId
    }
}
del *.nupkg
pause
exit
