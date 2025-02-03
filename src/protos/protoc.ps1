param(
    [string]$baseProtoPath,
    [string]$outputPath
)

#create output dir if it does not exist
New-Item -ItemType Directory -Path $outputPath -Force

# Get all proto files
$protoFiles = Get-ChildItem -Path $baseProtoPath -Filter '*.proto' -Recurse -File | Select-Object -ExpandProperty FullName;
$uniqueProtoFiles = $protoFiles | Sort-Object -Unique;
Write-Host 'Files to be processed:';
$uniqueProtoFiles | ForEach-Object { Write-Host $_ };
Write-Host '';

# Get all subdirectories in the base proto path
$protoPaths = Get-ChildItem -Path $baseProtoPath -Directory -Recurse | Select-Object -ExpandProperty FullName
$protoPathArgs = ($protoPaths | ForEach-Object { "--proto_path=$_" }) -join " "
Write-Host 'Proto files directories:'
$protoPathArgs | ForEach-Object { Write-Host $_ };
Write-Host '';

$protocCommand = "protoc $protoPathArgs --proto_path=$baseProtoPath --csharp_opt=file_extension=.g.cs --csharp_out=$outputPath $uniqueProtoFiles"
Write-Host 'Final protoc command:'
Write-Host $protocCommand
Invoke-Expression $protocCommand
