param(
    [String]$imagePath = ".\images",
    [String]$apiHostname = "localhost",
    [int]$apiPort = 5000,
    [String]$apiPathPrefix,
    [String]$username,
    [String]$password
)

if (Test-Path -Path $imagePath) {
    $imagePath = (Resolve-Path -Path $imagePath).Path;
}

if ($username -ne [String]::Empty) {
    $securePassword = ConvertTo-SecureString $password -AsPlainText -Force
    $credential = New-Object System.Management.Automation.PSCredential ($username, $securePassword)
}

.\import-designations.ps1 -apiHostname $apiHostname -apiPort $apiPort -apiPathPrefix $apiPathPrefix -credential $credential
.\import-skills.ps1 -apiHostname $apiHostname -apiPort $apiPort -apiPathPrefix $apiPathPrefix -credential $credential
.\import-staff.ps1 -csvPath "grad_data.csv" -imagePath $imagePath -apiHostname $apiHostname -apiPort $apiPort -apiPathPrefix $apiPathPrefix -credential $credential
.\import-staff.ps1 -csvPath "student_data.csv" -imagePath $imagePath -apiHostname $apiHostname -apiPort $apiPort -apiPathPrefix $apiPathPrefix -credential $credential
